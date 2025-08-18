using Courses.Application.Courses.Abstractions;
using Courses.Application.Courses.UseCases.GetContent;
using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Courses.Enumerators;
using Courses.Domain.Courses.Errors;
using Dapper;
using Learnix.Commons.Application.Exceptions;
using Learnix.Commons.Application.Factories;
using Learnix.Commons.Application.Messaging;

namespace Courses.Application.Courses.DomainEvents
{
    internal sealed class CourseUpdatedDomainEventHandler(ISqlConnectionFactory sqlConnectionFactory, ICourseContentRepository courseContentRepository) : DomainEventHandler<CourseUpdatedDomainEvent>
    {
        public override async Task ExecuteAsync(CourseUpdatedDomainEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var courseContent = await GetCourseByIdAsync(domainEvent.CourseId, cancellationToken)
                ?? throw new LearnixException(nameof(CourseCreatedDomainEvent), CourseErrors.NotFound(domainEvent.CourseId));

            await courseContentRepository.ReplaceAsync(courseContent, cancellationToken);
        }

        public async Task<CourseContentResponse?> GetCourseByIdAsync(Guid courseId, CancellationToken cancellationToken = default)
        {
            using var connection = sqlConnectionFactory.Create();

            const string sql = """
                WITH ModuleNavigation AS (
                    SELECT
                        m.Id,
                        m.Title,
                        m.CourseId,
                        m.OrderIndex,
                        LAG(m.Id) OVER (PARTITION BY m.CourseId ORDER BY m.OrderIndex) AS PreviousModuleId,
                        LEAD(m.Id) OVER (PARTITION BY m.CourseId ORDER BY m.OrderIndex) AS NextModuleId
                    FROM [courses].[Modules] m
                ),
                LessonNavigation AS (
                    SELECT
                        l.Id,
                        l.Title,
                        l.VideoUrl,
                        l.ModuleId,
                        l.OrderIndex,
                        l.DurationInMinutes,
                        LAG(l.Id) OVER (PARTITION BY l.ModuleId ORDER BY l.OrderIndex) AS PreviousLessonId,
                        LEAD(l.Id) OVER (PARTITION BY l.ModuleId ORDER BY l.OrderIndex) AS NextLessonId
                    FROM [courses].[Lessons] l
                )
                SELECT
                    c.Id AS CourseId,
                    c.Title AS CourseTitle,
                    c.Description AS CourseDescription,
                    c.DificultLevel AS CourseDificultLevel,
                    c.Status AS CourseStatus,
                    mn.Id AS ModuleId,
                    mn.Title AS ModuleTitle,
                    mn.OrderIndex AS ModuleOrderIndex,
                    mn.PreviousModuleId,
                    mn.NextModuleId,
                    ln.Id AS LessonId,
                    ln.Title AS LessonTitle,
                    ln.VideoUrl AS LessonVideoUrl,
                    ln.OrderIndex AS LessonOrderIndex,
                    ln.PreviousLessonId,
                    ln.NextLessonId,
                    ln.DurationInMinutes
                FROM [courses].[Courses] c
                LEFT JOIN ModuleNavigation mn ON c.Id = mn.CourseId
                LEFT JOIN LessonNavigation ln ON mn.Id = ln.ModuleId
                WHERE c.Id = @CourseId
                ORDER BY mn.OrderIndex, ln.OrderIndex
                """;

            var results = await connection.QueryAsync<dynamic>(sql, new { CourseId = courseId });

            if (!results.Any()) return null;

            return GetCourseContentResponse(results);
        }

        private static CourseContentResponse GetCourseContentResponse(IEnumerable<dynamic> results)
        {
            var firstRow = results.First();

            var modules = results
                .Where(x => x.ModuleId != null)
                .GroupBy(x => x.ModuleId)
                .Select(moduleGroup =>
                {
                    var moduleData = moduleGroup.First();
                    var lessons = moduleGroup
                        .Where(x => x.LessonId != null)
                        .Select(lessonData => new LessonResponse(
                            lessonData.LessonId,
                            lessonData.LessonTitle,
                            lessonData.LessonVideoUrl,
                            (uint)lessonData.LessonOrderIndex,
                            lessonData.NextLessonId,
                            lessonData.PreviousLessonId
                        ))
                        .OrderBy(l => l.OrderIndex)
                        .ToList();

                    return new ModuleResponse(
                        moduleData.ModuleId,
                        moduleData.ModuleTitle,
                        (uint)moduleData.ModuleOrderIndex,
                        lessons,
                        moduleData.NextModuleId,
                        moduleData.PreviousModuleId
                    );
                })
                .OrderBy(m => m.OrderIndex)
                .ToList();

            return new CourseContentResponse(
                firstRow.CourseId,
                firstRow.CourseTitle,
                firstRow.CourseDescription,
                Enum.Parse<DificultLevelEnum>(firstRow.CourseDificultLevel),
                Enum.Parse<CourseStatusEnum>(firstRow.CourseStatus),
                modules
            );
        }
    }
}