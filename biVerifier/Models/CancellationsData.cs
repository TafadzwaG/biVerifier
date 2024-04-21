namespace biVerifier.Models
{
    public class CancellationsData
    {
        public int ID { get; set; }
        public string? Client { get; set; }
        public string? SageAcc { get; set; }
        public string? Site { get; set; }
        public string? Contact_Person { get; set; }
        public string? Account_Manager { get; set; }
        public string? Cancellation_Month { get; set; }
        public string? Cancellation_End_Date { get; set; }

        public string? Reason { get;  set; }
        public string? Notes { get;  set; }
        public string? Reduced_Value_Ex_Vat { get;  set; }
        public string? TechResponsible { get; set; }

        public string? Total_Channels { get; set; }
        public string? Platform { get; set; }
        public string? Cancel_GSM { get; set; }
        public string? Cancel_DNS { get; set; }
        public string? Cancel_LPR_Licenses { get; set; }
        public string? Cancel_Video_Analytics_Licenses { get; set; }
        public string? Cancel_Internet_Connectivity { get; set; }
        public string? Cancel_Billing { get; set; }
        public string? Status { get; set; }

    }
}
