namespace PalTracker
{
    public static class MappingExtensions
    {
        public static TimeEntry ToEntity(this TimeEntryRecord record) => new TimeEntry
        {
            Id = record.Id,
            projectId = record.ProjectId,
            userId = record.UserId,
            date = record.Date,
            hours = record.Hours
        };

        public static TimeEntryRecord ToRecord(this TimeEntry entity) => new TimeEntryRecord
        {
            Id = entity.Id,
            ProjectId = entity.projectId,
            UserId = entity.userId,
            Date = entity.date,
            Hours = entity.hours
        };
    }
}
