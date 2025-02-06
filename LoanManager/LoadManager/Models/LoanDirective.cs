namespace LoadManager.Models
{
    /// <summary>
    /// Interest rate rule depending on dept and age
    /// </summary>
    public class LoanDirective
    {
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public long MinDebt { get; set; }
        public long MaxDebt { get; set; }
        public double InterestRate { get; set; }
        public bool ShouldIncludePrime { get; set; }
    }
}
