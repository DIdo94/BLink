using BLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BLink.Core.Repositories
{
    public interface IMembersRepository
    {
        IEnumerable<Member> GetMembers();

        Task AddMemberAsync(Member member);

        Task<Member> GetMemberByEmail(string email);

        Task SaveChangesAsync();

        Task<IEnumerable<PlayerFilterResult>> GetPlayersByCriteria(PlayerFilterCriteria filterCriteria);

        Task<Member> GetPlayerById(int playerId);

        void EditMember(Member member);

        Position GetPositionById(int positionId);

        Position GetPositionByName(string name);

        Task<IdentityRole> GetMemberRole(Expression<Func<IdentityRole, bool>> predicate);
    }
}
