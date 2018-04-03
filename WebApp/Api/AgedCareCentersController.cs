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
    [Route("api/AgedCareCenters")]
    public class AgedCareCentersController : Controller
    {
        private readonly AACCContext _context;

        public AgedCareCentersController(AACCContext context)
        {
            _context = context;
        }

        // GET: api/AgedCareCenters
        [HttpGet]
        public IEnumerable<AgedCareCenter> GetAgedCareCenters()
        {
            return _context.AgedCareCenters;
        }

        // GET: api/AgedCareCenters/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAgedCareCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agedCareCenter = await _context.AgedCareCenters.SingleOrDefaultAsync(m => m.AgedCareCenterId == id);

            if (agedCareCenter == null)
            {
                return NotFound();
            }

            return Ok(agedCareCenter);
        }

        // PUT: api/AgedCareCenters/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgedCareCenter([FromRoute] int id, [FromBody] AgedCareCenter agedCareCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != agedCareCenter.AgedCareCenterId)
            {
                return BadRequest();
            }

            _context.Entry(agedCareCenter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgedCareCenterExists(id))
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

        // POST: api/AgedCareCenters
        [HttpPost]
        public async Task<IActionResult> PostAgedCareCenter([FromBody] AgedCareCenter agedCareCenter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AgedCareCenters.Add(agedCareCenter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgedCareCenter", new { id = agedCareCenter.AgedCareCenterId }, agedCareCenter);
        }

        // DELETE: api/AgedCareCenters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgedCareCenter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var agedCareCenter = await _context.AgedCareCenters.SingleOrDefaultAsync(m => m.AgedCareCenterId == id);
            if (agedCareCenter == null)
            {
                return NotFound();
            }

            _context.AgedCareCenters.Remove(agedCareCenter);
            await _context.SaveChangesAsync();

            return Ok(agedCareCenter);
        }

        private bool AgedCareCenterExists(int id)
        {
            return _context.AgedCareCenters.Any(e => e.AgedCareCenterId == id);
        }
    }
}