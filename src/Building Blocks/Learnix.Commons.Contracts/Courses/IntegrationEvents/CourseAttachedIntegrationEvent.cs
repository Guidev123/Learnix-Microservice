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
            List<ModuleResponse> modules,
            List<LessonResponse> lessons)
        {
            CorrelationId = correlationId;
            CourseId = courseId;
            Title = title;
            Description = description;
            DificultLevel = dificultLevel;
            Status = status;
            Modules = modules;
            Lessons = lessons;
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
        public List<LessonResponse> Lessons { get; init; } = [];

        public sealed record ModuleResponse
        {
            public ModuleResponse(Guid id, string title, uint orderIndex, Guid courseId, Guid? nextModuleId, Guid? previousModuleId)
            {
                Id = id;
                Title = title;
                OrderIndex = orderIndex;
                CourseId = courseId;
                NextModuleId = nextModuleId;
                PreviousModuleId = previousModuleId;
            }

            private ModuleResponse()
            { }

            public Guid Id { get; init; }
            public string Title { get; init; } = string.Empty;
            public uint OrderIndex { get; init; }
            public Guid CourseId { get; init; }
            public Guid? NextModuleId { get; init; }
            public Guid? PreviousModuleId { get; init; }
        }

        public sealed record LessonResponse
        {
            public LessonResponse(Guid id, string title, string videoUrl, uint orderIndex, Guid moduleId, Guid? nextLessonId, Guid? previousLessonId)
            {
                Id = id;
                Title = title;
                VideoUrl = videoUrl;
                OrderIndex = orderIndex;
                ModuleId = moduleId;
                NextLessonId = nextLessonId;
                PreviousLessonId = previousLessonId;
            }
            private LessonResponse()
            { }
            public Guid Id { get; init; }
            public string Title { get; init; } = string.Empty;
            public string VideoUrl { get; init; } = string.Empty;
            public uint OrderIndex { get; init; }
            public Guid ModuleId { get; init; }
            public Guid? NextLessonId { get; init; }
            public Guid? PreviousLessonId { get; init; }
        }
    }
}