using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Business.Entities
{
    public class Vote
    {
        public Vote()
        {
            Initialize();
        }

        private void Initialize()
        {
            VotedCandidates = new Collection<Candidate>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }        

        public bool IsConfirmed { get; set; }

        public ICollection<Candidate> VotedCandidates { get; set; }

        public Guid? PersonId { get; set; }
        public Person Person { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
