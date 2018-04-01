using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Api
{
    [Produces("application/json")]
    [Route("api/Assessors")]
    public class AssessorsController : Controller
    {
        private readonly AACCContext _context;

        public AssessorsController(AACCContext context)
        {
            _context = context;
            //Db Initializer
           // _context.Database.EnsureCreated();
        }

        // GET: api/Assessors
        [HttpGet]
        public IEnumerable<Assessor> GetAssessor()
        {
            return _context.Assessors.ToList();
        }

        // GET: api/Assessors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssessor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assessor = await _context.Assessors.SingleOrDefaultAsync(m => m.AssessorId == id);

            if (assessor == null)
            {
                return NotFound();
            }

            return Ok(assessor);
        }

        // PUT: api/Assessors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessor([FromRoute] int id, [FromBody] Assessor assessor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != assessor.AssessorId)
            {
                return BadRequest();
            }

            _context.Entry(assessor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessorExists(id))
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

        // POST: api/Assessors
        [HttpPost]
        public async Task<IActionResult> PostAssessor([FromBody] Assessor assessor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Assessors.Add(assessor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssessor", new { id = assessor.AssessorId }, assessor);
        }

        // DELETE: api/Assessors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assessor = await _context.Assessors.SingleOrDefaultAsync(m => m.AssessorId == id);
            if (assessor == null)
            {
                return NotFound();
            }

            _context.Assessors.Remove(assessor);
            await _context.SaveChangesAsync();

            return Ok(assessor);
        }

        private bool AssessorExists(int id)
        {
            return _context.Assessors.Any(e => e.AssessorId == id);
        }
    }
}