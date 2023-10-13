namespace visionet_webapi.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException() : base("Forbidden")
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }
    }
}
