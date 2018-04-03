using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.AccreditationStandarts
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DetailsModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

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
    }
}
