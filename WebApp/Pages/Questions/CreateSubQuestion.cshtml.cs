using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.Questions
{
    public class CreateSubQuestionModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public CreateSubQuestionModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int? ParentId { get; set; } 
        public IActionResult OnGet(int? parenId)
        {
            Question = new Question { ParentId = parenId };
            ParentId = parenId;
            return Page();
        }

        [BindProperty]
        public Question Question { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Question.ParentId = id;
            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("SubQuestions", new { id });
        }
    }
}