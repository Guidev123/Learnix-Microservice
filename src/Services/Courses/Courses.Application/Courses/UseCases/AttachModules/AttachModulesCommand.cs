using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.AttachModules
{
    public sealed record AttachModulesCommand : ICommand
    {
        public AttachModulesCommand(List<ModuleRequest> modules)
        {
            Modules = modules;
        }

        public List<ModuleRequest> Modules { get; init; }
        public Guid CourseId { get; private set; }

        public AttachModulesCommand SetCourseId(Guid courseId)
        {
            CourseId = courseId;
            return this;
        }

        public sealed record ModuleRequest(
            string Title
        );
    }
}