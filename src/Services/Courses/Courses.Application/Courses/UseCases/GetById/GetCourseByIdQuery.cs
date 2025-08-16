using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.UseCases.GetById
{
    public sealed record GetCourseByIdQuery(Guid CourseId) : IQuery<GetCourseByIdResponse>;
}