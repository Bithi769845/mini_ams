using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace mini_AMS.Pages.Vouchers
{
    public class IndexModel : PageModel
    {
        
            private readonly IConfiguration _config;

            public IndexModel(IConfiguration config) => _config = config;

            public List<VoucherListItem> Vouchers { get; set; } = new();

            public void OnGet()
            {
                using var conn = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
                conn.Open();

                // STEP 1: Get Main Vouchers
                using var cmd = new SqlCommand("SELECT Id, VoucherType, Date, ReferenceNo FROM Vouchers", conn);
                using var reader = cmd.ExecuteReader();
                var voucherData = new List<VoucherListItem>();

                while (reader.Read())
                {
                    voucherData.Add(new VoucherListItem
                    {
                        Id = (int)reader["Id"],
                        VoucherType = reader["VoucherType"].ToString(),
                        Date = (DateTime)reader["Date"],
                        ReferenceNo = reader["ReferenceNo"].ToString()
                    });
                }
                reader.Close();

                // STEP 2: Get Entries for All Voucher Ids
                foreach (var v in voucherData)
                {
                    using var entryCmd = new SqlCommand(@"SELECT e.AccountId, a.Name as AccountName, e.Debit, e.Credit
                                                  FROM VoucherEntries e
                                                  INNER JOIN ChartOfAccounts a ON e.AccountId = a.Id
                                                  WHERE e.VoucherId = @VoucherId", conn);
                    entryCmd.Parameters.AddWithValue("@VoucherId", v.Id);
                    using var entryReader = entryCmd.ExecuteReader();

                    v.Entries = new List<EntryItem>();
                    while (entryReader.Read())
                    {
                        v.Entries.Add(new EntryItem
                        {
                            AccountId = (int)entryReader["AccountId"],
                            AccountName = entryReader["AccountName"].ToString(),
                            Debit = Convert.ToDecimal(entryReader["Debit"]),
                            Credit = Convert.ToDecimal(entryReader["Credit"])
                        });
                    }
                }

                Vouchers = voucherData;
            }

            public class VoucherListItem
            {
                public int Id { get; set; }
                public string VoucherType { get; set; }
                public DateTime Date { get; set; }
                public string ReferenceNo { get; set; }
                public List<EntryItem> Entries { get; set; } = new();
            }

            public class EntryItem
            {
                public int AccountId { get; set; }
                public string AccountName { get; set; }
                public decimal Debit { get; set; }
                public decimal Credit { get; set; }
            }
   
    }
}
