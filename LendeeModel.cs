using LibraryAppAccess.AppData;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace LibraryAppAccess.LibraryModels
{
    public class LendeeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LendeeId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "bigint")]
        public long ContactNum { get; set; }

        public virtual bool LendeeModelExists(int id)
        {
            ILibraryAppContext _context = new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
            return _context.Lendees.Any(e => e.LendeeId == id);
        }
       
    }

}
