using BLink.Core.Repositories;
using BLink.Core.Services;
using System;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class InvitationsService : IInvitationsService
    {
        private readonly IInvitationsRepository _invitationsRepository;
        public InvitationsService(IInvitationsRepository invitationsRepository)
        {
            _invitationsRepository = invitationsRepository;
        }

        public Task InvitePlayerAsync()
        {
            throw new NotImplementedException();
        }
    }
}
