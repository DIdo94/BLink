using BLink.Core.Repositories;
using BLink.Models;
using BLink.Models.Enums;
using BLink.Models.RequestModels.Members;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLink.Data.Repositories
{
    public class MembersRepository : IMembersRepository
    {
        private readonly BlinkDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public MembersRepository(BlinkDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Task AddMemberAsync(Member member)
        {
            return _dbContext.Members.AddAsync(member);
        }

        public void EditMember(Member member)
        {
            _dbContext.Members.Update(member);
        }

        public Task<Member> GetMemberByEmail(string email)
        {
            var membes = _dbContext
                .Members;
            return Task.FromResult(_dbContext
                .Members
                .Include(m => m.Club)
                .Include(m => m.IdentityUser)
                .ThenInclude(m => m.Roles)
                .Include(m => m.MemberPositions)
                .FirstOrDefault(m => m.IdentityUser.Email == email));
        }

        public Position GetPositionById(int positionId)
        {
            return _dbContext
                 .MemberPositions
                 .Include(mp => mp.Position)
                 .FirstOrDefault(mp => mp.PostitionId == positionId)
                 ?.Position;
        }

        public IEnumerable<Member> GetMembers()
        {
            return _dbContext.Members;
        }

        public Task<Member> GetPlayerById(int playerId)
        {
            IdentityRole playerRole = _dbContext
               .Roles
               .FirstOrDefault(r => r.Name == Role.Player.ToString());
            return _dbContext
                .Members
                .Include(m => m.IdentityUser)
                .ThenInclude(u => u.Roles)
                .FirstOrDefaultAsync(m => 
                    m.IdentityUser.Roles.Any(r => r.RoleId == playerRole.Id) && 
                    m.Id == playerId);
        }

        public async Task<IEnumerable<PlayerFilterResult>> GetPlayersByCriteria(PlayerFilterCriteria filterCriteria)
        {
            IdentityRole playerRole = await GetMemberRole(r => r.Name == Role.Player.ToString());
            IQueryable<Member> players = _dbContext
                .Members
                .Include(m => m.IdentityUser)
                .ThenInclude(u => u.Roles)
                .Include(m => m.MemberPositions);
            players = players.Where(m => m.IdentityUser.Roles.Any(r => r.RoleId == playerRole.Id));
            if (filterCriteria.ClubId.HasValue)
            {
                players = players.Include(p => p.Club).Where(p => p.Club.Id == filterCriteria.ClubId.Value);
            }
            else
            {
                players = players.Include(p => p.Club).Where(p => p.Club == null);
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.Name))
            {
                players = players.Where(p =>
                    p.FirstName.StartsWith(filterCriteria.Name, StringComparison.OrdinalIgnoreCase) ||
                    p.LastName.StartsWith(filterCriteria.Name, StringComparison.OrdinalIgnoreCase));
            }

            players = players.Where(p =>
                p.Height >= filterCriteria.MinHeight &&
                p.Height <= filterCriteria.MaxHeight &&
                p.Weight >= filterCriteria.MinWeight &&
                p.Weight <= filterCriteria.MaxWeight);

            if (filterCriteria.Position != 0)
            {
                players = players.Where(p => p.MemberPositions.Any(mp => mp.Position.Name == filterCriteria.Position.ToString()));
            }

            return players.Select(p => new PlayerFilterResult
            {
                Id = p.Id,
                ClubId = filterCriteria.ClubId,
                FirstName = p.FirstName,
                Height = p.Height,
                LastName = p.LastName,
                Weight = p.Weight,
                PositionId = p.MemberPositions.First().PostitionId
            });
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public Position GetPositionByName(string name)
        {
            return _dbContext
                .MemberPositions
                .Include(mp => mp.Position)
                .FirstOrDefault(mp => mp.Position.Name == name)
                ?.Position;
        }

        public Task<IdentityRole> GetMemberRole(Expression<Func<IdentityRole, bool>> predicate)
        {
            return _dbContext.Roles.FirstOrDefaultAsync(predicate);
        }
    }
}
