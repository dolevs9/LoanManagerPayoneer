using LoadManager.Models;

namespace LoadManager.Repositories
{
    /// <summary>
    /// Repository to handle rules of interest related to age and loan dept
    /// </summary>
    public class LoanDirectivesRepository : ILoanDirectivesRepository
    {
        IDataContext _dataContext;

        public LoanDirectivesRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.AddLocation(GetType().Name);
        }

        public LoanDirective FindItemForAgeAndDebt(int age, long debt)
        {
            List<LoanDirective> matchingItems = _dataContext.Query<LoanDirective>(GetType().Name,
                item => item.MinAge < age && item.MaxAge > age && item.MinDebt < debt && item.MaxDebt > debt);
            return matchingItems[0];
        }
    }
}
