namespace biVerifier.Models
{
    public class Licenses
    {
        public int ID { get; set; }
        public string? Sites { get; set; }
        public string? Requestor { get; set; }
        public string? ChangeDate { get; set; }
        public string? ChangeCode { get; set; }
        public string? CurrentAI { get; set; }
        public string? NewAI { get; set;}
        public string? LicensesNo { get; set; }
        public string? Cost { get; set; }
        public string? ChangeNotes { get; set; }
    }
}
