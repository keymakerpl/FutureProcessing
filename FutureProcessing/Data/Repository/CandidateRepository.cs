using Business.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MSSQLDataAccess;

namespace FutureProcessing.Data.Repository
{
    public class CandidateRepository : GenericRepository<Candidate, FPDbContext>, ICandidateRepository
    {
        public CandidateRepository(FPDbContext context) : base(context)
        {
        }
    }
}
