namespace Learnix.Commons.Domain.DomainObjects
{
    public class DomainException : Exception
    {
        public DomainException(string? message) : base(message)
        {
        }
    }
}