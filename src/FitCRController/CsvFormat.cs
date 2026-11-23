using System.Globalization;

namespace FitCRController
{
    /// <summary>Converts values to the text format used in the csv files, so the format is written only once.</summary>
    public static class CsvFormat
    {
        public static string Date(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static string DateAndTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public static string Number(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string Number(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string Code<T>(T value)
            where T : Enum
        {
            return Convert.ToInt32(value, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
        }
    }
}
