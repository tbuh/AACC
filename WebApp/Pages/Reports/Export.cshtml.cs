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

        private Dictionary<int, QuestionReply> QuestionReplyList;
        public Report Report { get; set; }
        public List<AccreditationStandart> AccreditationStandartList { get; set; }
        public AgedCareCenter AgedCareCenter { get; set; }
        public Assessor Assessor { get; set; }

        public string GetAnswer(Question q)
        {
            if (Report.QuestionReply == null) return "";
            if (!q.ParentId.HasValue)
            {
                var reply = Report.QuestionReply.FirstOrDefault(r => r.QuestionId == q.QuestionId);
                if (reply != null)
                    return reply.Response ? "Met" : "";
                else
                {
                    int cnt = 0;
                    foreach (var subQ in q.Questions)
                    {
                        reply = Report.QuestionReply.FirstOrDefault(r => r.QuestionId == subQ.QuestionId);
                        if (reply.Response) cnt++;
                    }
                    return q.Questions.Count == cnt ? "Met" : "";
                }
            }
            else
            {
                var reply = Report.QuestionReply.FirstOrDefault(r => r.QuestionId == q.QuestionId);
                return reply != null && reply.Response ? "Met" : "";
            }
        }

        public string GetNotes(Question q)
        {
            if (Report.QuestionReply == null) return "";
            var reply = Report.QuestionReply.FirstOrDefault(r => r.QuestionId == q.QuestionId);
            return reply != null ? reply.Notes : "";
        }

        public QuestionReply GetReply(Question q)
        {
            if (Report.QuestionReply == null) return null;
            return Report.QuestionReply.FirstOrDefault(r => r.QuestionId == q.QuestionId);
        }

        public async Task<IActionResult> OnGet(int id)
        {
            Report = await _context.Reports.Include(r => r.QuestionReply).SingleOrDefaultAsync(m => m.ReportId == id);

            if (Report == null)
            {
                return NotFound();
            }

            //if (Report.QuestionReply != null)
            //{
            //    QuestionReplyList = Report.QuestionReply.ToDictionary(r=>)
            //}

            AgedCareCenter = await _context.AgedCareCenters.SingleAsync(a => a.AgedCareCenterId == Report.AgedCareCenterId);
            Assessor = await _context.Assessors.SingleAsync(a => a.AssessorId == Report.AssessorId);
            AccreditationStandartList = await _context.AccreditationStandarts.Include(acs => acs.Questions).ThenInclude(q => q.Questions).OrderBy(acs => acs.StandartType).ToListAsync();

            return Page();
        }
    }
}