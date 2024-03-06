using AutoMapper;
using Domain.Entities;
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
            try
            {
                logger.LogInformation($"Entered HTTP GET. Params: {rawAddressData.Address}");
                var cleanAddress = await service.GetJsonFromDaData(mapper.Map<AddressDto>(rawAddressData)).ConfigureAwait(false);
                logger.LogInformation($"Out of HTTP GET. Result: {cleanAddress}");
                return Ok(cleanAddress);
            }
            catch (Exception ex)
            {
                logger.LogError($"DaDataController HTTP GET: {ex.Message}");
                return StatusCode(500, "An error occurred while cleaning the address.");
            }
        }
    }
}
