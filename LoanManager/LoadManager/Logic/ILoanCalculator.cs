namespace LoadManager.Logic
{
    public interface ILoanCalculator
    {
        double CalculateLoanInterest(string personId, long dept, int loanDurationInMonths);
    }
}