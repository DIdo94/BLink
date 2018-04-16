using BLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLink.Models.RequestModels.Members;

namespace BLink.Core.Services
{
    public interface IClubsService
    {
        IEnumerable<Club> GetAllClubs();

        Task CreateClub(Club club);

        Task SaveChangesAsync();

        Club GetMemberClub(string email);
    }
}
