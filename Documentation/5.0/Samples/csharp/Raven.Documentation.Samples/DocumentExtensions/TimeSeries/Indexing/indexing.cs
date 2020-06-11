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
            //map multimap mapreduce TimeSeriesNamesFor
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            documentStore.Initialize();

            //
            #region indexes_IndexDefinition
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
            #endregion


            #region indexes_IndexDefinitionBuilder
            var TSIndexDefBuilder = new TimeSeriesIndexDefinitionBuilder<Company>("bob's index");
            TSIndexDefBuilder.AddMap("StockPrice",
                        timeseries => from ts in timeseries
                                      from entry in ts.Entries
                                      select entry.Value);


            #endregion
        }
        //

        #region indexes_CreationTask
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
        #endregion

        //strong/query syntax multi map
        #region indexes_MultiMapCreationTask
        private class MyMultiMapTsIndex : AbstractMultiMapTimeSeriesIndexCreationTask
        {
            public MyMultiMapTsIndex()
            {
                AddMap<Employee>(
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
        #endregion

        #region indexes_MapReduce
        private class MyMapReduceTSIndex : AbstractTimeSeriesIndexCreationTask<Company, MyMapReduceTSIndex.Result>
        {
            public class Result
            {
                public double TradeVolume { get; set; }
                public DateTime Date { get; set; }
                public string CompanyID { get; set; }
            }

            public MyMapReduceTSIndex()
            {
                AddMap(
                    "HeartRate",
                    timeSeries => from ts in timeSeries
                                    from entry in ts.Entries
                                    select new Result
                                    {
                                        TradeVolume = entry.Values[4],
                                        CompanyID = ts.DocumentId,
                                        Date = entry.Timestamp,
                                    });

                Reduce = results => from r in results
                                    group r by new { r.Date, r.CompanyID } into g
                                    select new Result();
            }
        }
        #endregion
    }

    internal class Employee
    {
    }

    internal class User
    {
    }

    internal class Company
    {
    }
}
