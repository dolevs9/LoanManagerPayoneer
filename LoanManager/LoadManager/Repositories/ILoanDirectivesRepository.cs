using LoadManager.Models;

namespace LoadManager.Repositories
{
    public interface ILoanDirectivesRepository : IRepository
    {
        LoanDirective FindItemForAgeAndDebt(int age, long debt);
    }
}