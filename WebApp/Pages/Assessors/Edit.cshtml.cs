using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Assessors
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public EditModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Assessor Assessor { get; set; }
        public IEnumerable<SelectListItem> TeamIds { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TeamIds = await _context.Teams.Select(t => new SelectListItem { Text = t.Name, Value = t.TeamId.ToString() }).ToListAsync();
            Assessor = await _context.Assessors.SingleOrDefaultAsync(m => m.AssessorId == id);

            if (Assessor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Assessor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessorExists(Assessor.AssessorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AssessorExists(int id)
        {
            return _context.Assessors.Any(e => e.AssessorId == id);
        }
    }
}
