using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Enrollments.UseCases.GetCourseContent
{
    public sealed record GetCourseContentQuery(Guid CourseId) : IQuery<CourseContentResponse>;
}