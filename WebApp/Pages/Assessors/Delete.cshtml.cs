using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Assessors
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DeleteModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Assessor Assessor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Assessor = await _context.Assessors.SingleOrDefaultAsync(m => m.AssessorId == id);

            if (Assessor == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Assessor = await _context.Assessors.FindAsync(id);

            if (Assessor != null)
            {
                _context.Assessors.Remove(Assessor);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
