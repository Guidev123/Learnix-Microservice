using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Features.GetCourseContent
{
    public sealed record GetCourseContentQuery(Guid CourseId, Guid StudentId) : IQuery<CourseContentResponse>;
}