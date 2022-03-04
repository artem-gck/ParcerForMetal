using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.Certificate;

namespace Parcer.Controllers
{
    [Route("api/parcer")]
    public class ParcerController : Controller
    {
        private IParserManager _service;

        public ParcerController(IParserManager service)
            => _service = service;

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CertificateLink certificateLink)
        {
            if (certificateLink is null)
            {
                return NoContent();
            }

            var id = await _service.CreateCertificateAsync(certificateLink);

            return CreatedAtAction(nameof(CreateAsync), id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Certificate>> GetAsunc(int id)
            => await _service.GetCertificateAsync(id);
    }
}