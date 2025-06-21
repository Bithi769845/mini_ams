namespace mini_AMS.Models
{
    public class VoucherModel
    {
        public int Id { get; set; }
        public string VoucherType { get; set; } = null!;
        public DateTime Date { get; set; }
        public string ReferenceNo { get; set; } = null!;
        public List<VoucherEntry> Entries { get; set; } = new();
    }
}
