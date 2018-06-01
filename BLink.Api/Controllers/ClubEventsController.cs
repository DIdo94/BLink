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
        public IActionResult GetEvents([FromQuery] ClubEventFilterRequest clubEventFilterRequest)
        {
            return Json(_clubEventsService.GetClubEvents(clubEventFilterRequest));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] ClubEventCreateRequest clubEventCreateRequest)
        {
            await _clubEventsService.CreateEvent(clubEventCreateRequest);
            await _clubEventsService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{eventId}")]
        public async Task<IActionResult> EditEvent([FromRoute] int eventId, [FromBody] ClubEventCreateRequest clubEventCreateRequest)
        {
            bool isSuccess = await _clubEventsService.EditEvent(eventId, clubEventCreateRequest);
            if (isSuccess)
            {
                await _clubEventsService.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }


        [HttpDelete("{eventId}")]
        public async Task<IActionResult> RemoveEvent([FromRoute] int eventId)
        {
            bool isSuccess = await _clubEventsService.RemoveEvent(eventId);
            if (isSuccess)
            {
                await _clubEventsService.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
