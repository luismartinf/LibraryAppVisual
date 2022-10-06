using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

using LibraryAppAccess.AppData;
using LibraryAppAccess.LibraryModels;

using System.Data.Entity.Infrastructure;
namespace LibraryAppAccess.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class BookModelController : ControllerBase
        {
            private ILibraryAppContext _context = new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
          

            public BookModelController(ILibraryAppContext contextI = null)
            {
                if (contextI != null)
                {
                    _context = contextI;
                }

            }

            // GET: api/BookModel
            [HttpGet]
            public virtual ActionResult<IEnumerable<BookModel>> GetBooks()
            {

                return _context.Books.ToList();
            }

            // GET: api/BookModel/5
            [HttpGet("{id}")]
            public virtual ActionResult<BookModel> GetBookModel(int id)
            {
                var BookModel = _context.Books.Find(id);

                if (BookModel == null)
                {
                    return NotFound();
                }

                return BookModel;
            }

            // PUT: api/BookModel/5
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
            [HttpPut("{id}")]
            public virtual IActionResult PutBookModel(int id, BookModel BookModel)
            {
                if (id != BookModel.BookId)
                {
                    return BadRequest();
                }

                //_context.Entry(BookModel).State = EntityState.Modified;
                _context.MarkAsModified(BookModel);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookModel.BookModelExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            // POST: api/BookModel
            // To protect from overposting attacks, enable the specific properties you want to bind to, for
            // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
            [HttpPost]
            public virtual ActionResult<BookModel> PostBookModel(BookModel BookModel)
            {
                _context.Books.Add(BookModel);
                _context.SaveChanges();

                return CreatedAtAction("GetBookModel", new { id = BookModel.BookId }, BookModel);
            }

            // DELETE: api/BookModel/5
            [HttpDelete("{id}")]
            public virtual ActionResult<BookModel> DeleteBookModel(int id)
            {
                var BookModel = _context.Books.Find(id);
                if (BookModel == null)
                {
                    return NotFound();
                }

                _context.Books.Remove(BookModel);
                _context.SaveChanges();

                return BookModel;
            }

        

      }
    
}
