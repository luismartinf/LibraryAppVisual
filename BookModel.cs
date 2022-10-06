using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LibraryAppAccess.AppData;
using LibraryAppAccess.Controllers;

namespace LibraryAppAccess.LibraryModels
{
    public class BookModel
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        [Column(TypeName = "bit")]
        public bool Onlend { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int YearPub { get; set; }

        [Column(TypeName = "DateTime")]
        public DateTime DateLend { get; set; }


        [Column(TypeName = "int")]
        public int LendeeId { get; set; }

        [ForeignKey("LendeeId")]
        public virtual LendeeModel OnLendees { get; set; }


        public virtual bool BookModelExists(int id)
        {
            ILibraryAppContext _context = new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
            return _context.Books.Any(e => e.BookId == id);
        }
    }
}
