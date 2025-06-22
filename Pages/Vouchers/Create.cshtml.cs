using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using mini_AMS.Models;
using mini_AMS.Services;

namespace mini_AMS.Pages.Vouchers
{
    public class CreateModel : PageModel
    {
        private readonly VoucherService _service;
        private readonly IConfiguration _config;

        public CreateModel(VoucherService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }
        [BindProperty]
        public VoucherModel Voucher { get; set; } = new();
        public List<AccountItem> Accounts { get; set; } = new();
        public void OnGet() {
            using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            using var cmd = new SqlCommand("SELECT Id, Name FROM ChartOfAccounts", conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Accounts.Add(new AccountItem
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()
                });
            }
        }
        public class AccountItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _service.SaveVoucher(Voucher.VoucherType, Voucher.Date, Voucher.ReferenceNo, Voucher.Entries);
            return RedirectToPage("/Vouchers/Index");
        }
    }
}
