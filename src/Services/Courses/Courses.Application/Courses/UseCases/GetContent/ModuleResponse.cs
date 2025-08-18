namespace Courses.Application.Courses.UseCases.GetContent
{
    public sealed class ModuleResponse
    {
        public ModuleResponse(Guid id, string title, uint orderIndex, List<LessonResponse> lessons, Guid? nextModuleId, Guid? previousModuleId)
        {
            Id = id;
            Title = title;
            OrderIndex = orderIndex;
            Lessons = lessons;
            NextModuleId = nextModuleId;
            PreviousModuleId = previousModuleId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public uint OrderIndex { get; set; }
        public List<LessonResponse> Lessons { get; set; }
        public Guid? NextModuleId { get; set; }
        public Guid? PreviousModuleId { get; set; }
    }
}