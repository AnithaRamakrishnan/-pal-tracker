using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PalTracker;
using Xunit;

namespace PalTrackerTests
{
    [Collection("Integration")]
    public class MySqlTimeEntryRepositoryTest
    {
        private readonly MySqlTimeEntryRepository _repository;

        public MySqlTimeEntryRepositoryTest()
        {
            DbTestSupport.ExecuteSql("TRUNCATE TABLE time_entries");

            var builder = new DbContextOptionsBuilder<TimeEntryContext>()
                .UseMySql(DbTestSupport.TestDbConnectionString);

            _repository = new MySqlTimeEntryRepository(new TimeEntryContext(builder.Options));
        }

        [Fact]
        public void CreateInsertsATimeEntryRecord()
        {
            var newTimeEntry = new TimeEntry(123, 456, DateTime.Parse("2012-01-02"), 12);

            var createdTimeEntryId = _repository.Create(newTimeEntry).Id.Value;

            var foundInDb = FindInDb(createdTimeEntryId);

            Assert.Equal(createdTimeEntryId, foundInDb[0]["id"]);
            Assert.Equal(newTimeEntry.projectId, foundInDb[0]["project_id"]);
            Assert.Equal(newTimeEntry.userId, foundInDb[0]["user_id"]);
            Assert.Equal(newTimeEntry.date, foundInDb[0]["date"]);
            Assert.Equal(newTimeEntry.hours, foundInDb[0]["hours"]);

            Assert.Equal(1, foundInDb.Count);
        }

        [Fact]
        public void CreateReturnsTheCreatedTimeEntry()
        {
            var newTimeEntry = new TimeEntry(123, 456, DateTime.Parse("2012-01-02"), 12);

            var createdTimeEntry = _repository.Create(newTimeEntry);

            Assert.NotNull(createdTimeEntry.Id);
            Assert.Equal(newTimeEntry.projectId, createdTimeEntry.projectId);
            Assert.Equal(newTimeEntry.userId, createdTimeEntry.userId);
            Assert.Equal(newTimeEntry.date, createdTimeEntry.date);
            Assert.Equal(newTimeEntry.hours, createdTimeEntry.hours);
        }

        [Fact]
        public void FindFindsATimeEntry()
        {
            CreateInDb(new TimeEntry(1, 123, 456, DateTime.Parse("2012-01-02"), 12));

            var timeEntry = _repository.Find(1);

            Assert.Equal(1L, timeEntry.Id);
            Assert.Equal(123L, timeEntry.projectId);
            Assert.Equal(456L, timeEntry.userId);
            Assert.Equal(DateTime.Parse("2012-01-02"), timeEntry.date);
            Assert.Equal(12, timeEntry.hours);
        }

        [Fact]
        public void ListFindsAllTimeEntries()
        {
            var expected = new List<TimeEntry>
            {
                new TimeEntry(1, 111, 222, DateTime.Parse("2017-12-09"), 2),
                new TimeEntry(2, 333, 444, DateTime.Parse("2012-01-02"), 12),
                new TimeEntry(3, 555, 666, DateTime.Parse("1998-11-24"), 1)
            };

            expected.ForEach(CreateInDb);

            var timeEntries = _repository.List();

            expected.ForEach(e => Assert.Contains(e, timeEntries));

            Assert.Equal(expected.Count, timeEntries.Count());
        }

        [Fact]
        public void ContainsTrueWhenPresent()
        {
            CreateInDb(new TimeEntry(1, 123, 456, DateTime.Parse("2012-01-02"), 12));

            Assert.True(_repository.Contains(1));
        }

        [Fact]
        public void ContainsFalseWhenAbsent()
        {
            CreateInDb(new TimeEntry(1, 123, 456, DateTime.Parse("2012-01-02"), 12));

            Assert.False(_repository.Contains(2));
        }

        [Fact]
        public void UpdateReturnsTheUpdatedRecord()
        {
            CreateInDb(new TimeEntry(1, 123, 456, DateTime.Parse("2012-01-02"), 12));

            var update = new TimeEntry(888, 999, DateTime.Parse("2014-07-29"), 1);

            var updatedTimeEntry = _repository.Update(1, update);

            Assert.Equal(1L, updatedTimeEntry.Id);
            Assert.Equal(update.projectId, updatedTimeEntry.projectId);
            Assert.Equal(update.userId, updatedTimeEntry.userId);
            Assert.Equal(update.date, updatedTimeEntry.date);
            Assert.Equal(update.hours, updatedTimeEntry.hours);
        }

        [Fact]
        public void UpdateUpdatesTheRecord()
        {
            CreateInDb(new TimeEntry(1, 123, 456, DateTime.Parse("2012-01-02"), 12));

            var update = new TimeEntry(888, 999, DateTime.Parse("2014-07-29"), 1);

            _repository.Update(1, update);

            var foundInDb = FindInDb(1);

            Assert.Equal(1L, foundInDb[0]["id"]);
            Assert.Equal(update.projectId, foundInDb[0]["project_id"]);
            Assert.Equal(update.userId, foundInDb[0]["user_id"]);
            Assert.Equal(update.date, foundInDb[0]["date"]);
            Assert.Equal(update.hours, foundInDb[0]["hours"]);
            Assert.Equal(1, foundInDb.Count);
        }

        [Fact]
        public void DeleteRemovesTheRecord()
        {
            CreateInDb(new TimeEntry(1, 222, 333, DateTime.Parse("2020-12-14"), 24));
            CreateInDb(new TimeEntry(2, 444, 555, DateTime.Parse("2012-01-02"), 13));

            _repository.Delete(1);

            var foundInDb = FindAllInDb();

            Assert.Equal(1, foundInDb.Count);
            Assert.Equal(2L, foundInDb[0]["id"]);
            Assert.Equal(444L, foundInDb[0]["project_id"]);
            Assert.Equal(555L, foundInDb[0]["user_id"]);
            Assert.Equal(DateTime.Parse("2012-01-02"), foundInDb[0]["date"]);
            Assert.Equal(13, foundInDb[0]["hours"]);
        }

        private static void CreateInDb(TimeEntry timeEntry)
        {
            var sql = $@"INSERT INTO time_entries(id, project_id, user_id, date, hours)
                         VALUES('{timeEntry.Id}',
                                '{timeEntry.projectId}',
                                '{timeEntry.userId}',
                                '{timeEntry.date:yyyy-MM-dd}',
                                '{timeEntry.hours}')";

            DbTestSupport.ExecuteSql(sql);
        }

        private static IList<IDictionary<string, object>> FindInDb(long id) => DbTestSupport.ExecuteSql(
            $@"SELECT id, project_id, user_id, date, hours 
               FROM time_entries 
               WHERE id = {id}"
        );

        private static IList<IDictionary<string, object>> FindAllInDb() => DbTestSupport.ExecuteSql(
            "SELECT id, project_id, user_id, date, hours FROM time_entries"
        );
    }
}