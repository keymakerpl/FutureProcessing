using Business.Entities;
using Infrastructure.Repositories;

namespace Infrastructure.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {        
    }
}
