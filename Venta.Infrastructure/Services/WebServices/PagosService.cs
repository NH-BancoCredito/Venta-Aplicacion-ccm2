using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Venta.Domain.Models;
using Venta.Domain.Services.WebServices;
using Newtonsoft.Json;

namespace Venta.Infrastructure.Services.WebServices
{
    public class PagosService : IPagosService
    {
        private readonly HttpClient _httpClientPagos;
        public PagosService(HttpClient httpClientPagos) {
            _httpClientPagos = httpClientPagos;
        }

        public async Task<bool> RegistrarPago(Pago pago)
        {
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Post, "api/pagos/registrar");

                var entidadSerializada = System.Text.Json.JsonSerializer.Serialize(pago);
                request.Content = new StringContent(entidadSerializada, Encoding.UTF8, MediaTypeNames.Application.Json);

                var response = _httpClientPagos.SendAsync(request).Result;

                var resultado = JsonConvert.DeserializeObject<Resultado>(response.Content.ReadAsStringAsync().Result);

                return resultado.HasSucceeded;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }
}
