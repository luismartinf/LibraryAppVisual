using LibraryAppAccess.LibraryModels;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LibraryAppAccess.AppData
{
    public class LibraryAppContext : DbContext, ILibraryAppContext
    {
        public LibraryAppContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<LendeeModel> Lendees { get; set; }
        public DbSet<BookModel> Books { get; set; }

        public void MarkAsModified(LendeeModel item)
        {
            Entry(item).State = EntityState.Modified;
        }

        public void MarkAsModified(BookModel item)
        {
            Entry(item).State = EntityState.Modified;
        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LendeeModel>().Property(p => p.LendeeId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookModel>().Property(p => p.BookId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            base.OnModelCreating(modelBuilder);
        }
    }

    public class LibraryContextFactory : IDbContextFactory<LibraryAppContext>
    {
        public LibraryContextFactory()
        {
        }

        public LibraryAppContext Create()
        {
            return new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
        }

        
    }
}
