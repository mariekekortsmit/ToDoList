using Microsoft.EntityFrameworkCore;

namespace LearningEF
{

    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options)
               : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<Person> Person { get; set; }
        public DbSet<Address> Addresses { get; set; }

    }
}
