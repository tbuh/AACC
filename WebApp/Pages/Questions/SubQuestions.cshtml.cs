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
        [BindProperty]
        public List<SubQuestion> SubQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (Question == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(Question.SubQuestions))
            {
                SubQuestions = new List<SubQuestion>
                {
                    new SubQuestion{ Id = 1},
                    new SubQuestion{ Id = 2},
                    new SubQuestion{ Id = 3},
                    new SubQuestion{ Id = 4}
                };
            }
            else
                SubQuestions = JsonConvert.DeserializeObject<List<SubQuestion>>(Question.SubQuestions);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Question = await _context.Questions.SingleOrDefaultAsync(m => m.QuestionId == id);
            if (Question == null)
            {
                return NotFound();
            }
            Question.SubQuestions = JsonConvert.SerializeObject(SubQuestions);
            _context.Attach(Question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(Question.QuestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("Details", new { id = Question.AccreditationStandartId });
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionId == id);
        }
    }
}