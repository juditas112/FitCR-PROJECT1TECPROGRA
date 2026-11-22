namespace FitCRController.Abstractions
{
    /// <summary>Interface for data handlers, which defines the contract for the classes that load and save data.</summary>
    public interface IDataHandler
    {
        public List<string> Errors { get; }

        /// <summary>Saves the data.</summary>
        public bool SaveData<T>(string filename, List<T> data);

        /// <summary>Loads the data.</summary>
        public List<T> LoadData<T>(string filename);
    }
}
