using BLink.Core.Constants;
using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Invitations;
using BLink.Models.RequestModels.Members;
using System;
using System.Collections.Generic;
using System.IO;
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
            var memberDetails = new MemberDetails
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Height = member.Height,
                PhotoPath = member.PhotoPath,
                Weight = member.Weight
            };

            if (member.MemberPositions.Any())
            {
                Position position = _membersRepository
                    .GetPositionById(member.MemberPositions.First().PostitionId);
                memberDetails.PreferedPosition = 
                    (PlayerPosition?)Enum.Parse(typeof(PlayerPosition), position.Name, true);
            }

            return memberDetails;
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

        public async Task<bool> EditMemberDetails(string email, EditMemberDetails editMemberDetails)
        {
            var member = await GetMemberByEmail(email);
            if (member == null)
            {
                return false;
            }

            var path = Path.Combine(
                     AppConstants.DataFilesPath,
                     email,
                     editMemberDetails.UserImage.FileName);
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await editMemberDetails.UserImage.CopyToAsync(stream);
            }

            member.FirstName = editMemberDetails.FirstName;
            member.LastName = editMemberDetails.LastName;
            member.MemberPositions = new List<MemberPositions>();
            if (editMemberDetails.PreferedPosition.HasValue)
            {
                member.MemberPositions.Add(new MemberPositions()
                {
                    Position = new Position
                    {
                        Name = editMemberDetails.PreferedPosition.Value.ToString()
                    }
                });
            }

            member.Weight = editMemberDetails.Weight;
            member.Height = editMemberDetails.Height;
            member.PhotoPath = path;

            _membersRepository.EditMember(member);
            return true;
        }

        public Position GetPositionByName(string name)
        {
            return _membersRepository.GetPositionByName(name);
        }
    }
}
