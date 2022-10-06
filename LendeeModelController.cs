using LibraryAppAccess.AppData;
using LibraryAppAccess.LibraryModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace LibraryAppAccess.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendeeModelController : Controller
    {
        private ILibraryAppContext _context = new LibraryAppContext("Server=ASPLAPLTM024;Database=LibraryappRMVCDB;Trusted_Connection=True;MultipleActiveResultSets=True;");
        

        public LendeeModelController(ILibraryAppContext contextI = null)
        {
            if (contextI != null)
            {
                _context = contextI;
            }

        }


        // GET: api/LendeeModel
        [HttpGet]
        public virtual ActionResult<IEnumerable<LendeeModel>> GetLendees()
        {
            return _context.Lendees.ToList();
        }

        // GET: api/LendeeModel/5
        [HttpGet("{id}")]
        public virtual ActionResult<LendeeModel> GetLendeeModel(int id)
        {
            var LendeeModel = _context.Lendees.Find(id);

            if (LendeeModel == null)
            {
                return NotFound();
            }

            return LendeeModel;
        }

        // PUT: api/LendeeModel/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public virtual IActionResult PutLendeeModel(int id, LendeeModel LendeeModel)
        {
            if (id != LendeeModel.LendeeId)
            {
                return BadRequest();
            }

            //_context.Entry(LendeeModel).State = EntityState.Modified;
            _context.MarkAsModified(LendeeModel);

            try
            {
                _context.SaveChanges();

            }
            catch (DbUpdateConcurrencyException)
            {
                
                if (!LendeeModel.LendeeModelExists(id))
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

        // POST: api/LendeeModel
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public virtual ActionResult<LendeeModel> PostLendeeModel(LendeeModel LendeeModel)
        {

            _context.Lendees.Add(LendeeModel);
            _context.SaveChanges();

            return CreatedAtAction("GetLendeeModel", new { id = LendeeModel.LendeeId }, LendeeModel);
        }

        // DELETE: api/LendeeModel/5
        [HttpDelete("{id}")]
        public virtual ActionResult<LendeeModel> DeleteLendeeModel(int id)
        {
            var LendeeModel = _context.Lendees.Find(id);
            if (LendeeModel == null)
            {
                return NotFound();
            }

            _context.Lendees.Remove(LendeeModel);
            _context.SaveChanges();

            return LendeeModel;
        }

    }
}
