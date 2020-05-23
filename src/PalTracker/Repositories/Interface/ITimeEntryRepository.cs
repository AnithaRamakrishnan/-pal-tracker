using System;
using System.Collections.Generic;
using System.Text;

namespace PalTracker
{
    public interface ITimeEntryRepository
    {
        TimeEntry Find(long id);
        TimeEntry Create(TimeEntry timeEntry);
        TimeEntry Update(long id, TimeEntry timeEntry);
        void Delete(long id);
        IEnumerable<TimeEntry> List();
        bool Contains(long id);
    }
}
