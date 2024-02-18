using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Domain.Models
{
    public class Pago
    {
        
        public decimal Monto { get; set; }
        public int FormaPago { get; set; }
        public string NumeroTarjeta { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string CVV { get; set; }
        public string NombreTitular { get; set; }
        public int NumeroCuotas { get; set; }
        public int IdVenta { get; set; }

    }
}
