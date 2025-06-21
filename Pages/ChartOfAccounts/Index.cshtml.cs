using Microsoft.AspNetCore.Mvc.RazorPages;
using mini_AMS.Services;
using mini_AMS.Models;

namespace mini_AMS.Pages.ChartOfAccounts
{
    public class IndexModel : PageModel
    {
        private readonly ChartOfAccountsService _service;

        public IndexModel(ChartOfAccountsService service)
        {
            _service = service;
        }

        public List<ChartOfAccount> RootAccounts { get; set; } = new List<ChartOfAccount>();

        public void OnGet()
        {
            var flatList = _service.GetAllAccounts();
            RootAccounts = BuildAccountTree(flatList);
        }

        private List<ChartOfAccount> BuildAccountTree(List<ChartOfAccount> flatList)
        {
            var lookup = flatList.ToDictionary(a => a.Id);
            var rootAccounts = new List<ChartOfAccount>();

            foreach (var item in flatList)
            {
                if (item.ParentId.HasValue && lookup.ContainsKey(item.ParentId.Value))
                    lookup[item.ParentId.Value].Children.Add(item);
                else
                    rootAccounts.Add(item);
            }

            return rootAccounts;
        }
    }
}
