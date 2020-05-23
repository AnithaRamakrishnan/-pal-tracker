using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalTracker
{
    public class InMemoryTimeEntryRepository : ITimeEntryRepository
    {
        private readonly IDictionary<long, TimeEntry> _timeEntries = new Dictionary<long, TimeEntry>();
       
        public TimeEntry Find(long id)
        {
            return _timeEntries[id];
        }

        public TimeEntry Create(TimeEntry timeEntry)
        {
            var id = _timeEntries.Count + 1;

            timeEntry.Id = id; 
                //System.Security.Principal.WindowsIdentity.GetCurrent().Name;  --userName
            _timeEntries.Add(id, timeEntry);

            return timeEntry; 
        }

        public TimeEntry Update(long id, TimeEntry timeEntry)
        {
            timeEntry.Id = id;
            _timeEntries[id] = timeEntry;
            return timeEntry;
        }

        public void Delete(long id)=> _timeEntries.Remove(id);
        public bool Contains(long id) => _timeEntries.ContainsKey(id);

        public IEnumerable<TimeEntry> List() => _timeEntries.Values.ToList();
    }
}
