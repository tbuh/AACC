using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Api
{
    [Produces("application/json")]
    [Route("api/MobileSync")]
    public class MobileSyncController : Controller
    {
        private readonly AACCContext _context;

        public MobileSyncController(AACCContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Sync([FromBody] List<Report> reports)
        {
            foreach (var report in reports)
            {
                if (report.AgedCareCenterId == -1 || report.AssessorId == -1) continue;

                if (report.IsDeleted)
                {
                    if (report.QuestionReply != null)
                        foreach (var reply in report.QuestionReply)
                        {
                            if (reply.QuestionReplyId != 0)
                                _context.Remove(reply);
                        }
                    _context.Remove(report);
                }
                else if (report.IsChanged)
                {
                    await _context.UpdateReport(report);
                }
                else
                {
                    await _context.SaveReport(report);
                }
                await _context.SaveChangesAsync();
            }

            var model = new SyncModel();
            try
            {
                model.AgedCareCenterList = await _context.AgedCareCenters.ToListAsync();
                model.AssessorList = await _context.Assessors.ToListAsync();
                var Questions = await _context.Questions.ToListAsync();
                model.ReportList = await _context.Reports.Include(r => r.QuestionReply).ToListAsync();


                var report = new Report
                {
                    AgedCareCenterId = -1,
                    AssessorId = -1,
                    ReportDate = DateTime.Now,
                };

                report.QuestionReply = Questions.Select(q => new QuestionReply { QuestionId = q.QuestionId, Response = false, Question = q }).ToList();
                model.NewReport = report;
            }
            catch (Exception ex)
            {

                throw;
            }
            return CreatedAtAction("Sync", model);
        }
    }

    public class SyncModel
    {
        public List<AgedCareCenter> AgedCareCenterList { get; set; }
        public List<Assessor> AssessorList { get; set; }
        public List<Report> ReportList { get; set; }
        public List<AccreditationStandart> AccreditationStandartList { get; set; }
        public Report NewReport { get; set; }
    }
}