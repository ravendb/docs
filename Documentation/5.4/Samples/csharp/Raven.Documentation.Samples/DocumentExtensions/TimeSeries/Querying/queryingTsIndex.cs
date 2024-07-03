using System;
using System.Linq;
using Raven.Client.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Indexes.TimeSeries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.TimeSeries.Querying
{
    public class QueryingTsIndex
    {
        public DocumentStore getDocumentStore()
        {
            DocumentStore store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "TestDB"
            };
            store.Initialize();

            return store;
        }
        
        #region sample_ts_index
        public class TsIndex : AbstractTimeSeriesIndexCreationTask<Employee>
        {
            // The index-entry:
            // ================
            public class IndexEntry
            {
                // The index-fields:
                // =================
                public double BPM { get; set; }
                public DateTime Date { get; set; }
                public string Tag { get; set; }
                public string EmployeeID { get; set; }
                public string EmployeeName { get; set; }
            }

            public TsIndex()
            {
                AddMap("HeartRates", timeSeries => 
                    from segment in timeSeries
                    from entry in segment.Entries
                    
                    let employee = LoadDocument<Employee>(segment.DocumentId)
                    
                    // Define the content of the index-fields:
                    // =======================================
                    select new IndexEntry()
                    {
                        BPM = entry.Values[0],
                        Date = entry.Timestamp,
                        Tag = entry.Tag,
                        EmployeeID = segment.DocumentId,
                        EmployeeName = employee.FirstName + " " + employee.LastName 
                    });
            }
        }
        #endregion
        
        #region employee_details_class
        // This class is used when projecting index-fields via DocumentQuery
        public class EmployeeDetails
        {
            public string EmployeeName { get; set; }
            public string EmployeeID { get; set; }
        }
        #endregion
        
        public async Task IndexQuery()
        {
            using (var store = getDocumentStore())
            {
                #region query_index_1
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session
                         // Query the index
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Query for all entries w/o any filtering
                        .ToList();
                    
                    // Access results:
                    TsIndex.IndexEntry entryResult = results[0];
                    string employeeName = entryResult.EmployeeName;
                    double BPM = entryResult.BPM;
                }
                #endregion
                
                #region query_index_2
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<TsIndex.IndexEntry> results = await asyncSession
                         // Query the index
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Query for all entries w/o any filtering
                        .ToListAsync();
                }
                #endregion
                
                #region query_index_3
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                         // Query the index
                        .DocumentQuery<TsIndex.IndexEntry, TsIndex>()
                         // Query for all entries w/o any filtering
                        .ToList();
                }
                #endregion
                
                #region query_index_4
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                         // Query the index for all entries w/o any filtering
                        .RawQuery<TsIndex.IndexEntry>($@"
                              from index 'TsIndex'
                         ")
                        .ToList();
                }
                #endregion
                
                #region query_index_5
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve only time series entries with high BPM values for a specific employee
                        .Where(x => x.EmployeeName == "Robert King" && x.BPM > 85)
                        .ToList();
                }
                #endregion
                
                #region query_index_6
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<TsIndex.IndexEntry> results = await asyncSession
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve only time series entries with high BPM values for a specific employee
                        .Where(x => x.EmployeeName == "Robert King" && x.BPM > 85)
                        .ToListAsync();
                }
                #endregion
                
                #region query_index_7
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                        .DocumentQuery<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve only time series entries with high BPM values for a specific employee
                        .WhereEquals(x => x.EmployeeName, "Robert King")
                        .AndAlso()
                        .WhereGreaterThan(x => x.BPM, 85)
                        .ToList();
                }
                #endregion
                
                #region query_index_8
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                         // Retrieve only time series entries with high BPM values for a specific employee
                        .RawQuery<TsIndex.IndexEntry>($@"
                              from index 'TsIndex'
                              where EmployeeName == 'Robert King' and BPM > 85.0 
                         ")
                        .ToList();
                }
                #endregion
                
                #region query_index_9
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve time series entries where employees had a low BPM value
                        .Where(x => x.BPM < 58)
                         // Order by the 'Date' index-field (descending order)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                }
                #endregion
                
                #region query_index_10
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<TsIndex.IndexEntry> results = await asyncSession
                        .Query<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve time series entries where employees had a low BPM value
                        .Where(x => x.BPM < 58)
                         // Order by the 'Date' index-field (descending order)
                        .OrderByDescending(x => x.Date)
                        .ToListAsync();
                }
                #endregion
                
                #region query_index_11
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                        .DocumentQuery<TsIndex.IndexEntry, TsIndex>()
                         // Retrieve time series entries where employees had a low BPM value
                        .WhereLessThan(x => x.BPM, 58)
                         // Order by the 'Date' index-field (descending order)
                        .OrderByDescending(x => x.Date)
                        .ToList();
                }
                #endregion
                
                #region query_index_12
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                         // Retrieve entries with low BPM value and order by 'Date' descending
                        .RawQuery<TsIndex.IndexEntry>($@"
                              from index 'TsIndex'
                              where BPM < 58.0
                              order by Date desc
                         ")
                        .ToList();
                }
                #endregion
                
                #region query_index_13
                using (var session = store.OpenSession())
                {
                    List<string> results = session
                        .Query<TsIndex.IndexEntry, TsIndex>()
                        .Where(x => x.BPM > 100)
                         // Return only the EmployeeID index-field in the results
                        .Select(x => x.EmployeeID)
                         // Optionally: call 'Distinct' to remove duplicates from results
                        .Distinct()
                        .ToList();
                }
                #endregion
                
                #region query_index_14
                using (var asyncSession = store.OpenAsyncSession())
                {
                    List<string> results = await asyncSession
                        .Query<TsIndex.IndexEntry, TsIndex>()
                        .Where(x => x.BPM > 100)
                         // Return only the EmployeeID index-field in the results
                        .Select(x => x.EmployeeID)
                         // Optionally: call 'Distinct' to remove duplicates from results
                        .Distinct()
                        .ToListAsync();
                }
                #endregion
                
                #region query_index_15
                var fieldsToProject = new string[] {
                    "EmployeeID"
                };
                
                using (var session = store.OpenSession())
                {
                    List<EmployeeDetails> results = session.Advanced
                        .DocumentQuery<TsIndex.IndexEntry, TsIndex>()
                        .WhereGreaterThan(x => x.BPM, 100)
                         // Return only the EmployeeID index-field in the results
                        .SelectFields<EmployeeDetails>(fieldsToProject)
                         // Optionally: call 'Distinct' to remove duplicates from results
                        .Distinct()
                        .ToList();
                }
                #endregion
                
                #region query_index_16
                using (var session = store.OpenSession())
                {
                    List<TsIndex.IndexEntry> results = session.Advanced
                         // Return only the EmployeeID index-field in the results
                        .RawQuery<TsIndex.IndexEntry>($@"
                              from index 'TsIndex'
                              where BPM > 100.0
                              select distinct EmployeeID
                         ")
                        .ToList();
                }
                #endregion
            }
        }
    }
}

