using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.GetContent
{
    public sealed record GetCourseContentQuery(Guid CourseId) : IQuery<GetCourseContentResponse>;
}