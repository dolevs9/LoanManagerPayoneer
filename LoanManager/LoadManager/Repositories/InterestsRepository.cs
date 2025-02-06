using LoadManager.Models;

namespace LoadManager.Repositories
{
    /// <summary>
    /// Repository to keep Interest loan configurations, 
    /// like minimum loan duration, prime interest, adjusted months interest
    /// </summary>
    public class InterestsRepository : IRepository, IInterestsRepository
    {
        /// <summary>
        /// Supported types of configurations in the DB
        /// </summary>
        public enum InterestType
        {
            Prime,
            AdjustedMonths,
            MinimumLoanPeriod
        }

        IDataContext _dataContext;

        public InterestsRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.AddLocation(GetType().Name);
        }

        public double GetAdjustedMonthsInterestRate(int year)
        {
            return _dataContext.Query<Interest>(GetType().Name,
                item => item.Description == InterestType.AdjustedMonths.ToString() && item.Year == year)[0].Value;
        }

        public double GetPrimeInterestRate(int year)
        {
            return _dataContext.Query<Interest>(GetType().Name,
                item => item.Description == InterestType.Prime.ToString() && item.Year == year)[0].Value;
        }

        public double GetMinimumPeriodLoanDuration(int year)
        {
            return _dataContext.Query<Interest>(GetType().Name,
                item => item.Description == InterestType.MinimumLoanPeriod.ToString() && item.Year == year)[0].Value;
        }
    }
}
