using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Reports
{
    public class DeleteModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DeleteModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Report Report { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Report = await _context.Reports.SingleOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
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

            Report = await _context.Reports.Include(r => r.QuestionReply).SingleAsync(r=>r.ReportId == id.Value);

            if (Report != null)
            {
                if (Report.QuestionReply != null)
                {
                    _context.QuestionReplies.RemoveRange(Report.QuestionReply);
                }
                _context.Reports.Remove(Report);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
