using MediatR;
using Microsoft.AspNetCore.Mvc;
using Venta.Application.CasosUso.AdministrarProductos.ActualizarProducto;
using Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos;

namespace Venta.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configInfo;

        public ProductosController(IMediator mediator, IConfiguration configInfo)
        {
            _mediator = mediator;
            _configInfo = configInfo;
        }

        [HttpGet("consultar")]
        public async Task<IActionResult> Consultar([FromQuery] ConsultarProductosRequest request)
        {
            //var dado1 = _configInfo["dbVenta-cnx"];
            var response = await _mediator.Send(request);

            return Ok(response);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarProductoRequest request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
