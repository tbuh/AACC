using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;

namespace WebApp.Pages.Questions
{
    public class CreateModel : PageModel
    {
        private readonly WebApp.Models.AACCContext _context;

        public CreateModel(WebApp.Models.AACCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
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

            Question.AccreditationStandartId = id;
            _context.Questions.Add(Question);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AccreditationStandarts/Index");
        }
    }
}