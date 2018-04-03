using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.AccreditationStandarts
{
    public class EditModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public EditModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AccreditationStandart AccreditationStandart { get; set; }
        public IEnumerable<SelectListItem> StandartType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AccreditationStandart = await _context.AccreditationStandarts.SingleOrDefaultAsync(m => m.AccreditationStandartId == id);

            StandartType = new List<SelectListItem> {
                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L1).ToString(),
                               Text = "L1"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L2).ToString(),
                               Text = "L2"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L3).ToString(),
                               Text = "L3"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L4).ToString(),
                               Text = "L4"
                           }
            };
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
