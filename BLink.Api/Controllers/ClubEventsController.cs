using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLink.Core.Services;
using BLink.Models.RequestModels.ClubEvents;
using Microsoft.AspNetCore.Authorization;

namespace BLink.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClubEventsController : Controller
    {
        private readonly IClubEventsService _clubEventsService;

        public ClubEventsController(IClubEventsService clubEventsService)
        {
            _clubEventsService = clubEventsService;
        }

        [HttpGet]
        public IEnumerable<ClubEventFilterResult> GetEvents([FromQuery] ClubEventFilterRequest clubEventFilterRequest)
        {
            return _clubEventsService.GetClubEvents(clubEventFilterRequest);
        }

        [HttpPost]
        public async Task CreateEvent([FromBody] ClubEventCreateRequest clubEventCreateRequest)
        {
            await _clubEventsService.CreateEvent(clubEventCreateRequest);
            await _clubEventsService.SaveChangesAsync();
        }
    }
}
