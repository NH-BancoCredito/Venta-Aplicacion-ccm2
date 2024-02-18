using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Venta.Application.Common;
using Venta.Domain.Repositories;
using Models = Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoHandler :
        IRequestHandler<ActualizarProductoRequest, IResult>
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IMapper _mapper;

        public ActualizarProductoHandler(IProductoRepository productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        /* 
         1 - Deberia verificar si el productos
         Si existe , entoces actualizar en la table de producto
         Si no existe, crear un nuevo registro

         */
        public async Task<IResult> Handle(ActualizarProductoRequest request, CancellationToken cancellationToken)
        {
            IResult response = null;

            try
            {
                var producto = await _productoRepository.Consultar(request.IdProducto);

                if(producto == null)
                {

                    var productoIns = new Models.Producto();
                    var productoIns2 = _mapper.Map<Models.Producto>(request);

                    productoIns.Nombre = productoIns2.Nombre;
                    productoIns.Stock = productoIns2.Stock;
                    productoIns.StockMinimo = productoIns2.StockMinimo;
                    productoIns.PrecioUnitario = productoIns2.PrecioUnitario;
                    productoIns.IdCategoria = productoIns2.IdCategoria;

                    var insertar = await _productoRepository.Adicionar(productoIns);

                    if (insertar)
                        return new SuccessResult();
                    else
                        return new FailureResult();

                }
                else
                {
                    var productoAct = _mapper.Map<Models.Producto>(request);

                    var actualizar = await _productoRepository.Modificar(productoAct);

                    if (actualizar)
                        return new SuccessResult();
                    else
                        return new FailureResult();
                }

            }
            catch (Exception ex)
            {
                return new FailureResult();
            }

        }

    }

}
