using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Assessors
{
    public class IndexModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public IndexModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public IList<Assessor> Assessor { get; set; }

        public async Task OnGetAsync()
        {

            Assessor = await _context.Assessors.Include(a => a.Team).ToListAsync();
        }
    }
}
