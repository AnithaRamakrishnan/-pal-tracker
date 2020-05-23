using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PalTracker
{
    public class MySqlTimeEntryRepository : ITimeEntryRepository
    {
        private readonly TimeEntryContext _timeEntryContext;
        public MySqlTimeEntryRepository(TimeEntryContext timeEntryContext)
        {
            _timeEntryContext = timeEntryContext;
        }

        public bool Contains(long id) =>
            _timeEntryContext.TimeEntryRecords.AsNoTracking().Any(t => t.Id == id);

        public TimeEntry Create(TimeEntry timeEntry)
        {
            var recordToCreate = timeEntry.ToRecord();
            var s = _timeEntryContext.TimeEntryRecords.Add(recordToCreate);
            _timeEntryContext.SaveChanges();
            return Find(recordToCreate.Id.Value);
        }

        public void Delete(long id)
        {
            _timeEntryContext.TimeEntryRecords.Remove(FindRecord(id));
            _timeEntryContext.SaveChanges();
        }

        public TimeEntry Find(long id) => FindRecord(id).ToEntity();

        public IEnumerable<TimeEntry> List() =>
            _timeEntryContext.TimeEntryRecords.AsNoTracking().Select(t => t.ToEntity());


        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            var recordToUpdate = timeEntry.ToRecord();
            recordToUpdate.Id = id;

            _timeEntryContext.Update(recordToUpdate);
            _timeEntryContext.SaveChanges();

            return Find(id);
        }
        private TimeEntryRecord FindRecord(long id) =>
           _timeEntryContext.TimeEntryRecords.AsNoTracking().Single(t => t.Id == id);
    }
}
