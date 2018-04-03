﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Models;

namespace WebApp.Pages.AccreditationStandarts
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
        public AccreditationStandart AccreditationStandart { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AccreditationStandarts.Add(AccreditationStandart);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}