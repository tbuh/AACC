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
    public class CreateModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public CreateModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGet()
        {
            TeamIds = await _context.Teams.Select(t => new SelectListItem { Text = t.Name, Value = t.TeamId.ToString() }).ToListAsync();
            return Page();
        }

        [BindProperty]
        public Assessor Assessor { get; set; }
        public IEnumerable<SelectListItem> TeamIds { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Assessors.Add(Assessor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}