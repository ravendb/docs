using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Searching
    {
        #region index_1
        public class Employees_ByNotes :
            AbstractIndexCreationTask<Employee, Employees_ByNotes.IndexEntry>
        {
            // The IndexEntry class defines the index-fields
            public class IndexEntry
            {
                public string EmployeeNotes { get; set; }
            }

            public Employees_ByNotes()
            {
                // The 'Map' function defines the content of the index-fields
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeNotes = employee.Notes[0]
                    };

                // Configure the index-field for FTS:
                // Set 'FieldIndexing.Search' on index-field 'EmployeeNotes'
                Index(x => x.EmployeeNotes, FieldIndexing.Search);
                
                // Optionally: Set your choice of analyzer for the index-field.
                // Here the text from index-field 'EmployeeNotes' will be tokenized by 'WhitespaceAnalyzer'.
                Analyze(x => x.EmployeeNotes, "WhitespaceAnalyzer");

                // Note:
                // If no analyzer is set then the default 'RavenStandardAnalyzer' is used.
            }
        }
        #endregion
        
        #region index_2
        public class Employees_ByEmployeeData : 
            AbstractIndexCreationTask<Employee, Employees_ByEmployeeData.IndexEntry>
        {
            public class IndexEntry
            {
                public object[] EmployeeData { get; set; }
            }

            public Employees_ByEmployeeData()
            {
                Map = employees => from employee in employees
                    select new IndexEntry()
                    {
                        EmployeeData = new object[]
                        {
                            // Multiple document-fields can be indexed
                            // into the single index-field 'EmployeeData' 
                            employee.FirstName,
                            employee.LastName,
                            employee.Title,
                            employee.Notes
                        }
                    };

                // Configure the index-field for FTS:
                // Set 'FieldIndexing.Search' on index-field 'EmployeeData'
                Index(x => x.EmployeeData, FieldIndexing.Search);
                
                // Note:
                // Since no analyzer is set then the default 'RavenStandardAnalyzer' is used.
            }
        }
        #endregion

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region search_1
                    List<Employee> employees = session
                         // Query the index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    //
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_2
                    List<Employee> employees = await asyncSession
                         // Query the index
                        .Query<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    // 
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_3
                    List<Employee> employees = session.Advanced
                         // Query the index
                        .DocumentQuery<Employees_ByNotes.IndexEntry, Employees_ByNotes>()
                         // Call 'Search':
                         // pass the index field that was configured for FTS and the term to search for.
                        .Search(x => x.EmployeeNotes, "French")
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have 'French' in their 'Notes' field.
                    // 
                    // * Search is case-sensitive since field was indexed using the 'WhitespaceAnalyzer'
                    //   which preserves casing.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_4
                    List<Employee> employees = session
                         // Query the static-index
                        .Query<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region search_5
                    List<Employee> employees = await asyncSession
                         // Query the static-index
                        .Query<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .OfType<Employee>()
                        .ToListAsync();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region search_6
                    List<Employee> employees = session.Advanced
                         // Query the static-index
                        .DocumentQuery<Employees_ByEmployeeData.IndexEntry, Employees_ByEmployeeData>()
                        .OpenSubclause()
                         // A logical OR is applied between the following two Search calls:
                        .Search(x => x.EmployeeData, "Manager")
                         // A logical AND is applied between the following two terms: 
                        .Search(x => x.EmployeeData, "French Spanish", @operator: SearchOperator.And)
                        .CloseSubclause()
                        .OfType<Employee>()
                        .ToList();
                    
                    // * Results will contain all Employee documents that have:
                    //   ('Manager' in any of the 4 document-fields that were indexed)
                    //   OR 
                    //   ('French' AND 'Spanish' in any of the 4 document-fields that were indexed)
                    //                                                                                                                                                                                                                                                                                                                  
                    // * Search is case-insensitive since the default analyzer is used
                    #endregion
                }
            }
        }
    }
}
