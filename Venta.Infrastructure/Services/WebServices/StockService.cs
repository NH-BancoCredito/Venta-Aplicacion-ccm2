using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;
using Venta.Domain.Services.WebServices;

namespace Venta.Infrastructure.Services.WebServices
{
    public class StockService : IStocksService
    {

        public StockService(HttpClient httpClientStocks) { 
        }

        public Task<bool> ActualizarStock(Producto producto)
        {
            throw new NotImplementedException();
        }
    }
}
