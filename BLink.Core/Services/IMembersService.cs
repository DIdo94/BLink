using BLink.Models;
using BLink.Models.RequestModels.Members;
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

        IEnumerable<PlayerFilterResult> GetPlayers(PlayerFilterCriteria filterCriteria);
    }
}
