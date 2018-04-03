using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages
{
    public class AccreditationStandartEditModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public AccreditationStandartEditModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccreditationStandart AccreditationStandart { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccreditationStandart = await _context.AccreditationStandarts.SingleOrDefaultAsync(m => m.AccreditationStandartId == id);

            if (AccreditationStandart == null)
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

            _context.Attach(AccreditationStandart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccreditationStandartExists(AccreditationStandart.AccreditationStandartId))
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

        private bool AccreditationStandartExists(int id)
        {
            return _context.AccreditationStandarts.Any(e => e.AccreditationStandartId == id);
        }
    }
}
