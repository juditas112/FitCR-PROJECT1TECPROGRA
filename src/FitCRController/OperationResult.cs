namespace FitCRController
{
    /// <summary>Result of an operation of the controllers, so the views can show a message without handling exceptions.</summary>
    public class OperationResult
    {
        public OperationResult(bool success, string message, string id = "")
        {
            this.Success = success;
            this.Message = message;
            this.Id = id;
        }

        public bool Success { get; }

        public string Message { get; }

        public string Id { get; }

        /// <summary>Creates a successful result.</summary>
        public static OperationResult Ok(string message, string id = "")
        {
            return new OperationResult(true, message, id);
        }

        /// <summary>Creates a failed result.</summary>
        public static OperationResult Fail(string message)
        {
            return new OperationResult(false, message);
        }
    }
}
