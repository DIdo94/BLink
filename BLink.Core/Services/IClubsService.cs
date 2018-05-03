using BLink.Models;
using BLink.Models.RequestModels.Clubs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLink.Core.Services
{
    public interface IClubsService
    {
        IEnumerable<Club> GetAllClubs();

        Task<Club> GetClubById(int clubId);

        Task CreateClub(Club club);

        Task SaveChangesAsync();

        Club GetMemberClub(string email);

        Task InvitePlayer(Club club, Member player, CreateInvitation createInvitation);
    }
}
