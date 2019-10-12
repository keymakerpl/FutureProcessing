using Business.DTOs;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace WebDataAccess
{
    public class WebContext : IWebContext
    {
        public IEnumerable<Candidate> GetCandidates()
        {
            var url = "http://webtask.future-processing.com:8069/candidates";
            var serializer = new XmlSerializer(typeof(Candidates));
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead(url))
                {
                    var reader = new StreamReader(stream);
                    var jsonString = reader.ReadToEnd();

                    var root = new JavaScriptSerializer().Deserialize<CandidatesRoot>(jsonString);
                    foreach (var candidate in root.candidates.candidate)
                    {
                        yield return candidate;
                    }
                }
            }
        }

        public IEnumerable<Person> GetDisallowedPersons()
        {
            var url = "http://webtask.future-processing.com:8069/blocked";
            var serializer = new XmlSerializer(typeof(Person));
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead(url))
                {
                    var reader = new StreamReader(stream);
                    var jsonString = reader.ReadToEnd();

                    var root = new JavaScriptSerializer().Deserialize<DisallowedRoot>(jsonString);
                    foreach (var person in root.disallowed.person)
                    {
                        yield return person;
                    }
                }
            }
        }
    }
}
