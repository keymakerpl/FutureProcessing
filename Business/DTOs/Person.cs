using System.Collections.Generic;

namespace Business.DTOs
{
    public class Person
    {
        public string pesel { get; set; }
    }

    public class Disallowed
    {
        public string publicationDate { get; set; }
        public List<Person> person { get; set; }
    }

    public class DisallowedRoot
    {
        public Disallowed disallowed { get; set; }
    }
}
