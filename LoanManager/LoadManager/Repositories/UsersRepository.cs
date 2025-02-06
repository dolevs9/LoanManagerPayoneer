using LoadManager.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace LoadManager.Repositories
{
    /// <summary>
    /// Repository to manage users
    /// </summary>
    public class UsersRepository : IRepository, IUsersRepository
    {
        IDataContext _dataContext;

        public UsersRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dataContext.AddLocation(GetType().Name);
        }

        public int GetAgeForPersonId(string id)
        {
            return _dataContext.RetrieveItem<User>(GetType().Name, id).Age;
        }
    }
}
