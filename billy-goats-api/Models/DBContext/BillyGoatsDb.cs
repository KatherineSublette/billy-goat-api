using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BillyGoats.Api.Models.DBContext
{
    public partial class BillyGoatsDb : DbContext
    {
        private IConfiguration configuration;

        public BillyGoatsDb(DbContextOptions<BillyGoatsDb> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Guest> Guests { get; set; }

        public DbSet<Guide> Guides { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // convert datatable name and column name to snake case
            modelBuilder.NamePascalToSnake();

            // set default value or default sql functions if property is tagged with DefaultValue or DefaultSqlValue
            // modelBuilder.SetDefault();
        }
    }
}
