using System.Collections.Generic;
using Business.DTOs;

namespace WebDataAccess
{
    public interface IWebContext
    {
        IEnumerable<Candidate> GetCandidates();
        IEnumerable<Person> GetDisallowedPersons();
    }
}