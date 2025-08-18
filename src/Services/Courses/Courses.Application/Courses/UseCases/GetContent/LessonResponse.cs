namespace Courses.Application.Courses.UseCases.GetContent
{
    public sealed class LessonResponse
    {
        public LessonResponse(Guid id, string title, string videoUrl, uint orderIndex, Guid? nextLessonId, Guid? previousLessonId)
        {
            Id = id;
            Title = title;
            VideoUrl = videoUrl;
            OrderIndex = orderIndex;
            NextLessonId = nextLessonId;
            PreviousLessonId = previousLessonId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string VideoUrl { get; set; }
        public uint OrderIndex { get; set; }
        public Guid? NextLessonId { get; set; }
        public Guid? PreviousLessonId { get; set; }
    }
}