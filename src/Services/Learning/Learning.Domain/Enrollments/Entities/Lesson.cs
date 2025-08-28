using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Lesson : Entity
    {
        public Lesson(Guid id, Guid moduleId, uint durationInMinutes)
        {
            Id = id;
            ModuleId = moduleId;
            DurationInMinutes = durationInMinutes;
        }

        public Guid ModuleId { get; private set; }
        public uint DurationInMinutes { get; private set; }

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}