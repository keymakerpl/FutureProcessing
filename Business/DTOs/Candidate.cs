using System.Collections.Generic;

namespace Business.DTOs
{
    public class Candidate
    {
        public string name { get; set; }
        public string party { get; set; }
    }

    public class Candidates
    {
        public string publicationDate { get; set; }
        public List<Candidate> candidate { get; set; }
    }

    public class CandidatesRoot
    {
        public Candidates candidates { get; set; }
    }

}
