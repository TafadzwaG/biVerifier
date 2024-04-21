namespace biVerifier.Models
{
    public class LicensesData
    {
        public int ID { get; set; }
        public string? Sites { get; set; }
        public string? Requestor { get; set;}
        public string? ChangeDate { get; set;}
        public string? ChangeCode { get; set;}
        public string? CurrentAI { get; set;}
        public string? NewAI { get; set;}
        public int? LicensesNo { get; set;}
        public int? Cost { get; set;}

        public string? Change_Notes { get; set;}

    }
}
