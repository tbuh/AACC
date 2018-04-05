using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.Reports
{
    public class CreateModel : PageModel
    {
        private readonly AACCContext _context;

        public CreateModel(AACCContext context)
        {
            _context = context;
        }

        public List<AccreditationStandart> AccreditationStandarts { get; set; }
        public IEnumerable<SelectListItem> AgedCareCenters { get; set; }
        public IEnumerable<SelectListItem> Assessors { get; set; }

        public IActionResult OnGet()
        {
            InitModel();
            return Page();
        }

        private void InitModel(bool isNew = true)
        {
            Assessors = _context.Assessors.Select(a => new SelectListItem { Text = a.Name, Value = a.AssessorId.ToString() });
            AgedCareCenters = _context.AgedCareCenters.Select(a => new SelectListItem { Text = a.Name, Value = a.AgedCareCenterId.ToString() });
            AccreditationStandarts = _context.AccreditationStandarts.Include(ass => ass.Questions).ToList();
            if (isNew)
                QuestionReplyList = (
                    from asst in AccreditationStandarts
                    from q in asst.Questions
                    select new QuestionReply
                    {
                        QuestionId = q.QuestionId
                    }).ToDictionary(qr => qr.QuestionId);


        }

        [BindProperty]
        public Dictionary<int, QuestionReply> QuestionReplyList { get; set; }
        [BindProperty]
        public Report Report { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                InitModel(false);
                return Page();
            }
            Report.ReportDate = DateTime.Now;
            Report.QuestionReply = QuestionReplyList.Values;

            _context.Reports.Add(Report);
            await _context.UpdateReport(Report);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}