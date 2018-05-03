using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Clubs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class ClubsService : IClubsService
    {
        private readonly IClubsRepository _clubsRepository;
        private readonly IInvitationsRepository _invitationsRepository;
        private readonly IMembersRepository _membersRepository;

        public ClubsService(
            IClubsRepository clubsRepository, 
            IInvitationsRepository invitationsRepository,
            IMembersRepository membersRepository)
        {
            _clubsRepository = clubsRepository;
            _invitationsRepository = invitationsRepository;
            _membersRepository = membersRepository;
        }

        public Task CreateClub(Club club)
        {
            return _clubsRepository.CreateClub(club);
        }

        public IEnumerable<Club> GetAllClubs()
        {
            return _clubsRepository.GetClubs();
        }

        public Task<Club> GetClubById(int clubId)
        {
            return _clubsRepository.GetClubById(clubId);
        }

        public Club GetMemberClub(string email)
        {
            return _clubsRepository
                .GetClubs(c => c.Members.Any(m => m.IdentityUser.Email == email))?
                .FirstOrDefault();
        }

        public Task InvitePlayer(Club club, Member player, CreateInvitation createInvitation)
        {
            var invitation = new Invitation
            {
                InvitedPlayer = player,
                InvitingClub = club,
                Description = createInvitation.Description,
                Status = InvitationStatus.NotReplied
            };

            return _invitationsRepository.AddInvitationAsync(invitation);
        }

        public Task SaveChangesAsync()
        {
            return _clubsRepository.SaveChangesAsync();
        }
    }
}
