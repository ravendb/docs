using System;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Client.Documents.Operations.Indexes;
using Sparrow.Extensions;
using Xunit;
using Xunit.Abstractions;
using System.Collections.Generic;


namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Indexing
{
    class indexing
    {
        public void Examples()
        {
            //map multimap mapreduce
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            documentStore.Initialize();

            //
            documentStore.Maintenance.Send(new PutIndexesOperation(
                                           new TimeSeriesIndexDefinition
                                           {
                                               Name = "MyTsIndex",
                                               Maps = {
                                               "from ts in timeSeries" +
                                               "from entry in ts.Entries" +
                                               "select new" +
                                               "{" +
                                               "    HeartBeat = entry.Values[0]," +
                                               "    entry.Timestamp.Date," +
                                               "}"
                                               }
                                           }
            ));
        }
        //


        private class MyTsIndex : AbstractTimeSeriesIndexCreationTask<Company>
        {
            public MyTsIndex()
            {
                AddMap(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                    from entry in ts.Entries
                                    select new
                                    {
                                        HeartBeat = entry.Values[0],
                                        entry.Timestamp.Date,
                                    });
            }
        }

        //strong/query syntax multi map
        private class MyMultiMapTsIndex : AbstractMultiMapTimeSeriesIndexCreationTask
        {
            public MyMultiMapTsIndex()
            {
                AddMap<Company>(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                    from entry in ts.Entries
                                    select entry.Values[0]);

                AddMap<User>(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                    from entry in ts.Entries
                                    select entry.Values[0]);
            }
        }

        //mapreduce
        private class AverageHeartRateDaily_ByDateAndUser : AbstractTimeSeriesIndexCreationTask<User>
        {

            public AverageHeartRateDaily_ByDateAndUser()
            {
                AddMap(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                    from entry in ts.Entries
                                    select new
                                    {
                                        HeartBeat = entry.Value,
                                        User = ts.DocumentId,
                                        Date = entry.Timestamp,
                                        Count = 1
                                    });

                Reduce = results => from r in results
                                    group r by new { r.Date, r.User } into g
                                    let sumHeartBeat = g.Sum(x => x.HeartBeat)
                                    let sumCount = g.Sum(x => x.Count)
                                    select new
                                    {
                                        HeartBeat = sumHeartBeat / sumCount,
                                        Count = sumCount
                                    };
            }
        }
    }

    internal class User
    {
    }

    internal class Company
    {
    }
}
