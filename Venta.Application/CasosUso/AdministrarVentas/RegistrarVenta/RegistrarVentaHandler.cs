using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Repositories;
using Models = Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public  class RegistrarVentaHandler:
        IRequestHandler<RegistrarVentaRequest, RegistrarVentaResponse>
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        public RegistrarVentaHandler(IVentaRepository ventaRepository, IProductoRepository productoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        public async Task<RegistrarVentaResponse> Handle(RegistrarVentaRequest request, CancellationToken cancellationToken)
        {
            var response = new RegistrarVentaResponse();

            //Aplicando el automapper para convertir el objeto Request a venta dominio
            var venta = _mapper.Map<Models.Venta>(request);

            ///============Condiciones de validaciones


            foreach (var detalle in venta.Detalle)
            {
                //1 - Validar si el productos existe
                var productoEncontrado = await _productoRepository.Consultar(detalle.IdProducto);
                if (productoEncontrado?.IdProducto <= 0)
                {
                    throw new Exception($"Producto no encontrado, código {detalle.IdProducto}");
                }



                //2 - Validar si existe stock suficiente - TODO

                //3 - Reservar el stock del producto - TODO
                //3.1 --Si sucedio algun erro al reservar el producto , retornar una exepcion
            }

            /// SI todo esta OK
            /// Registrar la venta - TODO
            /// 
            await _ventaRepository.Registrar(venta);

            response.VentaRegistrada = venta.IdVenta>0;

            return response;
        }
        

    }
}
