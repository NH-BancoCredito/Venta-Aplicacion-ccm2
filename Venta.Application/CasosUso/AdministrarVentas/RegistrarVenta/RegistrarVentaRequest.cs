using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Application.CasosUso.AdministrarVentas.RegistrarVenta
{
    public class RegistrarVentaRequest
    {

        public int IdCliente {  get; set; }

        public IEnumerable<RegistrarVentaDetalleRequest> Productos { get; set; }

    }

    public class RegistrarVentaDetalleRequest
    {
        public int IdProducto { get; set; }

        public int Cantidad { get; set; }

        public int Precio { get; set; }

    }

}
