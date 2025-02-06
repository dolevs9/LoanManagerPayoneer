namespace LoadManager.Models
{
    /// <summary>
    /// Loan data taken by user
    /// </summary>
    public class Loan
    {
        public Guid LoanId { get; set; }
        public string UserId { get; set; }
        public int DurationInMonths { get; set; }
        public int BaseLoanDebt { get; set; }
        public double RefundDeptPrice { get; set; }
        public DateTime LoanStartDate { get; set; }
    }
}
