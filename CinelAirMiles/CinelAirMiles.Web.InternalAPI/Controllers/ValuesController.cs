namespace CinelAirMiles.Web.InternalAPI.Controllers
{
    using System.Threading.Tasks;

    using CinelAirMiles.Common.Services;
    using CinelAirMiles.Web.InternalAPI.Helpers;
    using CinelAirMiles.Web.InternalAPI.Responses;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        const string TicketsUrl = "http://192.168.193.7:3000";
        const string Prefix = "";
        const string Controller = "/list";
        private readonly IApiService _apiService;
        private readonly IMilesHelper _milesHelper;

        public ValuesController(
            IApiService apiService,
            IMilesHelper milesHelper
            )
        {
            _apiService = apiService;
            _milesHelper = milesHelper;
        }

        // GET api/
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var response = await _apiService.GetItemAsync<TicketList>
                                    (TicketsUrl, Prefix, Controller);

            if (response.IsSuccess)
            {
                var ticketList = (TicketList)response.Result;

                var readTickets = await _milesHelper.TicketCheckerAsync(ticketList);

                return Ok(readTickets);
            }

            return StatusCode(503);
        }
    }
}
