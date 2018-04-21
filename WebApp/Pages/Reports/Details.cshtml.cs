using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Reports
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DetailsModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public Report Report { get; set; }
        public Assessor Assessor { get; set; }
        public AgedCareCenter AgedCareCenter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportId == id);
            AgedCareCenter = await _context.AgedCareCenters.Where(a => a.AgedCareCenterId == Report.AgedCareCenterId).SingleOrDefaultAsync();
            Assessor = await _context.Assessors.Where(a => a.AssessorId == Report.AssessorId).SingleOrDefaultAsync();

            if (Report == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
