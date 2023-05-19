using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Services;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VentaController : ControllerBase
    {
        private IVentaService _ventaService;
        public VentaController(IVentaService _ventaService) {
            
            this._ventaService = _ventaService;
        
        }


        [HttpPost]
        public IActionResult Add(VentaRequest model)
        {

            Respuesta respuesta = new Respuesta();

            try
            {
                _ventaService.add(model);
                respuesta.Exito = 1;
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
