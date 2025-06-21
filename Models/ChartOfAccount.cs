using System.Collections.Generic;
namespace mini_AMS.Models
{
    public class ChartOfAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? ParentId { get; set; }
        public List<ChartOfAccount> Children { get; set; } = new List<ChartOfAccount>();
    }
}
