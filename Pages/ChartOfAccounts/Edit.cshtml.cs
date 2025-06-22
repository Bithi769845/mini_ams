using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using mini_AMS.Services;

namespace mini_AMS.Pages.ChartOfAccounts
{
    public class EditModel : PageModel
    {
        private readonly ChartOfAccountsService _service;

        public EditModel(ChartOfAccountsService service) => _service = service;

        [BindProperty] public int Id { get; set; }
        [BindProperty] public string Name { get; set; } = null!;
        [BindProperty] public int? ParentId { get; set; }

        public void OnGet(int id)
        {
            // You can load the data from service
            Id = id;

            // Get all accounts
            var allAccounts = _service.GetAllAccounts();
            var account = allAccounts.FirstOrDefault(a => a.Id == id);

            if (account != null)
            {
                Name = account.Name;
                ParentId = account.ParentId;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            _service.UpdateAccount(Id, Name, ParentId);
            return RedirectToPage("/ChartOfAccounts/Index");
        }
    }
}
