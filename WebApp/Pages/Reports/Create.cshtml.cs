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
        }

        public List<IGrouping<int, QuestionReplyVM>> Questions { get; set; }
        public List<Report> Reports { get; set; }

        public IActionResult OnGet()
        {
            Questions = _context.Questions.Select(q=>new QuestionReplyVM(q)).OrderBy(q => q.Question.AccreditationStandartId).GroupBy(q => q.Question.AccreditationStandartId).ToList();
            return Page();
        }

        [BindProperty]
        public List<QuestionReply> QuestionReplyList { get; set; }
        [BindProperty]
        public Report Report { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            _context.Reports.Add(Report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }

    public class QuestionReplyVM
    {
        public QuestionReplyVM(Question q)
        {
            //Question = q;
            //QuestionReply = new QuestionReply { Question = q, QuestionId = q.QuestionId };
        }
        public QuestionReply QuestionReply { get; set; }
        public Question Question { get; set; }
    }
}