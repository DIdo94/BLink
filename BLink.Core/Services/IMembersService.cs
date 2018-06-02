using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Accounts;
using BLink.Models.RequestModels.Invitations;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLink.Core.Services
{
    public interface IMembersService
    {
        IEnumerable<Member> GetAllMembers();

        Task<Member> GetMemberByEmail(string email);

        Task<MemberDetails> GetMemberDetailsByEmail(string email);

        Task AddMemberAsync(Member member);

        Task SaveChangesAsync();

        Task<IEnumerable<PlayerFilterResult>> GetPlayers(PlayerFilterCriteria filterCriteria);

        Task<Member> GetPlayerById(int playerId);

        Task<IEnumerable<InvitationResponse>> GetMemberInvitations(string email);

        Task RespondInvitation(int invitationId, InvitationStatus invitationStatus, Member member);

        Task<bool> EditMemberDetails(string email, EditMemberDetails editMemberDetails);

        Position GetPositionByName(string name);

        Task<bool> LeaveClub(string email);

        Task<ApplicationUser> BuildUser(CreateUserViewModel userViewModel);
    }
}
