using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using BLink.Models.RequestModels.Members;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLink.Services
{
    public class MembersService : IMembersService
    {
        private readonly IMembersRepository _membersRepository;
        public MembersService(IMembersRepository membersRepository)
        {
            _membersRepository = membersRepository;
        }

        public Task AddMemberAsync(Member member)
        {
            return _membersRepository.AddMemberAsync(member);
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _membersRepository.GetMembers();
        }

        public Task<Member> GetMemberByEmail(string email)
        {
            return _membersRepository.GetMemberByEmail(email);
        }

        public async Task<MemberDetails> GetMemberDetailsByEmail(string email)
        {
            Member member = await GetMemberByEmail(email);
            return new MemberDetails
            {
                FirstName = member.FirstName,
                LastName = member.LastName,
                Height = member.Height,
                PhotoPath = member.PhotoPath,
                Weight = member.Weight
            };
        }

        public IEnumerable<PlayerFilterResult> GetPlayers(PlayerFilterCriteria filterCriteria)
        {
            var players = _membersRepository.GetPlayersByCriteria(filterCriteria);
            return players;
        }

        public Task SaveChangesAsync()
        {
            return _membersRepository.SaveChangesAsync();
        }
    }
}
