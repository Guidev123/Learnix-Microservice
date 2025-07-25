namespace Learnix.Commons.Application.Clock
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}