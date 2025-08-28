using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class CourseProgress : Entity
    {
        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }
        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }

    public sealed record DateRange : ValueObject
    {
        private DateRange(DateTime startedAt, DateTime? finishedAt)
        {
            StartedAt = startedAt;
            FinishedAt = finishedAt;
            Validate();
        }

        public DateTime StartedAt { get; private set; }
        public DateTime? FinishedAt { get; private set; }

        public static implicit operator DateRange((DateTime startDate, DateTime finishedAt) range)
        {
            return new DateRange(range.startDate, range.finishedAt);
        }

        protected override void Validate()
        {
            AssertionConcern.EnsureDifferent(StartedAt, DateTime.UtcNow);
            AssertionConcern.EnsureGreaterThan(FinishedAt, StartedAt);
        }
    }
}
