using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Invitations;
using BLink.Models.RequestModels.Members;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class MembersService : IMembersService
    {
        private readonly IMembersRepository _membersRepository;
        private readonly IInvitationsRepository _invitationsRepository;
        public MembersService(IMembersRepository membersRepository, IInvitationsRepository invitationsRepository)
        {
            _membersRepository = membersRepository;
            _invitationsRepository = invitationsRepository;
        }

        public async Task RespondInvitation(int invitationId, InvitationStatus invitationStatus, Member member)
        {
            var invitation = await _invitationsRepository.GetInvitationByIdAsync(invitationId, "InvitingClub.Members");
            if (invitation != null)
            {
                _invitationsRepository.RespondInvitation(invitation, invitationStatus);
                if (invitationStatus == InvitationStatus.Accepted)
                {
                    invitation.InvitingClub.Members.Add(member);
                }
            }
        }

        public Task AddMemberAsync(Member member)
        {
            return _membersRepository.AddMemberAsync(member);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _membersRepository.GetMembers();
        }

        public Task<Member> GetMemberByEmail(string email)
        {
            return _membersRepository.GetMemberByEmail(email);
        }

        public async Task<MemberDetails> GetMemberDetailsByEmail(string email)
        {
            Member member = await GetMemberByEmail(email);
            return new MemberDetails
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Height = member.Height,
                PhotoPath = member.PhotoPath,
                Weight = member.Weight
            };
        }

        public IEnumerable<InvitationResponse> GetMemberInvitations(string email)
        {
            var invitationResponses = new List<InvitationResponse>();
            var member = GetMemberByEmail(email);
            if (member == null)
            {
                return invitationResponses;
            }

            var invitations = _invitationsRepository.GetInvitations(i => i.InvitedPlayer.IdentityUser.Email == email);
            if (invitations.Any())
            {
                invitationResponses =  invitations.Select(i => new InvitationResponse
                {
                    ClubName = i.InvitingClub.Name,
                    Id = i.Id,
                    Description = i.Description
                }).ToList();
            }

            return invitationResponses;
        }

        public Task<Member> GetPlayerById(int playerId)
        {
            return _membersRepository.GetPlayerById(playerId);
        }

        public IEnumerable<PlayerFilterResult> GetPlayers(PlayerFilterCriteria filterCriteria)
        {
            var players = _membersRepository.GetPlayersByCriteria(filterCriteria);
            return players;
        }

        public Task SaveChangesAsync()
        {
            return _membersRepository.SaveChangesAsync();
        }
    }
}
