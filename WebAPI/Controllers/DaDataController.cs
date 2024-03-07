using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    [ApiController]
    public class DaDataController(IDaDataService service, ILogger<DaDataController> logger, IMapper mapper) : ControllerBase
    {
        // GET: api/<DaDataController>
        [HttpGet("cleanAddress")]
        public async Task<IActionResult> GetClear([FromQuery] RawAddressData rawAddressData)
        {
            logger.LogInformation("Entered HTTP GET. Params: {0}", rawAddressData.Address);
            var cleanAddress = await service.GetJsonFromDaData(mapper.Map<AddressDto>(rawAddressData)).ConfigureAwait(false);
            logger.LogInformation("Out of HTTP GET. Result: {0}", cleanAddress);
            return Ok(cleanAddress);
        }
    }
}
