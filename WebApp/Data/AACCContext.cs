using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
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
            Reports.Add(report).State = EntityState.Added;
            foreach (var item in report.QuestionReply)
            {
                QuestionReplies.Add(item).State = EntityState.Added;
                if (item.Question != null) Questions.Add(item.Question).State = EntityState.Unchanged;
            }

            await UpdateCompletionStatus(report);
            await SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task UpdateReport(Report report)
        {
            await UpdateCompletionStatus(report);
            Attach(report).State = EntityState.Modified;

            foreach (var qr in report.QuestionReply)
            {
                if (qr.QuestionReplyId != 0)
                {
                    var attached = Attach(qr);
                    if (qr.ReportImage == null)
                        attached.Property(x => x.ReportImage).IsModified = false;
                    else
                        attached.Property(x => x.ReportImage).IsModified = true;

                    attached.Property(x => x.Notes).IsModified = true;
                }
            }

            await SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task<IQueryable<Report>> GetReports(int userId)
        {
            var user = await Assessors.SingleAsync(a => a.AssessorId == userId);
            if (user.IsAdmin)
            {
                var team = await Teams.SingleAsync(a => a.TeamId == user.TeamId);
                var sql = $@"
select r.* from [dbo].[Reports] r
where r.[AssessorId] in (select a.[AssessorId] from [dbo].[Assessors] a
where a.[TeamId] = ${team.TeamId})";

                return Reports.FromSql(sql).Include(r => r.QuestionReply);
            }
            else
            {
                return Reports.Where(r => r.AssessorId == user.AssessorId);
            }
        }

        public async System.Threading.Tasks.Task<List<Report>> GetReportsWithReplies(int userId)
        {
            if (userId < 0)
                return await Reports.Include(r => r.QuestionReply).ToListAsync();

            var user = await Assessors.SingleAsync(a => a.AssessorId == userId);
            if (user.IsAdmin)
            {
                var team = await Teams.SingleAsync(a => a.TeamId == user.TeamId);
                var sql = $@"
select r.* from [dbo].[Reports] r
where r.[AssessorId] in (select a.[AssessorId] from [dbo].[Assessors] a
where a.[TeamId] = ${team.TeamId})";

                return await Reports.FromSql(sql).Include(r => r.QuestionReply).ToListAsync();
            }
            else
            {
                return await Reports.Include(r => r.QuestionReply).Where(r => r.AssessorId == user.AssessorId).ToListAsync();
            }
        }
    }
}
