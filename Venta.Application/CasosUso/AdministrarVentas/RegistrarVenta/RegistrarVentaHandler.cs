using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Venta.Application.Common;
using Venta.Domain.Models;
using Venta.Domain.Repositories;
using Venta.Domain.Service.Events;
using Venta.Domain.Services.WebServices;
using Models = Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public  class RegistrarVentaHandler:
        IRequestHandler<RegistrarVentaRequest, IResult>
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ITipoFormaPagoRepository _tipoFormaPagoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IStocksService _stocksService;
        private readonly IPagosService _pagosService;
        private readonly ILogger _logger;
        private readonly IEventSender _eventSender;

        public RegistrarVentaHandler(IVentaRepository ventaRepository, IProductoRepository productoRepository, ITipoFormaPagoRepository tipoFormaPagoRepository, IClienteRepository clienteRepository, IMapper mapper,
            IStocksService stocksService, IPagosService pagosService, ILogger<RegistrarVentaHandler> logger, IEventSender eventSender)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _tipoFormaPagoRepository = tipoFormaPagoRepository;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _stocksService = stocksService;
            _pagosService = pagosService;
            _logger = logger;
            _eventSender = eventSender;
        }

        public async Task<IResult> Handle(RegistrarVentaRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IResult response = null;

                //Aplicando el automapper para convertir el objeto Request a venta dominio
                var venta = _mapper.Map<Models.Venta>(request);

                _logger.LogInformation($"Cantidad de productos {venta.Detalle.Count()}");


                foreach (var detalle in venta.Detalle)
                {
                    //1 - Validar si el productos existe
                    var productoEncontrado = await _productoRepository.Consultar(detalle.IdProducto);
                    if (productoEncontrado?.IdProducto <= 0)
                    {
                        throw new Exception($"Producto no encontrado, código {detalle.IdProducto}");
                    }
                    //Actualizar el detalle del pedido con el precio del producto
                    detalle.Precio = productoEncontrado.PrecioUnitario;

                    //2 - Reservar el stock del producto
                    //2.1 --Si sucedio algun erro al reservar el producto , retornar una exepcion
                    await _stocksService.ActualizarStock(detalle.IdProducto, detalle.Cantidad);
                }

                /// Registrar la venta
                /// 
                await _ventaRepository.Registrar(venta);

                var tipoFormaPagoEncontrado = await _tipoFormaPagoRepository.Consultar(request.Pago.IdTipoFormaPago);
                if (tipoFormaPagoEncontrado?.IdTipoFormaPago <= 0)
                {
                    throw new Exception($"Tipo de forma de pago no encontrado, código {request.Pago.IdTipoFormaPago}");
                }

                //Registrar el pago
                var pago = new Pago();
                pago.Monto = venta.Monto;
                pago.FormaPago = tipoFormaPagoEncontrado.FormaPago;
                pago.NumeroTarjeta = tipoFormaPagoEncontrado.NumeroTarjeta;
                pago.FechaVencimiento = tipoFormaPagoEncontrado.FechaVencimiento;
                pago.CVV = tipoFormaPagoEncontrado.CVV;
                pago.NombreTitular = tipoFormaPagoEncontrado.NombreTitular;
                pago.NumeroCuotas = request.Pago.NumeroCuotas;
                pago.IdVenta = venta.IdVenta;

                try
                {
                    var procesadoOk = await _pagosService.RegistrarPago(pago);

                    if(!procesadoOk)
                        await _eventSender.PublishAsync("pagos", JsonSerializer.Serialize(pago), cancellationToken);


                } catch (Exception e)
                {

                    //Durante el tiempo que este abierto el circuito, el microservicio de ventas debe dejar la información de la transacción en um tópico de Kafka, para luego ser procesado.
                    await _eventSender.PublishAsync("pagos", JsonSerializer.Serialize(pago), cancellationToken);
                }

                

                //El microservicio de venta debe publicar en un tópico de Kafka la información del
                //pedido, junto con los datos de entrega.
                var clienteEncontrado = await _clienteRepository.Consultar(venta.IdCliente);
                if (clienteEncontrado?.IdCliente <= 0)
                {
                    throw new Exception($"Cliente no encontrado, código {venta.IdCliente}");
                }

                var entrega = new Entrega();
                entrega.IdVenta = venta.IdVenta;
                entrega.NombreCliente = clienteEncontrado.Nombre.Trim() + " " + clienteEncontrado.Apellidos.Trim();
                entrega.DireccionEntrega = clienteEncontrado.DireccionEntrega;
                entrega.Ciudad = clienteEncontrado.Ciudad;

                List<EntregaDetalle> entregaDetalle = new List<EntregaDetalle> ();

                foreach (var detalle in venta.Detalle)
                {
                    
                    var productoEncontrado2 = await _productoRepository.Consultar(detalle.IdProducto);
                    if (productoEncontrado2?.IdProducto <= 0)
                    {
                        throw new Exception($"Producto no encontrado, código {detalle.IdProducto}");
                    }

                    entregaDetalle.Add(new EntregaDetalle { Producto = productoEncontrado2.Nombre, Cantidad = detalle.Cantidad });

                }

                entrega.Detalle = entregaDetalle;

                await _eventSender.PublishAsync("entregas", JsonSerializer.Serialize(entrega), cancellationToken);


                response = new SuccessResult<int>(venta.IdVenta);

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                throw ex;
            }
        }
        

    }
}
