using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {

        private readonly AACCContext _context;

        public IndexModel(AACCContext context)
        {
            _context = context;
        }

        public IEnumerable<Assessor> Assessors { get; set; }
        public IEnumerable<AgedCareCenter> AgedCareCenters { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public Assessor Assessor { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {


            Assessors = await _context.Assessors.ToListAsync();
            AgedCareCenters = await _context.AgedCareCenters.ToListAsync();
            Teams = await _context.Teams.ToListAsync();

            return Page();
        }

        [HttpDelete]
        public async Task<IActionResult> OnDeleteAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var assessor = await _context.Assessors.SingleOrDefaultAsync(m => m.AssessorId == id);
            if (assessor == null)
            {
                return NotFound();
            }

            _context.Assessors.Remove(assessor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
