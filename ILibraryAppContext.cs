using System;
using LibraryAppAccess.LibraryModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace LibraryAppAccess.AppData
{
    public interface ILibraryAppContext
    {
        DbSet<LendeeModel> Lendees { get; set; }
        DbSet<BookModel> Books { get; set; }
        int SaveChanges();

        void MarkAsModified(LendeeModel item);
        void MarkAsModified(BookModel item);


    }
}
