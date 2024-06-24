using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    #region the_index
    // The index definition:
    
    public class Employees_ByName : AbstractIndexCreationTask<Employee, Employees_ByName.IndexEntry>
    {
        // The IndexEntry class defines the index-fields
        public class IndexEntry
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
            
        public Employees_ByName()
        {
            // The 'Map' function defines the content of the INDEX-fields
            Map = employees => from employee in employees
                select new IndexEntry
                {
                    // * The content of INDEX-fields 'FirstName' & 'LastName'
                    //   is composed of the relevant DOCUMENT-fields.
                    FirstName = employee.FirstName,
                    LastName = employee.LastName
                    
                    // * The index-fields can be queried on to fetch matching documents. 
                    //   You can query and filter Employee documents based on their first or last names.
                    
                    // * Employee documents that do Not contain both 'FirstName' and 'LastName' fields
                    //   will Not be indexed.
                    
                    // * Note: the INDEX-field name does Not have to be exactly the same
                    //   as the DOCUMENT-field name. 
                };
        }
    }
    #endregion

    public class QueryIndex
    {
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region index_query_1_1
                    // Query the 'Employees' collection using the index - without filtering
                    // (Open the 'Index' tab to view the index class definition)
                    
                    List<Employee> employees = session
                         // Pass the queried collection as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .Query<Employee, Employees_ByName>()
                         // Execute the query
                        .ToList();

                    // All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_1_2
                    // Query the 'Employees' collection using the index - without filtering

                    List<Employee> employees = await asyncSession
                         // Pass the queried collection as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .Query<Employee, Employees_ByName>()
                         // Execute the query
                        .ToListAsync();
                    
                    // All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region index_query_1_3
                    // Query the 'Employees' collection using the index - without filtering

                    List<Employee> employees = session
                         // Pass the index name as a parameter
                         // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
                        .Query<Employee>("Employees/ByName")
                         // Execute the query
                        .ToList();

                    // All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region index_query_2_1
                    // Query the 'Employees' collection using the index - filter by INDEX-field
                    
                    List<Employee> employees = session
                         // Pass the IndexEntry class as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .Query<Employees_ByName.IndexEntry, Employees_ByName>()
                         // Filter the retrieved documents by some predicate on an INDEX-field
                        .Where(x => x.LastName == "King")
                         // Specify the type of the returned document entities
                        .OfType<Employee>()
                         // Execute the query
                        .ToList();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_2_2
                    // Query the 'Employees' collection using the index - filter by INDEX-field

                    List<Employee> employees = await asyncSession
                         // Pass the IndexEntry class as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .Query<Employees_ByName.IndexEntry, Employees_ByName>()
                         // Filter the retrieved documents by some predicate on an INDEX-field
                        .Where(x => x.LastName == "King")
                         // Specify the type of the returned document entities
                        .OfType<Employee>()
                         // Execute the query
                        .ToListAsync();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region index_query_3_1
                    // Query the 'Employees' collection using the index - page results
                    
                    // This example is based on the previous filtering example
                    List<Employee> employees = session
                        .Query<Employees_ByName.IndexEntry, Employees_ByName>()
                        .Where(x => x.LastName == "King")
                        .Skip(5)  // Skip first 5 results
                        .Take(10) // Retrieve up to 10 documents
                        .OfType<Employee>()
                        .ToList();

                    // Results will include up to 10 matching documents
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_3_2
                    // Query the 'Employees' collection using the index - page results
                    
                    // This example is based on the previous filtering example
                    List<Employee> employees = await asyncSession
                        .Query<Employees_ByName.IndexEntry, Employees_ByName>()
                        .Where(x => x.LastName == "King")
                        .Skip(5)  // Skip first 5 results
                        .Take(10) // Retrieve up to 10 documents
                        .OfType<Employee>()
                        .ToListAsync();

                    // Results will include up to 10 matching documents
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region index_query_4_1
                    // Query the 'Employees' collection using the index - filter by INDEX-field
                    
                    List<Employee> employees = session.Advanced
                         // Pass the IndexEntry class as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .DocumentQuery<Employees_ByName.IndexEntry, Employees_ByName>()
                         // Filter the retrieved documents by some predicate on an INDEX-field
                        .WhereEquals(x => x.LastName, "King")
                         // Specify the type of the returned document entities
                        .OfType<Employee>()
                         // Execute the query
                        .ToList();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_4_2
                    // Query the 'Employees' collection using the index - filter by INDEX-field
                    
                    List<Employee> employees = await asyncSession.Advanced
                         // Pass the IndexEntry class as the first generic parameter
                         // Pass the index class as the second generic parameter
                        .AsyncDocumentQuery<Employees_ByName.IndexEntry, Employees_ByName>()
                         // Filter the retrieved documents by some predicate on an INDEX-field
                        .WhereEquals(x => x.LastName, "King")
                         // Specify the type of the returned document entities
                        .OfType<Employee>()
                         // Execute the query
                        .ToListAsync();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region index_query_4_3
                    // Query the 'Employees' collection using the index - filter by INDEX-field
                    
                    List<Employee> employees = session.Advanced
                         // Pass the IndexEntry class as the generic param
                         // Pass the index name as the param
                         // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
                        .DocumentQuery<Employees_ByName.IndexEntry>("Employees/ByName")
                         // Filter the retrieved documents by some predicate on an INDEX-field
                        .WhereEquals(x => x.LastName, "King")
                         // Specify the type of the returned document entities
                        .OfType<Employee>()
                         // Execute the query
                        .ToList();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_4_4
                    // Query the 'Employees' collection using the index - filter by INDEX-field
                    
                    List<Employee> employees = await asyncSession.Advanced
                        // Pass the IndexEntry class as the generic parameter
                        // Pass the index name as the parameter
                        // Use slash `/` in the index name, replacing the underscore `_` from the index class definition
                        .AsyncDocumentQuery<Employees_ByName.IndexEntry>("Employees/ByName")
                        // Filter the retrieved documents by some predicate on an INDEX-field
                        .WhereEquals(x => x.LastName, "King")
                        // Specify the type of the returned document entities
                        .OfType<Employee>()
                        // Execute the query
                        .ToListAsync();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region index_query_5_1
                    // Query with RawQuery - filter by INDEX-field

                    List<Employee> employees = session.Advanced
                         // Provide RQL to RawQuery
                        .RawQuery<Employee>("from index 'Employees/ByName' where LastName == 'King'")
                         // Execute the query
                        .ToList();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region index_query_5_2
                    // Query with RawQuery - filter by INDEX-field

                    List<Employee> employees = await asyncSession.Advanced
                         // Provide RQL to RawQuery
                        .AsyncRawQuery<Employee>("from index 'Employees/ByName' where LastName == 'King'")
                         // Execute the query
                        .ToListAsync();

                    // Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
                    #endregion
                }
            }
        }
    }
}
