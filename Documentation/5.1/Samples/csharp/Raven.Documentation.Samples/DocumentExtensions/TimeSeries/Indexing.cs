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
    class Indexing
    {
        public void Examples()
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            documentStore.Initialize();

            #region indexes_IndexDefinition
            documentStore.Maintenance.Send(new PutIndexesOperation(
                                      new TimeSeriesIndexDefinition
                                      {
                                          Name = "Stocks_ByTradeVolume",
                                          Maps = {
                                          "from ts in timeSeries.Companies.StockPrice " +
                                          "from entry in ts.Entries " +
                                          "select new " +
                                          "{ " +
                                          "    TradeVolume = entry.Values[4], " +
                                          "    entry.Timestamp.Date " +
                                          "}"
                                          }
                                      }));
            #endregion

            #region indexes_IndexDefinitionBuilder
            var TSIndexDefBuilder = 
                new TimeSeriesIndexDefinitionBuilder<Company>("Stocks_ByTradeVolume");

            TSIndexDefBuilder.AddMap("StockPrice",
                        timeseries => from ts in timeseries
                                        from entry in ts.Entries
                                        select new
                                        {
                                            TradeVolume = entry.Values[4],
                                            entry.Timestamp.Date
                                        });

            documentStore.Maintenance.Send(new PutIndexesOperation(
                            TSIndexDefBuilder.ToIndexDefinition(documentStore.Conventions)));
            #endregion


        }
        
        #region indexes_CreationTask
        public class Stocks_ByTradeVolume : AbstractTimeSeriesIndexCreationTask<Company>
        {
            public Stocks_ByTradeVolume()
            {
                AddMap("StockPrice",
                        timeseries => from ts in timeseries
                                      from entry in ts.Entries
                                      select new
                                      {
                                          TradeVolume = entry.Values[4],
                                          entry.Timestamp.Date
                                      });
            }
        }
        #endregion

        #region indexes_MultiMapCreationTask
        public class Vehicles_ByLocation : AbstractMultiMapTimeSeriesIndexCreationTask
        {
            public Vehicles_ByLocation()
            {
                AddMap<Plane>(
                    "GPS_Coordinates",
                    timeSeries => from ts in timeSeries
                                  from entry in ts.Entries
                                  select new 
                                  {
                                      Latitude = entry.Values[0],
                                      Longitude = entry.Values[0],
                                      entry.Timestamp
                                  });

                AddMap<Ship>(
                    "GPS_Coordinates",
                    timeSeries => from ts in timeSeries
                                  from entry in ts.Entries
                                  select new
                                  {
                                      Latitude = entry.Values[0],
                                      Longitude = entry.Values[0],
                                      entry.Timestamp
                                  });
            }
        }
        #endregion

        #region indexes_AbstractJavaScriptCreationTask
        public class Company_TradeVolume_ByDate : AbstractJavaScriptTimeSeriesIndexCreationTask
        {
            public Company_TradeVolume_ByDate()
            {
                Maps = new HashSet<string> { @"
                    timeSeries.map('Companies', 'StockPrices', function (ts) {
                        return ts.Entries.map(entry => ({
                                Volume: entry.Values[0],
                                Date: entry.Timestamp,
                                Company: ts.DocumentId
                        }));
                    })"
                };
            }
        }
        #endregion

        #region indexes_MapReduce
        public class TradeVolume_PerDay_ByCountry : 
                     AbstractTimeSeriesIndexCreationTask<Company, TradeVolume_PerDay_ByCountry.Result>
        {
            public class Result
            {
                public double TradeVolume { get; set; }
                public DateTime Date { get; set; }
                public string Country { get; set; }
            }

            public TradeVolume_PerDay_ByCountry()
            {
                AddMap(
                    "StockPrice",
                    timeSeries => from ts in timeSeries
                                  let company = LoadDocument<Company>(ts.DocumentId)
                                  from entry in ts.Entries
                                  select new Result
                                  {
                                      TradeVolume = entry.Values[4],
                                      Date = entry.Timestamp.Date,
                                      Country = company.Address.Country
                                  });

                    Reduce = results => 
                                  from r in results
                                  group r by new { r.Date, r.Country } into g
                                  select new Result
                                  {
                                      TradeVolume = g.Sum(x => x.TradeVolume),
                                      Date = g.Key.Date,
                                      Country = g.Key.Country
                                  };
            }
        }
        #endregion
    }

    internal class Ship
    {
    }

    internal class Plane
    {
    }

    internal class Employee
    {
    }

    internal class User
    {
    }

    internal class Company
    {
        public Address Address { get; set; }
    }

    internal class Address
    {
        public string Country { get; set; }
    }
}
