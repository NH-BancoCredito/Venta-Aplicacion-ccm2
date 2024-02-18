using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Domain.Models
{
    public class TipoFormaPago
    {
        public int IdTipoFormaPago { get; set; }
        public int FormaPago { get; set; }
        public string NumeroTarjeta { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CVV { get; set; }
        public string NombreTitular { get; set; }

        //public virtual IEnumerable<Venta> Ventas { get; set; }

    }
}
