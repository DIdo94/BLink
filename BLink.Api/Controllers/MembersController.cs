using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BLink.Core.Services;
using Microsoft.AspNetCore.Authorization;
using BLink.Models;
using BLink.Models.RequestModels.Members;
using System.Collections.Generic;
using BLink.Models.RequestModels.Invitations;
using BLink.Models.Enums;
using System.IO;
using BLink.Core.Constants;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BLink.Api.Controllers
{
    //[Authorize]
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

        // TODO With memberId
        [HttpGet("{email}/invitations")]
        public IActionResult GetMemberInvitations([FromRoute] string email)
        {
            IEnumerable<InvitationResponse> invitationResponses = _membersService.GetMemberInvitations(email);
            return Json(invitationResponses);
        }

        // TODO With memberId
        [HttpPost("{email}/invitations/{invitationId}/respond")]
        public async Task<IActionResult> RespondInvitationAsync(
            [FromRoute] string email, 
            [FromRoute] int invitationId,
            [FromBody] RespondInvitationRequest respondInvitationRequest)
        {
            if (respondInvitationRequest.InvitationStatus != InvitationStatus.NotReplied)
            {
                // TODO check if is a logged user`s invitation
                var member = await _membersService.GetMemberByEmail(email);
                if (member == null)
                {
                    return BadRequest();
                }

                await _membersService.RespondInvitation(
                    invitationId, 
                    respondInvitationRequest.InvitationStatus, 
                    member);
                await _membersService.SaveChangesAsync();
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("{email}/mainPhoto")]
        public async Task<IActionResult> GetMemberMainPhoto([FromRoute] string email)
        {
            if (email == null)
            {
                return BadRequest();
            }

            var member = await _membersService.GetMemberByEmail(email);
            if (member == null)
            {
                return BadRequest();
            }

            var path = Path.Combine(
                           AppConstants.DataFilesPath,
                           member.PhotoPath);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        [HttpPost("{email}")]
        public async Task<IActionResult> EditMemberDetails([FromRoute] string email, [FromForm] EditMemberDetails editMemberDetails)
        {
            bool isSuccess = await _membersService.EditMemberDetails(email, editMemberDetails);
            if (isSuccess)
            {
                await _membersService.SaveChangesAsync();
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
