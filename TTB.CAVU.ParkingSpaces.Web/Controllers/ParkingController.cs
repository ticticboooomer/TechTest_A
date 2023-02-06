using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TTB.CAVU.ParkingSpaces.Services.Parking;
using TTB.CAVU.ParkingSpaces.Web.Model;

namespace TTB.CAVU.ParkingSpaces.Web.Controllers
{
    [Route("api/parking")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet("available")]
        public async Task<ActionResult> GetAvailable([FromBody]AvailableSpacesRequestModel model)
        {
            var response = await _parkingService.GetAvailableSpacesAsync(model.Start, model.End);
            return Ok(response);
        }
    }
}
