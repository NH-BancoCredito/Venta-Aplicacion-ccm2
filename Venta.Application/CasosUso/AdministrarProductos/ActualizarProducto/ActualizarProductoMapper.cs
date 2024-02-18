using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Models = Venta.Domain.Models;

namespace Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto
{
    public class ActualizarProductoMapper : Profile
    {
        public ActualizarProductoMapper()
        {
            CreateMap<ActualizarProductoRequest, Models.Producto>();

        }
    }
}
