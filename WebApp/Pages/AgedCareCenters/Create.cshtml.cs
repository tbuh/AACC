using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;

namespace WebApp.Pages.AgedCareCenters
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
        public AgedCareCenter AgedCareCenter { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var file = HttpContext.Request.Form.Files;
            if (file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file[0].CopyToAsync(memoryStream);

                    AgedCareCenter.Logo = memoryStream.ToArray();
                }
            }

            _context.AgedCareCenters.Add(AgedCareCenter);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}