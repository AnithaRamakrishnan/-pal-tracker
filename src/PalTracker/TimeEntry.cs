using System;
using System.Collections.Generic;
using System.Text;

namespace PalTracker
{
    public struct TimeEntry
    {
        public long? Id { get; set; }
        public long projectId { get; set; }
        public long userId { get; set; }
        public DateTime date { get; set; }
        public int hours { get; set; }

        public TimeEntry(long id, long ProjectId, long UserId, DateTime Date, int Hours)
        {
            Id = id;
            projectId = ProjectId;
            userId = UserId;
            date = Date;
            hours = Hours;
        }

        public TimeEntry(long ProjectId, long UserId, DateTime Date, int Hours)
        {
            Id = null;
            projectId = ProjectId;
            userId = UserId;
            date = Date;
            hours = Hours;
        }
    }
}
