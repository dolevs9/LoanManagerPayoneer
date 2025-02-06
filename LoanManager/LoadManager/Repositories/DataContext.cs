using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace LoadManager.Repositories
{
    /// <summary>
    /// This data context simulates a db over a file system using jsons, each repository (like a table), 
    /// represented by a folder, each item represented by a file
    /// </summary>
    public class DataContext : IDataContext
    {
        //Keeping the data in-memory until using save changes, as used in real .net data context.
        Dictionary<string, Dictionary<string, string>> LocationToKeyToItemContent = new Dictionary<string, Dictionary<string, string>>();

        private string _fullPathToExeParentDir = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// Each repository represent item format, and its item content stays in a different folder
        /// </summary>
        /// <param name="repositoryName"></param>
        public void AddLocation(string repositoryName)
        {
            string repositoryDirPath = _fullPathToExeParentDir + "\\" + repositoryName;
            var dir = Directory.CreateDirectory(repositoryDirPath);
        }

        /// <summary>
        /// Writes a specific item
        /// </summary>
        /// <param name="repositoryName"></param>
        /// <param name="key"></param>
        /// <param name="item"></param>
        public void SerializeItem(string repositoryName, string key, object item)
        {
            //Save item to memory
            //If missing hierarchy on its path create the hierarchy, if item exists override it.
            if(!LocationToKeyToItemContent.ContainsKey(repositoryName))
                LocationToKeyToItemContent.Add(repositoryName, new Dictionary<string, string>());

            if(!LocationToKeyToItemContent[repositoryName].ContainsKey(key))
                LocationToKeyToItemContent[repositoryName].Add(key, JsonConvert.SerializeObject(item));
            else
                LocationToKeyToItemContent[repositoryName][key] = JsonConvert.SerializeObject(item);
        }

        /// <summary>
        /// Retrieve specific item by key
        /// </summary>
        /// <typeparam name="Item"></typeparam>
        /// <param name="repositoryName"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Item RetrieveItem<Item>(string repositoryName, string key)
        {
            //Check if theres a in-memory version, if so then take it
            if(LocationToKeyToItemContent.ContainsKey(repositoryName) &&
                LocationToKeyToItemContent[repositoryName].ContainsKey(key))
                return (Item)JsonConvert.DeserializeObject(LocationToKeyToItemContent[repositoryName][key]);

            //If in memory version does not exist take the file version
            string content = File.ReadAllText($"{_fullPathToExeParentDir}\\{repositoryName}\\{key.ToString()}.dat");
            return (Item)JsonConvert.DeserializeObject(content, typeof(Item));
        }

        /// <summary>
        /// Queries files for the data requested, supports yet only saved data
        /// </summary>
        /// <typeparam name="Item"></typeparam>
        /// <param name="repositoryName">Name of the repository with the data</param>
        /// <param name="conditionDelegate">A delegate that tests if the data on each item matches its condition</param>
        /// <returns>All the items that matches the delegate condition</returns>
        public List<Item> Query<Item>(string repositoryName, Func<Item, bool> conditionDelegate)
        {
            //Get all the items in a specific repository dir
            List<Item> matchingItems = new List<Item>();
            string[] allFiles = Directory.GetFiles($"{_fullPathToExeParentDir}\\{repositoryName}");

            //For each item in the directory deserialize it to identify if it matches the query
            foreach(string fileName in allFiles)
            {
                //Read and deserialize the item
                string serializedItem = File.ReadAllText($"{fileName}");
                Item item = (Item)JsonConvert.DeserializeObject(serializedItem,typeof(Item));

                //Test the query result for the specific item
                if(conditionDelegate(item))
                    matchingItems.Add(item);
            }

            return matchingItems;
        }

        /// <summary>
        /// Saves the data to files and clear the memory
        /// </summary>
        public void SaveChanges()
        {
            //Run over all the saved data in memory
            foreach(KeyValuePair<string, Dictionary<string, string>> repositoryName in LocationToKeyToItemContent)
            {
                //Create the repository dir if not exists
                string repoDir = $"{_fullPathToExeParentDir}\\{repositoryName.Key}";
                if(Directory.Exists(repoDir))
                    Directory.CreateDirectory(repoDir);

                //Write each saved item as relevant file
                foreach(KeyValuePair<string, string> keyAndItem in repositoryName.Value)
                {
                    File.WriteAllText($"{repoDir}\\{keyAndItem.Key}.dat", keyAndItem.Value);
                }
            }

            //Clear the memory from all the data we just saved
            LocationToKeyToItemContent.Clear();
        }

    }
}
