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

        Task CreateClub(CreateClubModel createClubModel);

        Task SaveChangesAsync();

        Club GetMemberClub(string email);

        Task InvitePlayer(Club club, Member player, CreateInvitation createInvitation);

        Club GetClubByName(string name);

        Task UpdateClub(Club club, EditClub editClubModel);

        Task<bool> KickPlayer(int clubId, int playerId);
    }
}
