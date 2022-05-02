using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Indexes;
using Xunit;
using Xunit.Abstractions;

namespace Raven.Documentation.Samples.ClientApi.HowTo.DateAndTimeOnlySample
{
    public class DateAndTimeOnlySamples 
    { 
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

        private class DateAndTimeOnly
        {
            public DateOnly DateOnly { get; set; }
            public TimeOnly TimeOnly { get; set; }
            public DateTime DateTime { get; set; }

            public int? Age { get; set; }
        }

            public DateAndTimeOnlyIndex()
        {
            Map = dates => from date in dates
                           select new IndexEntry() { DateOnly = date.DateOnly, TimeOnly = date.TimeOnly };
        }
    }
    #endregion
    public class querysampleforIndexWithDateTimeAndDateOnly
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
