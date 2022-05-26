using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        public async Task<ActionResult<Certificate>> CreateFromLinkAsync([FromForm] CertificateLink certificateLink) //Add certificate
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificateLink is null)
                    return BadRequest();

                var certificate = await _metalService.CreateFromLinkAsync(certificateLink);

                return certificate;
            }
            else
                return Unauthorized();
        }

        [HttpPost("certificate/check")]
        public async Task<ActionResult<Certificate>> ChackCertificateByLinkAsync([FromForm] CertificateLink certificateLink)
        {
            if (certificateLink is null)
                return BadRequest();

            var certificate = await _metalService.CreateFromLinkAsync(certificateLink);

            return certificate;
        }

        [HttpPost("certificate")]
        public async Task<IActionResult> CreateCertificateAsync([FromBody] Certificate certificate)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                if (certificate is null)
                    return NoContent();

                var id = await _metalService.CreateCertificateAsync(certificate);

                return CreatedAtAction(nameof(CreateCertificateAsync), id);
            }
            else
                return Unauthorized();
        }

        //[HttpPost("package")]
        //public async Task<ActionResult<int>> AddPackage([FromBody] Package package)
        //{
        //    return await _metalService.AddPackage(package);
        //}

        [HttpPost("package/{id}")]
        public async Task<ActionResult<int>> AddPackage(int id, [FromBody]Package package)
        {
            package.CertificateId = id;
            return await _metalService.AddPackage(package);
        }

        [HttpGet("certificate")]
        public async Task<ActionResult<List<Certificate>>> GetAllSertificatesAsync()
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                return await _metalService.GetAllCertificatesAsync();
            else
                return Unauthorized();
        }

        [HttpPut("certificate/{id}")]
        public async Task<IActionResult> UpdateSertificateAsync(Certificate certificate)
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
        public async Task<ActionResult<Certificate>> GetSertificateAsync(int id)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                var certificate = await _metalService.GetCertificateAsync(id);

                return certificate is null ? NoContent() : certificate;
            }
            else
                return Unauthorized();
        }

        [HttpGet("package")]
        public async Task<ActionResult<List<PackageViewModel>>> GetAllPackagesAsync(string status = null)
        {
            //if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
                if (status is null)
                {
                    return await _metalService.GetAllPackagesAsync();
                }
                else
                    return await _metalService.GetPackagesByStatus(status);

            //else
            //    return Unauthorized();
        }

        [HttpPut("package")]
        public async Task<IActionResult> UpdateStatusPackageAsync(string batch, string status)
        {
            if (await _tokenService.CheckAccessKey(Request.Headers[_headerName].ToString()))
            {
                await _metalService.UpdateStatusPackageAsync(batch, status);

                return Ok();
            }

            return Unauthorized();
        }

        [HttpPut("package/defect")]
        public async Task<ActionResult<int>> AddDefectToPackageAsync([FromBody] Defect defect)
        {
            //var file = Request.Form.Files.FirstOrDefault();
            //var fileStream = file.OpenReadStream();
            //using var memoryStream = new MemoryStream();

            //fileStream.CopyTo(memoryStream); 
            //defect.Photo = memoryStream.ToArray();

            return await _metalService.AddDeffectToPackage(defect);
        }
    }
}