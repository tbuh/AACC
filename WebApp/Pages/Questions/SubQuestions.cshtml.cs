using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Pages.Questions
{
    public class SubQuestionsModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public SubQuestionsModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public Question Question { get; set; }
        public int ParentId { get; set; }
        [BindProperty]
        public List<Question> SubQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ParentId = id.Value;
            Question = _context.Questions.SingleOrDefault(m => m.QuestionId == id.Value);
            SubQuestions = await _context.Questions.Where(m => m.ParentId == id).OrderBy(q => q.QuestionId).ToListAsync();
            return Page();
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}