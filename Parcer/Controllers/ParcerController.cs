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
        private readonly IMetalManager _metalService;
        private readonly ITokenManager _tokenService;
        private readonly string _headerName;

        public ParcerController(IMetalManager metalService, ITokenManager tokenService, IConfiguration configuration)
            => (_metalService, _tokenService, _headerName) = (metalService, tokenService, configuration.GetSection("HeaderName").Value);

        [HttpPost]
        public async Task<IActionResult> CreateFromLinkAsync([FromBody] CertificateLink certificateLink)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificateLink is null)
                {
                    return BadRequest();
                }

                var id = await _metalService.CreateFromLinkAsync(certificateLink);

                return CreatedAtAction(nameof(CreateFromLinkAsync), id);
            }
            else
                return Unauthorized();
        }

        [HttpPost("certificate")]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] Certificate certificate)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificate is null)
                {
                    return NoContent();
                }

                var id = await _metalService.CreateCertificateAsync(certificate);

                return CreatedAtAction(nameof(CreateCertificateAsync), id);
            }
            else
                return Unauthorized();
        }

        [HttpPut("certificate/{id}")]
        public async Task<IActionResult> UpdateSertificateAsunc(Certificate certificate)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var certificateId = await _metalService.UpdateCertificateAsync(certificate);

                return Ok(certificateId);
            }
            else
                return Unauthorized();
        }

        [HttpGet("certificate/{id}")]
        public async Task<ActionResult<Certificate>> GetSertificateAsunc(int id)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var certificate = await _metalService.GetCertificateAsync(id);

                return certificate is null ? NoContent() : certificate;
            }
            else
                return Unauthorized();
        }

        [HttpGet("certificate")]
        public async Task<ActionResult<List<Certificate>>> GetAllSertificatesAsunc()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _metalService.GetAllCertificatesAsync();
            else
                return Unauthorized();
        }

        [HttpGet("package")]
        public async Task<ActionResult<List<PackageViewModel>>> GetAllPackagesAsunc()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _metalService.GetAllPackagesAsync();
            else
                return Unauthorized();
        }
    }
}