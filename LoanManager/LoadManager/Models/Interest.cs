namespace LoadManager.Models
{
    /// <summary>
    /// Specific single interest rules data saved in the db and can be changed as needed
    /// </summary>
    public class Interest
    {
        public int Year { get; set; }
        public string Description { get; set; }
        public double Value { get; set; }
    }
}
