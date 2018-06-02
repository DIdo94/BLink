using BLink.Core.Constants;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.RequestModels.Clubs;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
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
        public async Task<IActionResult> CreateClub([FromForm] CreateClubModel createClubModel)
        {
            Club club = _clubsService.GetClubByName(createClubModel.Name);
            if (club != null)
            {
                return BadRequest("Името е заето");
            }

            await _clubsService.CreateClub(createClubModel);
            await _clubsService.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("{clubId}")]
        public async Task<IActionResult> EditClub([FromRoute] int clubId, [FromForm] EditClub editClubModel)
        {
            Club club = await _clubsService.GetClubById(clubId);
            if (club == null)
            {
                return BadRequest("Несъществуващ клуб");
            }

            Club clubByName = _clubsService.GetClubByName(editClubModel.Name);
            if (clubByName != null && clubByName.Id != club.Id)
            {
                return BadRequest("Името е заето");
            }

            await _clubsService.UpdateClub(club, editClubModel);
            await _clubsService.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{clubId}/players")]
        public async Task<IActionResult> GetClubPlayers([FromRoute] int clubId, [FromQuery] PlayerFilterCriteria filterCriteria)
        {
            filterCriteria.ClubId = clubId;
            var players = await _membersService.GetPlayers(filterCriteria);
            return Json(players);
        }

        [HttpPost("{clubId}/invite-player")]
        public async Task<IActionResult> InvitePlayer([FromRoute] int clubId, [FromBody]CreateInvitation createInvitation)
        {
            var club = await _clubsService.GetClubById(clubId);
            if (club == null)
            {
                return BadRequest();
            }

            if (createInvitation != null && ModelState.IsValid)
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

        [HttpGet("{clubId}/mainPhoto")]
        public async Task<IActionResult> GetClubPhoto([FromRoute] int clubId)
        {
            var club = await _clubsService.GetClubById(clubId);
            if (club == null)
            {
                return BadRequest();
            }

            var path = Path.Combine(
                           AppConstants.DataFilesPath,
                           club.PhotoPath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        [HttpPost("{clubId}/kickPlayer/{playerId}")]
        public async Task<IActionResult> KickPlayer([FromRoute] int clubId, [FromRoute] int playerId)
        {
            bool isSuccess = await _clubsService.KickPlayer(clubId, playerId);
            if (isSuccess)
            {
                await _clubsService.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
            };
        }
    }
}
