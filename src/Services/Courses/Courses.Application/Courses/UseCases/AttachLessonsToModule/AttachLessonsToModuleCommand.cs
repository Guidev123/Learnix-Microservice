using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.AttachLessonsToModule
{
    public sealed record AttachLessonsToModuleCommand : ICommand
    {
        public AttachLessonsToModuleCommand(List<LessonRequest> lessons)
        {
            Lessons = lessons;
        }

        public List<LessonRequest> Lessons { get; private set; }
        public Guid ModuleId { get; private set; }
        public Guid CourseId { get; private set; }

        public AttachLessonsToModuleCommand SetModuleId(Guid moduleId)
        {
            ModuleId = moduleId;
            return this;
        }

        public AttachLessonsToModuleCommand SetCourseId(Guid courseId)
        {
            CourseId = courseId;
            return this;
        }

        public sealed record LessonRequest(
            string Title,
            string VideoUrl,
            string VideoThumbnailUrl,
            int DurationInMinutes);
    }
}