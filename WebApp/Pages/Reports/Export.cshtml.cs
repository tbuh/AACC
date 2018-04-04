using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Reports
{
    public class ExportModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public ExportModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Report Report { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}