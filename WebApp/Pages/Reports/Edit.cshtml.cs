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
    public class EditModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public EditModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Report Report { get; set; }
        public List<AccreditationStandart> AccreditationStandarts { get; set; }
        public IEnumerable<SelectListItem> AgedCareCenters { get; set; }
        public IEnumerable<SelectListItem> Assessors { get; set; }
        [BindProperty]
        public Dictionary<int, ReplyVM> QuestionReplyList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Report = await _context.Reports.Include(r => r.QuestionReply).SingleOrDefaultAsync(m => m.ReportId == id);
            if (Report == null)
            {
                return NotFound();
            }

            Assessors = _context.Assessors.Select(a => new SelectListItem { Text = a.Name, Value = a.AssessorId.ToString() });
            AgedCareCenters = _context.AgedCareCenters.Select(a => new SelectListItem { Text = a.Name, Value = a.AgedCareCenterId.ToString() });
            AccreditationStandarts = _context.AccreditationStandarts.Include(acs => acs.Questions).OrderBy(acs => acs.AccreditationStandartId).ToList();

            QuestionReplyList =
            (from acs in AccreditationStandarts
             from q in acs.Questions
             join r in Report.QuestionReply on q.QuestionId equals r.QuestionId into gj
             from subr in gj.DefaultIfEmpty()
             select new ReplyVM(q, subr, Report)).ToDictionary(rvm => rvm.Question.QuestionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Report.QuestionReply = QuestionReplyList.Values.Select(rvm => rvm.Reply).ToList();
            await _context.UpdateReport(Report);
            _context.Attach(Report).State = EntityState.Modified;

            foreach (var qr in Report.QuestionReply)
            {            
                if (qr.QuestionReplyId != 0) _context.Attach(qr).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(Report.ReportId))
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

        private bool ReportExists(int id)
        {
            return _context.Reports.Any(e => e.ReportId == id);
        }
    }

    public class ReplyVM
    {
        public ReplyVM()
        {
            Reply = new QuestionReply();
        }
        public ReplyVM(Question q, QuestionReply qr, Report r)
        {
            Question = q; Reply = qr ?? new QuestionReply { QuestionId = q.QuestionId, ReportId = r.ReportId };
        }
        public Question Question { get; set; }
        public QuestionReply Reply { get; set; }
    }

}
