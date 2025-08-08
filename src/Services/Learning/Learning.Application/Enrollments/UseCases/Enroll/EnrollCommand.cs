using Learnix.Commons.Application.Messaging;

namespace Learning.Application.Enrollments.UseCases.Enroll
{
    public sealed record EnrollCommand : ICommand<EnrollResponse>
    {
        public EnrollCommand(Guid courseId)
        {
            CourseId = courseId;
        }

        public Guid CourseId { get; init; }
        public Guid StudentId { get; private set; }
        public void SetStudentId(Guid studentId)
        {
            StudentId = studentId;
        }
    }
}