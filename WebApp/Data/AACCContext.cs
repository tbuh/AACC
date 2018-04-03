using Microsoft.EntityFrameworkCore;

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
    }
}
