using System;

namespace Infrastructure.Events
{
    public class UserAuthorizationEventArgs
    {
        public Guid ID { get; set; }

        public string Pesel { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}
