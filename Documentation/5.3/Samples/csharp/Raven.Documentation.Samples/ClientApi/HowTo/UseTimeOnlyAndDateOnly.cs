using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;
using System;
using Raven.TestDriver;

namespace Raven.Documentation.Samples.ClientApi.HowTo.DateAndTimeOnlySample
{
    public class DateAndTimeOnlySamples : RavenTestDriver
    {
        #region IndexConvertsStringsWithAsDateOnlySample
        // Create a Static Index.
        public class StringAsDateOnlyConversion : AbstractIndexCreationTask<StringItem, DateOnlyItem>
        {
            public StringAsDateOnlyConversion()
            {
                // This map index converts strings that are in date format to DateOnly with AsDateOnly().
                Map = items => from item in items
                               // RavenDB doesn't look for DateOnly or TimeOnly as default types during indexing
                               // so the variables must by wrapped in AsDateDonly() or AsTimeOnly() explicitly.
                               where AsDateOnly(item.DateTimeValue) < AsDateOnly(item.DateOnlyValue).AddDays(-50)
                                   select new DateOnlyItem { DateOnlyField = AsDateOnly(item.StringDateOnlyField) };
            }
        }

        public class StringItem
        {
            public string StringDateOnlyField { get; set; }
            public object DateTimeValue { get; set; }
            public object DateOnlyValue { get; set; }
        }

        public class DateOnlyItem
        {
            public DateOnly? DateOnlyField { get; set; }
        };
        #endregion

        public void SampleUsingAsDateOnly()
        {
            using var store = new DocumentStore();
            #region AsDateOnlyStringToDateOnlyQuerySample
            using (var session = store.OpenSession())
            {
                // A string in date format is saved.
                session.Store(new StringItem()
                {
                    StringDateOnlyField = "2022-05-12"
                });
                session.SaveChanges();
            }
            // This is the index used earlier.
            new StringAsDateOnlyConversion().Execute(store);
            WaitForIndexing(store);

            using (var session = store.OpenSession())
            {
                var today = new DateOnly(2022, 5, 12);
                // Query the index created earlier for items which were marked with today's date
                var element = session.Query<DateOnlyItem, StringAsDateOnlyConversion>()
                    .Where(item => item.DateOnlyField == today)
                    // This is an optional type relaxation for projections 
                    .As<StringItem>().Single();
            }
            #endregion
        }

        #region IndexConvertsDateTimeWithAsDateOnlySample
        // Create a Static Index.
        public class DateTimeAsDateOnlyConversion : AbstractIndexCreationTask<DateTimeItem, DateOnlyItem>
        {
            public DateTimeAsDateOnlyConversion()
            {
                // This map index converts DateTime to DateOnly with AsDateOnly().
                Map = items => from item in items
                                   // RavenDB doesn't look for DateOnly or TimeOnly as default types during indexing
                                   // so the variables must by wrapped in AsDateDonly() or AsTimeOnly() explicitly.
                                   where AsDateOnly(item.DateTimeValue) < AsDateOnly(item.DateOnlyValue).AddDays(-50)
                                   select new DateOnlyItem { DateOnlyField = AsDateOnly(item.DateTimeField) };
            }
        }

        public class DateTimeItem
        {
            public DateTime? DateTimeField { get; set; }
            public object DateTimeValue { get; set; }
            public object DateOnlyValue { get; set; }
        }
        #endregion



        public void SampleUsingDateTimeAsDateOnlyConversion()
            {
            using var store = new DocumentStore();
            #region AsDateOnlyDateTimeToDateOnlyQuerySample
            using (var session = store.OpenSession()) 
            {
            // A DateTime value is saved
            session.Store(new DateTimeItem()
            {
                DateTimeField = DateTime.Now
            });
            session.SaveChanges();
            }
            // The index above is called and we wait for the index to finish converting
            new DateTimeAsDateOnlyConversion().Execute(store);
            WaitForIndexing(store);

            using (var session = store.OpenSession())
            {
                // Query the index
                var today = DateOnly.FromDateTime(DateTime.Now);
                var element = session.Query<DateOnlyItem, DateTimeAsDateOnlyConversion>()
                    .Where(item => item.DateOnlyField == today)
                    // This is an optional type relaxation for projections 
                    .As<DateTimeItem>().Single();
            }
            #endregion
        }

    public void QuerySampleForIndexWithDateTimeAndDateOnly()
        {
                using (var store = new DocumentStore())
                {
                    using var session = store.OpenSession();
                    #region TimeOnly_between15-17
                    var after = new TimeOnly(15, 00);
                    var before = new TimeOnly(17, 00);
                    var result = session
                    .Query<DateAndTimeOnly>()
                    .Where(i => i.TimeOnly > after && i.TimeOnly < before)
                    .ToList();
                    #endregion
                }
        }

        public class DateAndTimeOnly
        {
            public DateOnly DateOnly { get; set; }
            public TimeOnly TimeOnly { get; set; }
            public DateTime DateTime { get; set; }

            public int? Age { get; set; }
        }

        #region DateAndTimeOnlyIndexSample
        public class DateAndTimeOnlyIndex : AbstractIndexCreationTask<DateAndTimeOnly, DateAndTimeOnlyIndex.IndexEntry>
        {
            public class IndexEntry
            {

                public DateOnly DateOnly { get; set; }
                public int Year { get; set; }
                public DateOnly DateOnlyString { get; set; }
                public TimeOnly TimeOnlyString { get; set; }
                public TimeOnly TimeOnly { get; set; }
            }

            public DateAndTimeOnlyIndex()
            {
                Map = dates => from date in dates
                               // RavenDB doesn't look for DateOnly or TimeOnly as default types during indexing
                               // so they need to be called explicitly.
                               select new IndexEntry() { DateOnly = date.DateOnly, TimeOnly = date.TimeOnly };
            }

        }
        #endregion
    }
}
