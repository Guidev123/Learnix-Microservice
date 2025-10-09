using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Features.Publish
{
    public sealed record PublishCourseCommand(Guid CourseId) : ICommand;
}