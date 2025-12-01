using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.StartLessonProgress
{
    public sealed record StartLessonProgressCommand : ICommand
    {
        public StartLessonProgressCommand(Guid courseId, Guid moduleId, Guid lessonId)
        {
            CourseId = courseId;
            ModuleId = moduleId;
            LessonId = lessonId;
        }

        public Guid CourseId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid LessonId { get; init; }
        public Guid StudentId { get; private set; }

        public StartLessonProgressCommand SetStudentId(Guid studentId)
        {
            return this with { StudentId = studentId };
        }
    }
}