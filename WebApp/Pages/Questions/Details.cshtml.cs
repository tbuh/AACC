using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Pages.Questions
{
    public class DetailsModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public DetailsModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public List<Question> Questions { get; set; }
        public int StandartId { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            StandartId = id;

            Questions = await _context.Questions.Where(m => m.AccreditationStandartId == id).ToListAsync();

            if (Questions == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
