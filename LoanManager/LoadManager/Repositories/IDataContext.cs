
namespace LoadManager.Repositories
{
    public interface IDataContext
    {
        List<Item> Query<Item>(string locationString, Func<Item, bool> conditionDelegate);
        Item RetrieveItem<Item>(string locationString, string key);
        void SaveChanges();
        void SerializeItem(string locationString, string key, object item);
        void AddLocation(string repositoryName);
    }
}