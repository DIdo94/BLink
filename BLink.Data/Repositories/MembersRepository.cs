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

        public async Task<Member> GetMemberByEmail(string email)
        {
            var membes = _dbContext
                .Members;
            return await Task.FromResult(_dbContext
                .Members
                .Include(m => m.IdentityUser)
                .FirstOrDefault(m => m.IdentityUser.Email == email));
        }

        public IEnumerable<Member> GetMembers()
        {
            return _dbContext.Members;
        }

        public IEnumerable<PlayerFilterResult> GetPlayersByCriteria(PlayerFilterCriteria filterCriteria)
        {
            IdentityRole playerRole = _dbContext
                .Roles
                .FirstOrDefault(r => r.Name == Role.Player.ToString());
            IQueryable<Member> players = _dbContext
                .Members
                .Include(m => m.IdentityUser)
                .Include(m => m.IdentityUser.Roles);
            players = players.Where(m => m.IdentityUser.Roles.Any(r => r.RoleId == playerRole.Id));

            if (filterCriteria.ClubId.HasValue)
            {
                players = players.Include(p => p.Club).Where(p => p.Club.Id == filterCriteria.ClubId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filterCriteria.FirstName))
            {
                players = players.Where(p =>
                    p.FirstName.StartsWith(filterCriteria.FirstName, StringComparison.OrdinalIgnoreCase));
            }

            return players.Select(p => new PlayerFilterResult
            {
                FirstName = p.FirstName,
                Height = p.Height,
                LastName = p.LastName,
                Weight = p.Weight
            });
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
