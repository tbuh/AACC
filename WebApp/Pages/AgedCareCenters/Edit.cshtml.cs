using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.AgedCareCenters
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public EditModel(WebApp.Models.AACCContext context)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var file = HttpContext.Request.Form.Files;
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file[0].CopyToAsync(memoryStream);

                    AgedCareCenter.Logo = memoryStream.ToArray();
                }
            }

            _context.Attach(AgedCareCenter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgedCareCenterExists(AgedCareCenter.AgedCareCenterId))
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

        private bool AgedCareCenterExists(int id)
        {
            return _context.AgedCareCenters.Any(e => e.AgedCareCenterId == id);
        }
    }
}
