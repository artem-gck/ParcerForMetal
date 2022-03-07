using Microsoft.AspNetCore.Mvc;
using Parser.Serviсes;
using Parser.Serviсes.Models;
using Parser.Serviсes.Models.CertificateModel;
using Parser.Serviсes.ViewModel;

namespace Parcer.Controllers
{
    [Route("api/parcer")]
    public class ParcerController : Controller
    {
        private IMetalManager _service;

        public ParcerController(IMetalManager service)
            => _service = service;

        [HttpPost]
        public async Task<IActionResult> CreateFromLinkAsync([FromBody] CertificateLink certificateLink)
        {
            if (certificateLink is null)
            {
                return NoContent();
            }

            var id = await _service.CreateFromLinkAsync(certificateLink);

            return CreatedAtAction(nameof(CreateFromLinkAsync), id);
        }

        [HttpPost("certificate")]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] Certificate certificate)
        {
            if (certificate is null)
            {
                return NoContent();
            }

            var id = await _service.CreateCertificateAsync(certificate);

            return CreatedAtAction(nameof(CreateCertificateAsync), id);
        }

        [HttpGet("certificate/{id}")]
        public async Task<ActionResult<Certificate>> GetSertificateAsunc(int id)
            => await _service.GetCertificateAsync(id);

        [HttpGet("certificate")]
        public async Task<ActionResult<List<Certificate>>> GetAllSertificatesAsunc()
            => await _service.GetAllCertificatesAsync();

        [HttpGet("package")]
        public async Task<ActionResult<List<PackageViewModel>>> GetAllPackagesAsunc()
            => await _service.GetAllPackagesAsync();
    }
}