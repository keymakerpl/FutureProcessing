using Business.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace MSSQLDataAccess
{
    public class FPDbContext : DbContext
    {
        public FPDbContext() : base("FPDb")
        {
            Database.SetInitializer<FPDbContext>(new CreateDatabaseIfNotExists<FPDbContext>());
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
