using Learning.Domain.Enrollments.Errors;
using Learnix.Commons.Domain.DomainObjects;
using Learnix.Commons.Domain.ValueObjects;

namespace Learning.Domain.Enrollments.ValueObjects
{
    public sealed record CourseProgressDateRange : ValueObject
    {
        public CourseProgressDateRange(DateTime startedAt, DateTime? completedAt = null)
        {
            StartedAt = startedAt;
            CompletedAt = completedAt;
            Validate();
        }

        private CourseProgressDateRange()
        { }

        public DateTime StartedAt { get; }
        public DateTime? CompletedAt { get; }
        public bool IsCompleted => CompletedAt.HasValue;

        public static implicit operator CourseProgressDateRange(DateTime startedAt)
            => new(startedAt);

        public static implicit operator CourseProgressDateRange((DateTime startedAt, DateTime? completedAt) dateRange)
            => new(dateRange.startedAt, dateRange.completedAt);

        protected override void Validate()
        {
            AssertionConcern.EnsureTrue(StartedAt <= DateTime.UtcNow, CourseErrors.ProgresStartedDateCanNotBeInFuture.Description);
            if (IsCompleted)
            {
                AssertionConcern.EnsureTrue(CompletedAt!.Value >= StartedAt, CourseErrors.ProgressCompletedDateMustBeAfterStartedDate.Description);
            }
        }
    }
}