using Learnix.Commons.Domain.DomainObjects;

namespace Learning.Domain.Enrollments.Entities
{
    public sealed class Course : Entity
    {
        private readonly List<Module> _modules = [];

        private Course(Guid id, string title, string description, bool isActive)
        {
            Id = id;
            Title = title;
            Description = description;
            IsActive = isActive;
            Validate();
        }

        private Course()
        { }

        public string Title { get; private set; } = null!;
        public string Description { get; private set; } = null!;
        public bool IsActive { get; private set; }
        public IReadOnlyCollection<Module> Modules => _modules.AsReadOnly();
        public uint ModulesQuantity => (uint)_modules.Count;
        public uint DurationInMinutes => (uint)_modules.Sum(m => m.DurationInMinutes);

        protected override void Validate()
        {
            throw new NotImplementedException();
        }
    }
}