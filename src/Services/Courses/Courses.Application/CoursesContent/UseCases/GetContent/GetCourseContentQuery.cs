using Learnix.Commons.Application.Messaging;

namespace Courses.Application.CoursesContent.UseCases.GetContent
{
    public sealed record GetCourseContentQuery(Guid CourseId) : IQuery<CourseContentResponse>;
}