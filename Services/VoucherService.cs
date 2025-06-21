using Microsoft.Data.SqlClient;
using mini_AMS.Models;
using System.Data;

namespace mini_AMS.Services
{
    public class VoucherService
    {
        private readonly string _connectionString;

        public VoucherService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void SaveVoucher(string voucherType, DateTime date, string referenceNo, List<VoucherEntry> entries)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_SaveVoucher", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@VoucherType", voucherType);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@ReferenceNo", referenceNo);

            var table = new DataTable();
            table.Columns.Add("AccountId", typeof(int));
            table.Columns.Add("Debit", typeof(decimal));
            table.Columns.Add("Credit", typeof(decimal));

            foreach (var entry in entries)
            {
                table.Rows.Add(entry.AccountId, entry.Debit, entry.Credit);
            }

            var param = cmd.Parameters.AddWithValue("@Entries", table);
            param.SqlDbType = SqlDbType.Structured;
            param.TypeName = "dbo.VoucherEntriesType";

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
