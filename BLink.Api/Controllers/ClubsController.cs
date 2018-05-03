using BLink.Core.Services;
using BLink.Models;
using BLink.Models.RequestModels.Clubs;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

        [HttpPost("{clubId}/invite-player")]
        public async Task<IActionResult> InvitePlayer([FromRoute] int clubId, [FromBody]CreateInvitation createInvitation)
        {
            var club = await  _clubsService.GetClubById(clubId);
            if (club == null)
            {
                return BadRequest();
            }

            if ( createInvitation != null && ModelState.IsValid)
            {
                var player = await _membersService.GetPlayerById(createInvitation.PlayerId);
                if (player == null)
                {
                    return BadRequest();
                }

                await _clubsService.InvitePlayer(club, player, createInvitation);
                await _clubsService.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }
    }
}
