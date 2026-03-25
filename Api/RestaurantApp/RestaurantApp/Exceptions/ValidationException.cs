namespace RestaurantApp.Exceptions
{
    public class ValidationException : Exception
    {
        public IEnumerable<string>? Errors { get; }

        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(IEnumerable<string> errors) : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }
}
