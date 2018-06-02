using BLink.Core.Constants;
using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Accounts;
using BLink.Models.RequestModels.Invitations;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
        private readonly IFileService _fileService;
        public MembersService(
            IMembersRepository membersRepository,
            IInvitationsRepository invitationsRepository,
            IFileService fileService)
        {
            _membersRepository = membersRepository;
            _invitationsRepository = invitationsRepository;
            _fileService = fileService;
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

        public async Task<IEnumerable<InvitationResponse>> GetMemberInvitations(string email)
        {
            var invitationResponses = new List<InvitationResponse>();
            var member = await GetMemberByEmail(email);
            if (member == null)
            {
                return invitationResponses;
            }

            IdentityRole coachRole = await _membersRepository.GetMemberRole(m => m.Name == Role.Coach.ToString());
            IEnumerable<Invitation> invitations = Enumerable.Empty<Invitation>();
            bool isCoach = member.IdentityUser.Roles.Any(r => coachRole.Id == r.RoleId);
            if (!isCoach && member.Club != null)
            {
                return invitationResponses;
            }

            if (member.IdentityUser.Roles.Any(r => coachRole.Id == r.RoleId))
            {
                invitations = _invitationsRepository.GetInvitations(i =>
                    i.InvitingClub.Id == member.Club.Id && i.Status == InvitationStatus.NotReplied);
            }
            else
            {
                invitations = _invitationsRepository.GetInvitations(i =>
                i.InvitedPlayer.IdentityUser.Email == email && i.Status == InvitationStatus.NotReplied);
            }

            if (invitations.Any())
            {
                invitationResponses = invitations.Select(i => new InvitationResponse
                {
                    ClubName = i.InvitingClub.Name,
                    PlayerName = $"{i.InvitedPlayer.FirstName} {i.InvitedPlayer.LastName}",
                    Id = i.Id,
                    Thumbnail = isCoach ? 
                        File.ReadAllBytes(i.InvitedPlayer.PhotoThumbnailPath) :
                        File.ReadAllBytes(i.InvitingClub.PhotoThumbnailPath),
                    Description = i.Description
                }).ToList();
            }

            return invitationResponses;
        }

        public Task<Member> GetPlayerById(int playerId)
        {
            return _membersRepository.GetPlayerById(playerId);
        }

        public async Task<IEnumerable<PlayerFilterResult>> GetPlayers(PlayerFilterCriteria filterCriteria)
        {
            var players = (await _membersRepository.GetPlayersByCriteria(filterCriteria)).ToList();
            if (players.Any())
            {
                foreach (var player in players)
                {
                    var positionName = _membersRepository.GetPositionById(player.PositionId).Name;
                    var position = (PlayerPosition?)Enum.Parse(typeof(PlayerPosition),
                       positionName, true);
                    player.PreferedPosition = position;
                    byte[] imageArray = File.ReadAllBytes(player.Thumbnail);
                    string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                    player.Thumbnail = base64ImageRepresentation;
                }
            }

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

            if (editMemberDetails.UserImage != null)
            {
                var path = Path.Combine(
                         AppConstants.DataFilesPath,
                         email,
                         editMemberDetails.UserImage.FileName);
                await _fileService.SaveImage(path, editMemberDetails.UserImage);
                string thumbnailPath = _fileService.CreateThumbnailFromImage(path, editMemberDetails.UserImage.FileName);
                member.PhotoPath = path;
                member.PhotoThumbnailPath = thumbnailPath;
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

            _membersRepository.EditMember(member);
            return true;
        }

        public Position GetPositionByName(string name)
        {
            return _membersRepository.GetPositionByName(name);
        }

        public async Task<bool> LeaveClub(string email)
        {
            var member = await GetMemberByEmail(email);
            if (member == null)
            {
                return false;
            }

            member.Club = null;
            _membersRepository.EditMember(member);

            return true;
        }

        public async Task<ApplicationUser> BuildUser(CreateUserViewModel userViewModel)
        {
            var path = Path.Combine(
                 AppConstants.DataFilesPath,
                 userViewModel.Email,
                 userViewModel.UserImage.FileName);
            await _fileService.SaveImage(path, userViewModel.UserImage);
            string thumbnailPath = _fileService.CreateThumbnailFromImage(path, userViewModel.UserImage.FileName);
            ApplicationUser user = new ApplicationUser
            {
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                Member = new Member
                {
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    Weight = userViewModel.Weight,
                    Height = userViewModel.Height,
                    PhotoPath = path,
                    PhotoThumbnailPath = thumbnailPath
                },
            };

            if (userViewModel.PreferedPosition.HasValue)
            {
                string positionName = userViewModel.PreferedPosition.Value.ToString();
                Position position = GetPositionByName(positionName);
                if (position == null)
                {
                    position = new Position
                    {
                        Name = positionName
                    };
                }

                MemberPositions memberPosition = new MemberPositions
                {
                    Member = user.Member,
                    Position = position
                };

                user.Member.MemberPositions.Add(memberPosition);
            }

            return user;
        }
    }
}
