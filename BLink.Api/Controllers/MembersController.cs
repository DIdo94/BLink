using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLink.Core.Services;
using Microsoft.AspNetCore.Authorization;
using BLink.Models;
using BLink.Models.RequestModels.Members;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLink.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class MembersController : Controller
    {
        private readonly IMembersService _membersService;
        private readonly IClubsService _clubsService;

        public MembersController(IMembersService membersService, IClubsService clubsService)
        {
            _membersService = membersService;
            _clubsService = clubsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMemberByEmail([FromQuery] string email)
        {
            return Json(await _membersService.GetMemberDetailsByEmail(email));
        }

        // TODO With memberId
        [HttpGet("{email}/club")]
        public IActionResult GetMemberClub([FromRoute] string email)
        {
            Club club = _clubsService.GetMemberClub(email);
            return Json(club);
        }

        [HttpGet("~/api/players")]
        public IActionResult GetPlayers([FromQuery] PlayerFilterCriteria filterCriteria)
        {
            IEnumerable<PlayerFilterResult> players = _membersService.GetPlayers(filterCriteria);
            return Json(players);
        }
    }
}
