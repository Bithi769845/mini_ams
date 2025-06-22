using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using mini_AMS.Services;

namespace mini_AMS.Pages.ChartOfAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly ChartOfAccountsService _service;

        public DeleteModel(ChartOfAccountsService service) => _service = service;

        [BindProperty] public int Id { get; set; }

        public void OnGet(int id) => Id = id;

        public IActionResult OnPost()
        {
            _service.DeleteAccount(Id);
            return RedirectToPage("/ChartOfAccounts/Index");
        }
    }
}
