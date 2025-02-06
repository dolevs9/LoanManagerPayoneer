using LoadManager.Models;

namespace LoadManager.Repositories
{
    public interface ILoansRepository : IRepository
    {
        List<Loan> GetAllLoans();
        void Insert(Loan loan);
    }
}