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
        public DbSet<QuestionsL1> QuestionsL1s { get; set; }
        public DbSet<QuestionsL2> QuestionsL2s { get; set; }
        public DbSet<AgedCareCenter> AgedCareCenters { get; set; }
        public DbSet<AccreditationStandartL1> AccreditationStandartL1s { get; set; }
        public DbSet<AccreditationStandartL2> AccreditationStandartL2s { get; set; }
    }
}
