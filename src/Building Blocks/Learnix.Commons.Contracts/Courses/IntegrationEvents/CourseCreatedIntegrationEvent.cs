using Learnix.Commons.Application.Messaging;

namespace Learnix.Commons.Contracts.Courses.IntegrationEvents
{
    public sealed record CourseCreatedIntegrationEvent : IntegrationEvent
    {
        public CourseCreatedIntegrationEvent(
            Guid correlationId,
            DateTime occurredOn,
            Guid courseId,
            string title,
            string description,
            string dificultLevel,
            List<ModuleResponse> modules)
        {
            CorrelationId = correlationId;
            CourseId = courseId;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Modules = modules;
            OccurredOn = occurredOn;
            Messagetype = GetType().Name;
        }

        private CourseCreatedIntegrationEvent()
        { }

        public Guid CourseId { get; init; }
        public string Title { get; init; } = null!;
        public string Description { get; init; } = null!;
        public string DificultLevel { get; init; } = null!;
        public List<ModuleResponse> Modules { get; init; } = [];

        public sealed record ModuleResponse(
             Guid Id,
             string Title,
             uint OrderIndex,
             List<LessonResponse> Lessons,
             Guid? NextModuleId,
             Guid? PreviousModuleId
         );

        public sealed record LessonResponse(
            Guid Id,
            string Title,
            string VideoUrl,
            uint OrderIndex,
            Guid? NextLessonId,
            Guid? PreviousLessonId
            );
    }
}