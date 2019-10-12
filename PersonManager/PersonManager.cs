using Business.Entities;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebDataAccess;

namespace PersonManager
{
    public class PersonManager : IPersonManager
    {
        private readonly IPersonRepository _personRepository;
        private readonly IWebContext _webContext;
        private readonly IStringHasher _stringHasher;
        private readonly IConfig _config;
        private readonly IPeselValidator _peselValidator;
        private readonly IVoteRepository _voteRepository;

        private DateTime _dateOfBirthFromPesel;                       

        public PersonManager(IPersonRepository personRepository, IWebContext webContext, IStringHasher stringHasher, IConfig config,
            IVoteRepository voteRepository , IPeselValidator peselValidator)
        {
            this._personRepository = personRepository;
            this._webContext = webContext;
            this._stringHasher = stringHasher;
            this._config = config;
            this._peselValidator = peselValidator;
            this._voteRepository = voteRepository;
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _personRepository.GetAllAsync();
        }

        public bool PersonExists(string peselHash)
        {
            return false;
        }

        public bool PersonIsAllowedToVote(string pesel)
        {
            var disallowed = _webContext.GetDisallowedPersons();
            if (!disallowed.Any(p => p.pesel == pesel)) return true;

            return false;
        }

        public async Task SaveAsync()
        {
            await _personRepository.SaveAsync();
        }

        public bool IsServiceReady()
        {
            return _personRepository.ServerHeartBeat();
        }

        public DateTime DateOfBirthFromPesel
        {
            get { return _dateOfBirthFromPesel; }
            set { _dateOfBirthFromPesel = value; }
        }

        public PeselStatuses VerifyPesel(string pesel)
        {
            _peselValidator.ChangePesel(pesel);
            var result = _peselValidator.Verify();

            try
            {
                DateOfBirthFromPesel = _peselValidator.GetBirthDate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); //TODO: Logger
            }

            return result;
        }

        /// <summary>
        /// If not found return new person entity
        /// </summary>
        /// <param name="pesel"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<Person> GetPersonAsync(string pesel, string firstName, string lastName)
        {
            var persons = await GetAllPersonsAsync();

            var person = persons.SingleOrDefault(p => _stringHasher.VerifyString(pesel, p.PeselHash, p.Salt));
            if (person != null)
                return person;

            var newPerson = GetNewPerson(pesel, firstName, lastName);
            return newPerson;
        }

        private Person GetNewPerson(string pesel, string firstName, string lastName)
        {            
            string peselHash;
            string salt;
            _stringHasher.GenerateSaltedHash(pesel, out peselHash, out salt);

            var person = new Person();
            person.PeselHash = peselHash;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.Salt = salt;

            _personRepository.Add(person);
            _personRepository.SaveAsync();
            
            return person;
        }
    }
}
