using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Documents.Indexes.TimeSeries;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Indexing
{
    class indexing
    {
        //method syntax simple map


        //strong/query syntax map
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

        //strong/query syntax mapreduce
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
}
