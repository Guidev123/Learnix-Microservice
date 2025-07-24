using Learnix.Commons.Domain.Results;

namespace Learnix.Commons.Application.Exceptions
{
    public sealed class LearnixException : Exception
    {
        public LearnixException(string requestName, Error? error = default, Exception? innerException = default)
            : base("Application exception", innerException)
        {
            RequestName = requestName;
            Error = error;
        }

        public string RequestName { get; }

        public Error? Error { get; }
    }
}