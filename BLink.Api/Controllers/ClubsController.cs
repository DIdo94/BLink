using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Clubs;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLink.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClubsController : Controller
    {
        private readonly IClubsService _clubsService;
        private readonly IMembersService _membersService;
        public ClubsController(IClubsService clubsService, IMembersService membersService)
        {
            _clubsService = clubsService;
            _membersService = membersService;
        }

        // GET: api/values
        [HttpGet]
        public IActionResult GetClubs()
        {
            return Json(_clubsService.GetAllClubs());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClub([FromBody] CreateClubModel createClubModel)
        {
            Club club = new Club
            {
                Name = createClubModel.Name,
            };

            // var str = User.Identity.Name; TODO work with this
            Member member = await _membersService.GetMemberByEmail(createClubModel.Email);
            club.Members.Add(member);
            await _clubsService.CreateClub(club);
            await _clubsService.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{clubId}/players")]
        public IActionResult GetClubPlayers([FromRoute] int clubId, [FromQuery] PlayerFilterCriteria filterCriteria)
        {
            filterCriteria.ClubId = clubId;
            var players = _membersService.GetPlayers(filterCriteria);
            return Json(players);
        }
    }
}
