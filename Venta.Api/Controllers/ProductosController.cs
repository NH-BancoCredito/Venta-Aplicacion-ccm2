using MediatR;
using Microsoft.AspNetCore.Mvc;
using Venta.Application.CasosUso.AdministrarProductos.ConsultarProductos;

namespace Venta.Api.Controllers
{
    public class ProductosController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Consultar(ConsultarProductosRequest request)
        {
            var response = await _mediator.Send(request);

            return Ok(response);
        }
    }
}
