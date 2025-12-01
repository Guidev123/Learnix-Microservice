using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.CompleteLessonProgress
{
    public sealed record CompleteLessonProgressCommand : ICommand
    {
        public CompleteLessonProgressCommand(Guid courseId, Guid moduleId, Guid lessonId, uint lessonDurationInMinutes)
        {
            CourseId = courseId;
            ModuleId = moduleId;
            LessonId = lessonId;
            LessonDurationInMinutes = lessonDurationInMinutes;
        }

        public Guid StudentId { get; private set; }
        public Guid CourseId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid LessonId { get; init; }
        public uint LessonDurationInMinutes { get; init; }
        public CompleteLessonProgressCommand SetStudentId(Guid studentId)
        {
            return this with { StudentId = studentId };
        }
    }
}