using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLink.Core.Repositories;
using BLink.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BLink.Models.Enums;

namespace BLink.Data.Repositories
{
    public class InvitationsRepository : IInvitationsRepository
    {
        private readonly BlinkDbContext _dbContext;
        public InvitationsRepository(BlinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void RespondInvitation(Invitation invitation, InvitationStatus invitationStatus)
        {
            if (invitation != null)
            {
                invitation.Status = invitationStatus;
                _dbContext.Invitations.Update(invitation);
            }
        }

        public Task AddInvitationAsync(Invitation invitation)
        {
            return _dbContext.Invitations.AddAsync(invitation);
        }

        public IEnumerable<Invitation> GetInvitations(Expression<Func<Invitation, bool>> predicate)
        {
            return _dbContext
                .Invitations
                .Include(i => i.InvitingClub)
                .Include(i => i.InvitedPlayer)
                .Include(i => i.InvitedPlayer.IdentityUser)
                .Where(predicate);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public Task<Invitation> GetInvitationByIdAsync(int invitationId, string includeProperties = null)
        {

            return string.IsNullOrWhiteSpace(includeProperties) ?
                _dbContext.Invitations.FindAsync(invitationId) :
                _dbContext.Invitations.Include(includeProperties)
                .FirstOrDefaultAsync(i => i.Id == invitationId);
        }
    }
}
