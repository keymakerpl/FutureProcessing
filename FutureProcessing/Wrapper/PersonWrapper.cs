using Business.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Wrapper;

namespace FutureProcessing.Wrapper
{
    public class PersonWrapper : ModelWrapper<Person>, IPersonWrapper
    {
        public PersonWrapper(Person model) : base(model)
        {
        }

        private string _firstName;
        public string FirstName
        {
            get { return GetValue<string>(); }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;
        public string LastName
        {
            get { return GetValue<string>(); }
            set { SetProperty(ref _lastName, value); }
        }
    }
}
