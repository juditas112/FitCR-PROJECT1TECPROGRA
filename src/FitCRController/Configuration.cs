using FitCRModels;

namespace FitCRController
{
    /// <summary>Static class for the application configuration settings.</summary>
    public static class Configuration
    {
        private const string FilesFolderName = "Files";

        private static readonly string FilesFolderPath = FindFilesFolder();

        private static readonly Dictionary<Type, string> FileNames = new Dictionary<Type, string>
        {
            { typeof(BasicUser), "users.csv" },
            { typeof(Trainer), "trainers.csv" },
            { typeof(GymClass), "gymclasses.csv" },
            { typeof(Routine), "routines.csv" },
            { typeof(Exercise), "exercises.csv" },
            { typeof(RoutineIncrease), "routineincreases.csv" },
            { typeof(Visit), "visits.csv" },
            { typeof(Measurement), "measurements.csv" },
            { typeof(Goal), "goals.csv" },
            { typeof(Badge), "badges.csv" },
            { typeof(Notification), "notifications.csv" },
        };

        public static string FilesFolder => FilesFolderPath;

        /// <summary>Gets the path of the file that stores a type.</summary>
        public static string GetFilePath<T>()
        {
            return Path.Combine(FilesFolderPath, FileNames[typeof(T)]);
        }

        private static string FindFilesFolder()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);
            while (directory != null)
            {
                var candidate = Path.Combine(directory.FullName, FilesFolderName);
                if (Directory.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            return Path.Combine(AppContext.BaseDirectory, FilesFolderName);
        }
    }
}
