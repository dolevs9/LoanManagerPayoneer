using LoadManager.Models;
using LoadManager.Repositories;

namespace LoadManager.Logic
{
    public class LoanCalculator : ILoanCalculator
    {
        IUsersRepository _usersRepository;
        ILoanDirectivesRepository _loanDirectiveRepository;
        IInterestsRepository _interestsRepository;
        IDataContext _dataContext;

        public LoanCalculator(IUsersRepository usersRepository, ILoanDirectivesRepository loanDirectiveRepository,
            IInterestsRepository interestsRepository, IDataContext dataContext)
        {
            _usersRepository = usersRepository;
            _loanDirectiveRepository = loanDirectiveRepository;
            _interestsRepository = interestsRepository;
            _dataContext = dataContext;
        }

        public double CalculateLoanInterest(string personId, long dept, int loanDurationInMonths)
        {
            //Get the age from id, and loan directive from age and dept
            int age = _usersRepository.GetAgeForPersonId(personId);
            LoanDirective loanDirective = _loanDirectiveRepository.FindItemForAgeAndDebt(age, dept);

            //Calculate amount of months above minimum and months interest rate according to amount of months
            int monthsAboveMinimumPeriod = loanDurationInMonths - (int)_interestsRepository.GetMinimumPeriodLoanDuration(DateTime.Today.Year);
            double adjustedMonthsInterestRate = (double)_interestsRepository.GetAdjustedMonthsInterestRate(DateTime.Today.Year);

            //Get base interest value and adjust the prime and extra months interest to it
            double extraMonthsInterest = (dept / 100) * adjustedMonthsInterestRate * monthsAboveMinimumPeriod;
            double interestPercentageAfterPrime = !loanDirective.ShouldIncludePrime ? 0 : _interestsRepository.GetPrimeInterestRate(DateTime.Today.Year);
            interestPercentageAfterPrime += loanDirective.InterestRate;
            double returnFunds = (dept / 100) * interestPercentageAfterPrime;

            return dept + returnFunds + extraMonthsInterest;
        }


    }
}
