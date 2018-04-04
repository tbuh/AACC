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

        public Report Report { get; set; }
        public List<AccreditationStandart> AccreditationStandartList { get; set; }
        public AgedCareCenter AgedCareCenter { get; set; }
        public Assessor Assessor { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            Report = await _context.Reports.Include(r => r.QuestionReply).SingleOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
            {
                return NotFound();
            }

            AgedCareCenter = await _context.AgedCareCenters.SingleAsync(a => a.AgedCareCenterId == Report.AgedCareCenterId);
            Assessor = await _context.Assessors.SingleAsync(a => a.AssessorId == Report.AssessorId);
            AccreditationStandartList = await _context.AccreditationStandarts.Include(acs => acs.Questions).OrderBy(acs => acs.StandartType).ToListAsync();

            return Page();
        }
    }
}