using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;

namespace WebApp.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly AACCContext _context;

        public CreateModel(AACCContext context)
        {
            _context = context;
            QuestionReplyList = new List<QuestionReply>();
        }

        [BindProperty]
        public List<IGrouping<int, QuestionReplyVM>> Questions { get; set; }
        public List<Report> Reports { get; set; }
        public List<AccreditationStandart> AccreditationStandarts { get; set; }
        public IEnumerable<SelectListItem> AgedCareCenters { get; set; }
        public IEnumerable<SelectListItem> Assessors { get; set; }

        public IActionResult OnGet()
        {
            InitModel();
            return Page();
        }

        private void InitModel()
        {
            Assessors = _context.Assessors.Select(a => new SelectListItem { Text = a.Name, Value = a.AssessorId.ToString() });
            AgedCareCenters = _context.AgedCareCenters.Select(a => new SelectListItem { Text = a.Name, Value = a.AgedCareCenterId.ToString() });
            Questions = _context.Questions.Select(q => new QuestionReplyVM(q)).OrderBy(q => q.Question.AccreditationStandartId).GroupBy(q => q.Question.AccreditationStandartId).ToList();
            AccreditationStandarts = _context.AccreditationStandarts.ToList();
        }

        [BindProperty]
        public List<QuestionReply> QuestionReplyList { get; set; }
        [BindProperty]
        public Report Report { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                InitModel();
                return Page();
            }
            Report.ReportDate = DateTime.Now;
            Report.QuestionReply = QuestionReplyList;

            _context.Reports.Add(Report);
            //foreach (var qr in QuestionReplyList)
            //{

            //}
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class QuestionReplyVM
    {
        public QuestionReplyVM(Question q)
        {
            Question = q;
            QuestionReply = new QuestionReply { QuestionId = q.QuestionId };
        }
        public QuestionReply QuestionReply { get; set; }
        public Question Question { get; set; }
    }
}