using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebApp.Models
{
    public class AACCContext : DbContext
    {
        public AACCContext(DbContextOptions<AACCContext> options)
            : base(options)
        {



        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Assessor> Assessors { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionReply> QuestionReplies { get; set; }
        public DbSet<AgedCareCenter> AgedCareCenters { get; set; }
        public DbSet<AccreditationStandart> AccreditationStandarts { get; set; }


        public async System.Threading.Tasks.Task UpdateCompletionStatus(Report report)
        {
            int cntQ = await Questions.CountAsync();
            int cntR = 0;
            if (report.QuestionReply != null)
                cntR = report.QuestionReply.Count(qr => qr.Response && qr.ReportId == report.ReportId);

            report.CompletionStatus = System.Math.Round((double)100 * cntR / cntQ);
        }

        public async System.Threading.Tasks.Task SaveReport(Report report)
        {
            Reports.Add(report);
            await UpdateCompletionStatus(report);
            await SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateReport(Report report)
        {
            await UpdateCompletionStatus(report);
            Attach(report).State = EntityState.Modified;

            foreach (var qr in report.QuestionReply)
            {
                if (qr.QuestionReplyId != 0) Attach(qr).State = EntityState.Modified;
            }
        }
    }
}
