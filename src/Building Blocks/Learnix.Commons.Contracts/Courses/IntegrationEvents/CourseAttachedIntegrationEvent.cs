using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Contracts.Courses.IntegrationEvents
{
    public sealed record CourseAttachedIntegrationEvent : IntegrationEvent
    {
        public CourseAttachedIntegrationEvent(
            Guid correlationId,
            DateTime occurredOn,
            Guid courseId,
            string title,
            string description,
            string dificultLevel,
            string status,
            List<ModuleResponse> modules)
        {
            CorrelationId = correlationId;
            CourseId = courseId;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Status = status;
            Modules = modules;
            OccurredOn = occurredOn;
            Messagetype = GetType().Name;
        }

        private CourseAttachedIntegrationEvent()
        { }

        public Guid CourseId { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string DificultLevel { get; init; } = null!;
        public string Status { get; init; } = null!;
        public List<ModuleResponse> Modules { get; init; } = [];

        public sealed record ModuleResponse(
             Guid Id,
             string Title,
             uint OrderIndex,
             Guid CourseId,
             List<LessonResponse> Lessons,
             Guid? NextModuleId,
             Guid? PreviousModuleId
         );

        public sealed record LessonResponse(
            Guid Id,
            string Title,
            string VideoUrl,
            uint OrderIndex,
            Guid ModuleId,
            Guid? NextLessonId,
            Guid? PreviousLessonId
            );
    }
}