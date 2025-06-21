using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using mini_AMS.Services;

namespace mini_AMS.Pages.ChartOfAccounts
{
    public class CreateModel : PageModel
    {
        private readonly ChartOfAccountsService _service;

        public CreateModel(ChartOfAccountsService service) => _service = service;

        [BindProperty] public string Name { get; set; } = null!;
        [BindProperty] public int? ParentId { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _service.CreateAccount(Name, ParentId);
            return RedirectToPage("/ChartOfAccounts/Index");
        }
    }
}
