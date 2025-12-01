using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.StartModuleProgress
{
    public sealed record StartModuleProgressCommand : ICommand
    {
        public StartModuleProgressCommand(Guid courseId, Guid moduleId)
        {
            CourseId = courseId;
            ModuleId = moduleId;
        }

        public Guid CourseId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid StudentId { get; private set; }
        public StartModuleProgressCommand SetStudentId(Guid studentId)
        {
            return this with { StudentId = studentId };
        }
    }
}