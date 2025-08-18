using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.Publish
{
    public sealed record PublishCourseCommand(Guid CourseId) : ICommand;
}