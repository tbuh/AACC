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
        public List<Question> SubQuestions { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            
            Question = await _context.Questions.Include(q => q.Questions).SingleOrDefaultAsync(m => m.QuestionId == id);
            if (Question == null)
            {
                return NotFound();
            }

            SubQuestions = Question.Questions.ToList();
            for (int i = 0; i < 4 - Question.Questions.Count; i++)
            {
                SubQuestions.Add(new Question { ParentId = id });
            }
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

            Question.Questions = new List<Question>();
            foreach (var item in SubQuestions)
            {
                if (string.IsNullOrEmpty(item.Title)) continue;
                Question.Questions.Add(item);
                if (item.QuestionId == 0)
                    _context.Questions.Add(item);
                else
                    _context.Attach(item).State = EntityState.Modified;
            }

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