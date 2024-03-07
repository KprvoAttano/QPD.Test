using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class DaDataController(IDaDataService service, IMapper mapper) : ControllerBase
    {
        // GET: api/<DaDataController>
        [HttpGet("cleanAddress")]
        public async Task<IActionResult> GetClear([FromQuery] RawAddressData rawAddressData, CancellationToken token)
        {
            var cleanAddress = await service.GetJsonFromDaDataAsync(mapper.Map<AddressDto>(rawAddressData), token).ConfigureAwait(false);
            return Ok(cleanAddress);
        }
    }
}
