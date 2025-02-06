using LoadManager.Models;

namespace LoadManager.Repositories
{
    public interface IUsersRepository : IRepository
    {
        int GetAgeForPersonId(string id);
    }
}