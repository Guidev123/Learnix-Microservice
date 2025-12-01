using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.UpdateLessonProgress
{
    public sealed record UpdateLessonProgressCommand : ICommand
    {
        public UpdateLessonProgressCommand(Guid courseId, Guid moduleId, Guid lessonId, uint minutesWatched, uint totalDuration)
        {
            CourseId = courseId;
            ModuleId = moduleId;
            LessonId = lessonId;
            MinutesWatched = minutesWatched;
            TotalDuration = totalDuration;
        }

        public Guid StudentId { get; private set; }
        public Guid CourseId { get; init; }
        public Guid ModuleId { get; init; }
        public Guid LessonId { get; init; }
        public uint MinutesWatched { get; init; }
        public uint TotalDuration { get; init; }

        public UpdateLessonProgressCommand SetStudentId(Guid studentId)
        {
            return this with { StudentId = studentId };
        }
    }
}