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
    public class AccreditationStandartDetailsModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public AccreditationStandartDetailsModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public List<AccreditationStandart> AccreditationStandarts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            AccreditationStandarts = await _context.AccreditationStandarts.ToListAsync();

            if (AccreditationStandarts == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
