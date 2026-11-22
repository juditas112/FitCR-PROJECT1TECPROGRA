namespace FitCRController
{
    /// <summary>Validations shared by several controllers. Each method returns the error message, or null if the value is valid.</summary>
    public static class Validation
    {
        public static string? WeightError(double weight)
        {
            return weight < 20 || weight > 300 ? "The weight must be between 20 and 300 kg." : null;
        }

        public static string? HeightError(double height)
        {
            return height < 1.0 || height > 2.5 ? "The height must be between 1.0 and 2.5 meters." : null;
        }

        public static bool HasComma(params string[] texts)
        {
            return texts.Any(text => text.Contains(','));
        }
    }
}
