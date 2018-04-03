using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.AgedCareCenters
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DeleteModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AgedCareCenter AgedCareCenter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AgedCareCenter = await _context.AgedCareCenters.SingleOrDefaultAsync(m => m.AgedCareCenterId == id);

            if (AgedCareCenter == null)
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

            AgedCareCenter = await _context.AgedCareCenters.FindAsync(id);

            if (AgedCareCenter != null)
            {
                _context.AgedCareCenters.Remove(AgedCareCenter);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
