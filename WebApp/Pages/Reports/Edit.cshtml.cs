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
        public Assessor Assessor { get; set; }
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

            Assessor = _context.Assessors.FirstOrDefault(a => a.AssessorId == Report.AssessorId);
            var questions = _context.Questions.Include(q => q.Questions).Where(q => !q.ParentId.HasValue);
            AgedCareCenters = _context.AgedCareCenters.Select(a => new SelectListItem { Text = a.Name, Value = a.AgedCareCenterId.ToString() });
            AccreditationStandarts = _context.AccreditationStandarts.Include(acs => acs.Questions).ThenInclude(q => q.Questions).OrderBy(acs => acs.AccreditationStandartId).ToList();

            QuestionReplyList =
            (from q in questions
             from q2 in q.Questions
             join r in Report.QuestionReply on q2.QuestionId equals r.QuestionId into gj
             from subr in gj.DefaultIfEmpty()
             select new ReplyVM(q2, subr, Report)

             ).Union
            (from q in questions
             join r in Report.QuestionReply on q.QuestionId equals r.QuestionId into gj
             from subr in gj.DefaultIfEmpty()
             where q.Questions.Count == 0
             select new ReplyVM(q, subr, Report)).ToDictionary(rvm => rvm.Question.QuestionId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var files = HttpContext.Request.Form.Files;
            var qrList = QuestionReplyList.Values.ToList();

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i] == null || files[i].Length == 0)
                {
                    continue;
                }
                using (var memoryStream = new MemoryStream())
                {
                    await files[i].CopyToAsync(memoryStream);

                    qrList[i].Reply.ReportImage = memoryStream.ToArray();
                }
            }
            try
            {
                //Report.ReportDate = DateTime.Now;
                Report.QuestionReply = QuestionReplyList.Values.Select(rvm => rvm.Reply).ToList();
                await _context.UpdateReport(Report);
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
            Question = q; Reply = qr ?? new QuestionReply { Question = q, QuestionId = q.QuestionId, ReportId = r.ReportId };
        }
        public Question Question { get; set; }
        public QuestionReply Reply { get; set; }
    }

}
