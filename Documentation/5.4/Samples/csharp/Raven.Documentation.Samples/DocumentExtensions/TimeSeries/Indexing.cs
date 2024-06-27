using System;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Client.Documents.Operations.Indexes;
using System.Collections.Generic;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries
{
    class TimeSeriesIndexes
    {
        #region index_1
        public class StockPriceTimeSeriesFromCompanyCollection : AbstractTimeSeriesIndexCreationTask<Company>
        {
            // The index-entry:
            // ================
            public class IndexEntry
            {
                // The index-fields:
                // =================
                public double TradeVolume { get; set; }
                public DateTime Date { get; set; }
                public string CompanyID { get; set; }
                public string EmployeeName { get; set; }
            }
            
            public StockPriceTimeSeriesFromCompanyCollection()
            { 
                // Call 'AddMap', specify the time series name to be indexed 
                AddMap("StockPrices", timeseries =>
                        from segment in timeseries
                        from entry in segment.Entries
                        
                        // Can load the document referenced in the TAG:
                        let employee = LoadDocument<Employee>(entry.Tag)
                        
                        // Define the content of the index-fields:
                        // =======================================
                        select new IndexEntry()
                        {
                            // Retrieve content from the time series ENTRY:
                            TradeVolume = entry.Values[4],
                            Date = entry.Timestamp.Date,
                            
                            // Retrieve content from the SEGMENT:
                            CompanyID = segment.DocumentId,
                            
                            // Retrieve content from the loaded DOCUMENT:
                            EmployeeName = employee.FirstName + " " + employee.LastName 
                        });
            }
        }
        #endregion

        #region index_2
        public class StockPriceTimeSeriesFromCompanyCollection_JS : 
            AbstractJavaScriptTimeSeriesIndexCreationTask
        {
            public StockPriceTimeSeriesFromCompanyCollection_JS()
            {
                Maps = new HashSet<string> { @"
                    timeSeries.map('Companies', 'StockPrices', function (segment) {

                        return segment.Entries.map(entry => {
                            let employee = load(entry.Tag, 'Employees');

                            return {
                                TradeVolume: entry.Values[4],
                                Date: new Date(entry.Timestamp.getFullYear(),
                                               entry.Timestamp.getMonth(),
                                               entry.Timestamp.getDate()),
                                CompanyID: segment.DocumentId,
                                EmployeeName: employee.FirstName + ' ' + employee.LastName
                            };
                        });
                    })"
                };
            }
        }
        #endregion
        
        #region index_3
        public class StockPriceTimeSeriesFromCompanyCollection_NonTyped : AbstractTimeSeriesIndexCreationTask 
        {
            public override TimeSeriesIndexDefinition CreateIndexDefinition()
            {
                return new TimeSeriesIndexDefinition
                {
                    Name = "StockPriceTimeSeriesFromCompanyCollection_NonTyped",
                    Maps =
                    {
                        @"
                        from segment in timeSeries.Companies.StockPrices 
                        from entry in segment.Entries
  
                        let employee = LoadDocument(entry.Tag, ""Employees"")

                        select new 
                        { 
                            TradeVolume = entry.Values[4], 
                            Date = entry.Timestamp.Date,
                            CompanyID = segment.DocumentId,
                            EmployeeName = employee.FirstName + ' ' + employee.LastName 
                        }"
                    }
                };
            }
        }
        #endregion
        
        #region index_4
        public class AllTimeSeriesFromCompanyCollection : AbstractTimeSeriesIndexCreationTask<Company>
        {
            public class IndexEntry
            {
                public double Value { get; set; }
                public DateTime Date { get; set; }
            }
            
            public AllTimeSeriesFromCompanyCollection()
            {
                // Call 'AddMapForAll' to index ALL the time series in the 'Companies' collection 
                // ==============================================================================
                AddMapForAll(timeseries =>
                    from segment in timeseries
                    from entry in segment.Entries
                        
                    select new IndexEntry()
                    {
                        Value = entry.Value,
                        Date = entry.Timestamp.Date
                    });
            }
        }
        #endregion
        
        #region index_5
        // Inherit from AbstractTimeSeriesIndexCreationTask<object>
        // Specify <object> as the type to index from ALL collections
        // ==========================================================
        
        public class AllTimeSeriesFromAllCollections : AbstractTimeSeriesIndexCreationTask<object>
        {
            public class IndexEntry
            {
                public double Value { get; set; }
                public DateTime Date { get; set; }
                public string DocumentID { get; set; }
            }
            
            public AllTimeSeriesFromAllCollections()
            {
                AddMapForAll(timeseries =>
                    from segment in timeseries
                    from entry in segment.Entries
                        
                    select new IndexEntry()
                    {
                        Value = entry.Value,
                        Date = entry.Timestamp.Date,
                        DocumentID = segment.DocumentId
                    });
            }
        }
        #endregion

        #region index_6
        public class Vehicles_ByLocation : AbstractMultiMapTimeSeriesIndexCreationTask
        {
            public class IndexEntry
            {
                public double Latitude { get; set; }
                public double Longitude { get; set; }
                public DateTime Date { get; set; }
                public string DocumentID { get; set; }
            }
            
            public Vehicles_ByLocation()
            {
                // Call 'AddMap' for each collection you wish to index
                // ===================================================
                
                AddMap<Plane>(
                    "GPS_Coordinates",timeSeries =>
                        from segment in timeSeries
                        from entry in segment.Entries
                        select new IndexEntry()
                        {
                            Latitude = entry.Values[0],
                            Longitude = entry.Values[1],
                            Date = entry.Timestamp.Date,
                            DocumentID = segment.DocumentId
                        });

                AddMap<Ship>(
                    "GPS_Coordinates",timeSeries =>
                        from segment in timeSeries
                        from entry in segment.Entries
                        select new IndexEntry()
                        {
                            Latitude = entry.Values[0],
                            Longitude = entry.Values[1],
                            Date = entry.Timestamp.Date,
                            DocumentID = segment.DocumentId
                        });
            }
        }
        #endregion

        #region index_7
        public class TradeVolume_PerDay_ByCountry : 
            AbstractTimeSeriesIndexCreationTask<Company, TradeVolume_PerDay_ByCountry.Result>
        {
            public class Result
            {
                public double TotalTradeVolume { get; set; }
                public DateTime Date { get; set; }
                public string Country { get; set; }
            }

            public TradeVolume_PerDay_ByCountry()
            {
                // Define the Map part:
                AddMap("StockPrices", timeSeries =>
                    from segment in timeSeries
                    from entry in segment.Entries
                    
                    let company = LoadDocument<Company>(segment.DocumentId)
                    
                    select new Result
                    {
                        Date = entry.Timestamp.Date,
                        Country = company.Address.Country,
                        TotalTradeVolume = entry.Values[4]
                    });

                // Define the Reduce part:
                Reduce = results =>
                    from r in results
                    group r by new {r.Date, r.Country}
                    into g
                    select new Result
                    {
                        Date = g.Key.Date,
                        Country = g.Key.Country,
                        TotalTradeVolume = g.Sum(x => x.TotalTradeVolume)
                    };
            }
        }
        #endregion
        
        public void IndexDefinitionExamples()
        {
            var documentStore = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "products"
            };
            documentStore.Initialize();

            #region index_definition_1
            // Define the 'index definition'
            var indexDefinition = new TimeSeriesIndexDefinition
                {
                    Name = "StockPriceTimeSeriesFromCompanyCollection ",
                    Maps =
                    {
                        @"
                        from segment in timeSeries.Companies.StockPrices 
                        from entry in segment.Entries 

                        let employee = LoadDocument(entry.Tag, ""Employees"")

                        select new 
                        { 
                            TradeVolume = entry.Values[4], 
                            Date = entry.Timestamp.Date,
                            CompanyID = segment.DocumentId,
                            EmployeeName = employee.FirstName + ' ' + employee.LastName 
                        }"
                    }
                };
            
            // Deploy the index to the server via 'PutIndexesOperation'
            documentStore.Maintenance.Send(new PutIndexesOperation(indexDefinition));
            #endregion

            #region index_definition_2
            // Create the index builder
            var TSIndexDefBuilder =
                new TimeSeriesIndexDefinitionBuilder<Company>("StockPriceTimeSeriesFromCompanyCollection ");
            
            TSIndexDefBuilder.AddMap("StockPrices", timeseries => 
                from segment in timeseries
                from entry in segment.Entries
                
                // Note:
                // Class TimeSeriesIndexDefinitionBuilder does not support the 'LoadDocument' API method.
                // Use one of the other index creation methods if needed.
                
                select new
                {
                    TradeVolume = entry.Values[4],
                    Date = entry.Timestamp.Date,
                    ComapnyID = segment.DocumentId
                });

            // Build the index definition
            var indexDefinitionFromBuilder = TSIndexDefBuilder.ToIndexDefinition(documentStore.Conventions);
            
            // Deploy the index to the server via 'PutIndexesOperation'
            documentStore.Maintenance.Send(new PutIndexesOperation(indexDefinitionFromBuilder));
            #endregion
            
            #region query_1
            using (var session = documentStore.OpenSession())
            {
                // Retrieve time series data for the specified company:
                // ====================================================
                List<StockPriceTimeSeriesFromCompanyCollection.IndexEntry> results = session
                   .Query<StockPriceTimeSeriesFromCompanyCollection.IndexEntry,
                       StockPriceTimeSeriesFromCompanyCollection>()
                   .Where(x => x.CompanyID == "Companies/91-A")
                   .ToList();
            }
            
            // Results will include data from all 'StockPrices' entries in document 'Companies/91-A'. 
            #endregion
            
            #region query_2
            using (var session = documentStore.OpenSession())
            {
                // Find what companies had a very high trade volume:
                // ==================================================
                List<string> results = session
                    .Query<StockPriceTimeSeriesFromCompanyCollection.IndexEntry,
                        StockPriceTimeSeriesFromCompanyCollection>()
                    .Where(x => x.TradeVolume >  150_000_000)
                    .Select(x => x.CompanyID)
                    .Distinct()
                    .ToList();
            }
            
            // Results will contain company "Companies/65-A"
            // since it is the only company with time series entries having such high trade volume.
            #endregion
        }
    }

    internal class Ship
    {
    }

    internal class Plane
    {
    }

    internal class User
    {
    }
}
