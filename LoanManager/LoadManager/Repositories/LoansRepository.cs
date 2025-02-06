using LoadManager.Models;

namespace LoadManager.Repositories
{
    /// <summary>
    /// Repository to save loans taken by user
    /// </summary>
    public class LoansRepository : IRepository, ILoansRepository
    {
        IDataContext _dataContext;

        public LoansRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.AddLocation(GetType().Name);
        }

        public List<Loan> GetAllLoans()
        {
            return _dataContext.Query<Loan>(GetType().Name, item => true);
        }

        public void Insert(Loan loan)
        {
            _dataContext.SerializeItem(GetType().Name,$"{loan.LoanId}",loan);
        }
    }
}
