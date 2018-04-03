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
    [Route("api/AccreditationStandarts")]
    public class AccreditationStandartsController : Controller
    {
        private readonly AACCContext _context;

        public AccreditationStandartsController(AACCContext context)
        {
            _context = context;
        }

        // GET: api/AccreditationStandarts
        [HttpGet]
        public IEnumerable<AccreditationStandart> GetAccreditationStandarts()
        {
            return _context.AccreditationStandarts;
        }

        // GET: api/AccreditationStandarts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccreditationStandart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accreditationStandart = await _context.AccreditationStandarts.SingleOrDefaultAsync(m => m.AccreditationStandartId == id);

            if (accreditationStandart == null)
            {
                return NotFound();
            }

            return Ok(accreditationStandart);
        }

        // PUT: api/AccreditationStandarts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccreditationStandart([FromRoute] int id, [FromBody] AccreditationStandart accreditationStandart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accreditationStandart.AccreditationStandartId)
            {
                return BadRequest();
            }

            _context.Entry(accreditationStandart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccreditationStandartExists(id))
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

        // POST: api/AccreditationStandarts
        [HttpPost]
        public async Task<IActionResult> PostAccreditationStandart([FromBody] AccreditationStandart accreditationStandart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccreditationStandarts.Add(accreditationStandart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccreditationStandart", new { id = accreditationStandart.AccreditationStandartId }, accreditationStandart);
        }

        // DELETE: api/AccreditationStandarts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccreditationStandart([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accreditationStandart = await _context.AccreditationStandarts.SingleOrDefaultAsync(m => m.AccreditationStandartId == id);
            if (accreditationStandart == null)
            {
                return NotFound();
            }

            _context.AccreditationStandarts.Remove(accreditationStandart);
            await _context.SaveChangesAsync();

            return Ok(accreditationStandart);
        }

        private bool AccreditationStandartExists(int id)
        {
            return _context.AccreditationStandarts.Any(e => e.AccreditationStandartId == id);
        }
    }
}