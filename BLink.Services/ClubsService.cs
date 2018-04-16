using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLink.Models.RequestModels.Members;

namespace BLink.Services
{
    public class ClubsService : IClubsService
    {
        private readonly IClubsRepository _clubsRepository;

        public ClubsService(IClubsRepository clubsRepository)
        {
            _clubsRepository = clubsRepository;
        }

        public Task CreateClub(Club club)
        {
            return _clubsRepository.CreateClub(club);
        }

        public IEnumerable<Club> GetAllClubs()
        {
            return _clubsRepository.GetClubs();
        }

        public Club GetMemberClub(string email)
        {
            return _clubsRepository
                .GetClubs(c => c.Members.Any(m => m.IdentityUser.Email == email))?
                .FirstOrDefault();
        }

        public Task SaveChangesAsync()
        {
            return _clubsRepository.SaveChangesAsync();
        }
    }
}
