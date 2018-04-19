using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public IndexModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public IList<Report> Reports { get; set; }
        public bool CanModify(Report r)
        {
            if (!IsSuperAdmin && ReportsToModify == null) return false;
            return IsSuperAdmin || ReportsToModify.Contains(r.ReportId);
        }

        private List<int> ReportsToModify { get; set; }
        private bool IsSuperAdmin { get; set; }

        public async Task OnGetAsync()
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
            var admin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Admin")?.Value;
            var superadmin = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "SuperAdmin")?.Value;

            IsSuperAdmin = !string.IsNullOrEmpty(superadmin);
            Reports = await _context.Reports.Include(r => r.AgedCareCenter).Include(r => r.Assessor).ToListAsync();

            if (!string.IsNullOrEmpty(userId) && userId != "0")
            {
                var user = await _context.Assessors.SingleAsync(a => a.AssessorId == int.Parse(userId));
                if (user.IsAdmin)
                {
                    var team = await _context.Teams.SingleAsync(a => a.TeamId == user.TeamId);
                    var sql = $@"
select r.* from [dbo].[Reports] r
where r.[AssessorId] in (select a.[AssessorId] from [dbo].[Assessors] a
where a.[TeamId] = ${team.TeamId})";

                    ReportsToModify = await _context.Reports.FromSql(sql)
                        .Select(r => r.ReportId)
                        .ToListAsync();
                }
                else
                {
                    ReportsToModify = await _context.Reports
                        .Where(r => r.AssessorId == user.AssessorId)
                        .Select(r => r.ReportId).ToListAsync();
                }
            }
        }
    }
}
