using System;
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

        private void CreateStandartTypeList()
        {
            StandartType = new List<SelectListItem> {
                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L1).ToString(),
                               Text = "L1"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L2).ToString(),
                               Text = "L2"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L3).ToString(),
                               Text = "L3"
                           }
                           ,                           new SelectListItem()
                           {
                               Value = ((int)WebApp.Models.StandartType.L4).ToString(),
                               Text = "L4"
                           } };
        }


        public IActionResult OnGet()
        {
            CreateStandartTypeList();
            return Page();
        }

        [BindProperty]
        public AccreditationStandart AccreditationStandart { get; set; }
        public IEnumerable<SelectListItem> StandartType { get; set; }

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