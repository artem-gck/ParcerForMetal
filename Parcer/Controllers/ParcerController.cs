using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes;
using Parser.Serviсes.Models;

namespace Parcer.Controllers
{
    [Route("api/parcer")]
    public class ParcerController : Controller
    {
        private IParserManager _service;

        public ParcerController(IParserManager service)
            => _service = service;

        [HttpPost]
        public async Task<IActionResult> Create(CertificateLink link)
        {
            if (link is null)
            {
                return NoContent();
            }

            var id = await _service.CreateCertificateAsync(link);

            return CreatedAtAction(nameof(Create), id);
        }
    }
}