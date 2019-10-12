using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Entities;
using Infrastructure.Helpers;

namespace Infrastructure.Interfaces
{
    public interface IPersonManager
    {
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        bool PersonExists(string peselHash);
        bool PersonIsAllowedToVote(string pesel);
        Task SaveAsync();
        bool IsServiceReady();
        PeselStatuses VerifyPesel(string pesel);
        Task<Person> GetPersonAsync(string pesel, string firstName, string lastName);
        DateTime DateOfBirthFromPesel { get; set; }
    }
}