using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MSSQLDataAccess;
using System.Data.Entity;

namespace FutureProcessing.Data.Repository
{
    public class VoteRepository : GenericRepository<Vote, FPDbContext>, IVoteRepository
    {
        public VoteRepository(FPDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Vote>> GetAllAsync()
        {
            return await Context.Set<Vote>().Include(v => v.VotedCandidates).ToListAsync();
        }
    }
}
