﻿using BLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLink.Models.RequestModels.Members;

namespace BLink.Core.Repositories
{
    public interface IMembersRepository
    {
        IEnumerable<Member> GetMembers();

        Task AddMemberAsync(Member member);

        Task<Member> GetMemberByEmail(string email);

        Task SaveChangesAsync();

        IEnumerable<PlayerFilterResult> GetPlayersByCriteria(PlayerFilterCriteria filterCriteria);

        Task<Member> GetPlayerById(int playerId);

        void EditMember(Member member);

        Position GetPositionById(int positionId);

        Position GetPositionByName(string name);
    }
}
