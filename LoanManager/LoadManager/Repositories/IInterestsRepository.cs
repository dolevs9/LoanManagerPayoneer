using LoadManager.Models;

namespace LoadManager.Repositories
{
    public interface IInterestsRepository : IRepository
    {
        double GetAdjustedMonthsInterestRate(int year);
        double GetPrimeInterestRate(int year);
        double GetMinimumPeriodLoanDuration(int year);
    }
}