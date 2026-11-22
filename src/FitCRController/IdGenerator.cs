namespace FitCRController
{
    /// <summary>Generates the identifiers of the new elements, for example U046 after U045.</summary>
    public static class IdGenerator
    {
        public static string Next(string prefix, IEnumerable<string> existingIds, int digits)
        {
            var max = existingIds
                .Where(id => id.StartsWith(prefix, StringComparison.Ordinal))
                .Select(id => int.TryParse(id.AsSpan(prefix.Length), out var number) ? number : 0)
                .DefaultIfEmpty(0)
                .Max();

            return prefix + (max + 1).ToString("D" + digits, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
