using Business.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using MSSQLDataAccess;

namespace FutureProcessing.Data.Repository
{
    public class PersonRepository : GenericRepository<Person, FPDbContext>, IPersonRepository
    {
        public PersonRepository(FPDbContext context) : base(context)
        {
        }
    }
}
