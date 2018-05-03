using BLink.Models;
using BLink.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLink.Core.Repositories
{
    public interface IInvitationsRepository
    {
        Task AddInvitationAsync(Invitation invitation);

        IEnumerable<Invitation> GetInvitations(Expression<Func<Invitation, bool>> predicate);

        Task<Invitation> GetInvitationByIdAsync(int invitationId, string includeProperties = null);

        Task SaveChangesAsync();

        void RespondInvitation(Invitation invitation, InvitationStatus invitationStatus);
    }
}
