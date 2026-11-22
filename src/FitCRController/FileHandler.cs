using FitCRController.Abstractions;
using System.Reflection;

namespace FitCRController
{
    /// <summary>Implementation for <see cref="IDataHandler"/> that handles the data operations using csv files.</summary>
    public class FileHandler : IDataHandler
    {
        /// <summary>Gets the errors found in the last load operation.</summary>
        public List<string> Errors { get; } = new List<string>();

        /// <summary>Loads the data.</summary>
        public List<T> LoadData<T>(string filename)
        {
            this.Errors.Clear();
            var data = new List<T>();

            if (string.IsNullOrWhiteSpace(filename) || !File.Exists(filename))
            {
                this.Errors.Add($"The file '{filename}' was not found.");
                return data;
            }

            string[] lines;
            try
            {
                lines = File.ReadAllLines(filename);
            }
            catch (IOException)
            {
                this.Errors.Add($"The file '{Path.GetFileName(filename)}' could not be opened.");
                return data;
            }

            for (var i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i]))
                {
                    continue;
                }

                try
                {
                    var lineElements = lines[i].Split(',');
                    data.Add((T)Activator.CreateInstance(typeof(T), lineElements)!);
                }
                catch (Exception ex) when (ex is TargetInvocationException || ex is MissingMethodException)
                {
                    this.Errors.Add($"Line {i + 1} of '{Path.GetFileName(filename)}' could not be read.");
                }
            }

            return data;
        }

        /// <summary>Saves the data.</summary>
        public bool SaveData<T>(string filename, List<T> data)
        {
            if (string.IsNullOrWhiteSpace(filename) || data == null)
            {
                return false;
            }

            try
            {
                var lines = new List<string>();

                if (File.Exists(filename))
                {
                    var header = File.ReadLines(filename).FirstOrDefault();
                    if (header != null)
                    {
                        lines.Add(header);
                    }
                }

                lines.AddRange(data.Select(element => element?.ToString() ?? string.Empty));
                File.WriteAllLines(filename, lines);
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                return false;
            }
        }
    }
}
