namespace MyWebNovel.Domain.Entities.Shared
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }

        public DomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public override string ToString()
        {
            // Include base message and inner exception details
            var innerExceptionMessage = InnerException?.ToString() ?? "No inner exception.";
            return $"{base.ToString()}\nInner Exception: {innerExceptionMessage}";
        }
    }
}
