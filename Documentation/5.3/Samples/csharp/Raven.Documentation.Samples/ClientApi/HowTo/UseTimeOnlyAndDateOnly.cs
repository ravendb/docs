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
        public class StringAsDateOnlyConversion : AbstractIndexCreationTask<StringItem, DateOnlyItem>
        {
            public StringAsDateOnlyConversion()
            {
                Map = items => from item in items
                               select new DateOnlyItem { DateOnlyField = AsDateOnly(item.StringDateOnlyField) };
            }
        }

        public class StringItem
        {
            public string StringDateOnlyField { get; set; }
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
                session.Store(new StringItem()
                {
                    StringDateOnlyField = "2022-05-12"
                });
                session.SaveChanges();
            }
            new StringAsDateOnlyConversion().Execute(store);
            WaitForIndexing(store);

            using (var session = store.OpenSession())
            {
                var today = new DateOnly(2022, 5, 12);
                var element = session.Query<DateOnlyItem, StringAsDateOnlyConversion>()
                    .Where(item => item.DateOnlyField == today).As<StringItem>().Single();
            }
            #endregion
        }

        #region IndexConvertsDateTimeWithAsDateOnlySample
        public class DateTimeAsDateOnlyConversion : AbstractIndexCreationTask<DateTimeItem, DateOnlyItem>
        {
            public DateTimeAsDateOnlyConversion()
            {
                Map = items => from item in items
                               select new DateOnlyItem { DateOnlyField = AsDateOnly(item.DateTimeField) };
            }
        }

        public class DateTimeItem
        {
            public DateTime? DateTimeField { get; set; }
        }
        #endregion



        public void SampleUsingDateTimeAsDateOnlyConversion()
        {
            using var store = new DocumentStore();
            #region AsDateOnlyStringToDateOnlyQuerySample
            using (var session = store.OpenSession()) 
        {
            session.Store(new DateTimeItem()
            {
                DateTimeField = DateTime.Now
            });
            session.SaveChanges();
        }
        new DateTimeAsDateOnlyConversion().Execute(store);
        WaitForIndexing(store);

        using (var session = store.OpenSession())
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var element = session.Query<DateOnlyItem, DateTimeAsDateOnlyConversion>()
                .Where(item => item.DateOnlyField == today).As<DateTimeItem>().Single();
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
                               select new IndexEntry() { DateOnly = date.DateOnly, TimeOnly = date.TimeOnly };
            }

        }
        #endregion
    }
}
