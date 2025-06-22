using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using mini_AMS.Models;
using System.Data;

namespace mini_AMS.Services
{
    public class ChartOfAccountsService
    {
        private readonly string _connectionString;

        public ChartOfAccountsService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<ChartOfAccount> GetAllAccounts()
        {
            var results = new List<ChartOfAccount>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Action", "GETALL");

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(new ChartOfAccount
                {
                    Id = (int)reader["Id"],
                    Name = reader["Name"].ToString()!,
                    ParentId = reader["ParentId"] == DBNull.Value ? null : (int?)reader["ParentId"]
                });
            }

            return results;
        }

        public void CreateAccount(string name, int? parentId) =>
            ExecuteNonQuery("CREATE", null, name, parentId);

        public void UpdateAccount(int id, string name, int? parentId) =>
            ExecuteNonQuery("UPDATE", id, name, parentId);

        public void DeleteAccount(int id) =>
            ExecuteNonQuery("DELETE", id, null, null);

        private void ExecuteNonQuery(string action, int? id, string? name, int? parentId)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@Id", id ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", (object?)name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ParentId", parentId ?? (object)DBNull.Value);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
