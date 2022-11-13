using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToStream
    {
        private interface IFoo
        {
            #region syntax
            IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query, out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);
            #endregion
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region stream_1
                    // Define a query on a collection
                    IRavenQueryable<Employee> query = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert");
                    
                    // Call 'Stream' to execute the query
                    // Optionally, pass an 'out param' for getting the query stats
                    IEnumerator<StreamResult<Employee>> streamResults = 
                        session.Advanced.Stream(query, out StreamQueryStatistics streamQueryStats);

                    // Read from the stream
                    while (streamResults.MoveNext())
                    {
                        // Process the received result
                        StreamResult<Employee> currentResult = streamResults.Current;
                        
                        // Get the document from the result
                        // This entity will Not be tracked by the session
                        Employee employee = currentResult.Document;
                        
                        // The currentResult item also provides the following:
                        var employeeId  = currentResult.Id;
                        var documentMetadata = currentResult.Metadata;
                        var documentChangeVector = currentResult.ChangeVector;

                        // Can get info from the stats, i.e. get number of total results
                        int totalResults = streamQueryStats.TotalResults;
                        // Get the Auto-Index that was used/created with this dynamic query
                        string indexUsed = streamQueryStats.IndexName;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_1_async
                    // Define a query on a collection
                    IRavenQueryable<Employee> query = asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert");
                    
                    // Call 'StreamAsync' to execute the query
                    // Optionally, pass an 'out param' for getting the query stats
                    await using (IAsyncEnumerator<StreamResult<Employee>> streamResults = 
                                 await asyncSession.Advanced.StreamAsync(query, out StreamQueryStatistics streamQueryStats))
                    {
                        // Read from the stream
                        while (await streamResults.MoveNextAsync())
                        {
                            // Process the received result
                            StreamResult<Employee> currentResult = streamResults.Current;
                            
                            // Get the document from the result
                            // This entity will Not be tracked by the session
                            Employee employee = currentResult.Document;
                            
                            // The currentResult item also provides the following:
                            var employeeId  = currentResult.Id;
                            var documentMetadata = currentResult.Metadata;
                            var documentChangeVector = currentResult.ChangeVector;
                            
                            // Can get info from the stats, i.e. get number of total results
                            int totalResults = streamQueryStats.TotalResults;
                            // Get the Auto-Index that was used/created with this dynamic query
                            string indexUsed = streamQueryStats.IndexName;
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stream_2
                    // Define a document query on a collection
                    IDocumentQuery<Employee> query = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert");
                    
                    // Call 'Stream' to execute the query
                    // Optionally, add an out param for getting the query stats
                    IEnumerator<StreamResult<Employee>> streamResults = 
                        session.Advanced.Stream(query, out StreamQueryStatistics streamQueryStats);

                    // Read from the stream
                    while (streamResults.MoveNext())
                    {
                        // Process the received result
                        StreamResult<Employee> currentResult = streamResults.Current;
                        
                        // Get the document from the result
                        // This entity will Not be tracked by the session
                        Employee employee = currentResult.Document;
                            
                        // The currentResult item also provides the following:
                        var employeeId  = currentResult.Id;
                        var documentMetadata = currentResult.Metadata;
                        var documentChangeVector = currentResult.ChangeVector;
                        
                        // Can get info from the stats, i.e. get number of total results
                        int totalResults = streamQueryStats.TotalResults;
                        // Get the Auto-Index that was used/created with this dynamic query
                        string indexUsed = streamQueryStats.IndexName;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_2_async
                    // Define a document query on a collection
                    IAsyncDocumentQuery<Employee> query = asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert");
                    
                    // Call 'StreamAsync' to execute the query
                    // Optionally, add an out param for getting the query stats
                    await using (IAsyncEnumerator<StreamResult<Employee>> streamResults =
                                 await asyncSession.Advanced.StreamAsync(query, out StreamQueryStatistics streamQueryStats))
                    {
                        // Read from the stream
                        while (await streamResults.MoveNextAsync())
                        {
                            // Process the received result
                            StreamResult<Employee> currentResult = streamResults.Current;
                            
                            // Get the document from the result
                            // This entity will Not be tracked by the session
                            Employee employee = currentResult.Document;
                            
                            // The currentResult item also provides the following:
                            var employeeId  = currentResult.Id;
                            var documentMetadata = currentResult.Metadata;
                            var documentChangeVector = currentResult.ChangeVector;
                            
                            // Can get info from the stats, i.e. get number of total results
                            int totalResults = streamQueryStats.TotalResults;
                            // Get the Auto-Index that was used/created with this dynamic query
                            string indexUsed = streamQueryStats.IndexName;
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stream_3
                    // Define a raw query using RQL
                    IRawDocumentQuery<Employee> query = session
                        .Advanced
                        .RawQuery<Employee>("from Employees where FirstName = 'Robert'");

                    // Call 'Stream' to execute the query
                    IEnumerator<StreamResult<Employee>> streamResults = session.Advanced.Stream(query);

                    while (streamResults.MoveNext())
                    {
                        StreamResult<Employee> currentResult = streamResults.Current;
                        Employee employee = streamResults.Current.Document;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_3_async
                    // Define a raw query using RQL
                    IAsyncRawDocumentQuery<Employee> query = asyncSession
                        .Advanced
                        .AsyncRawQuery<Employee>("from Employees where FirstName = 'Robert'");

                    // Call 'StreamAsync' to execute the query
                    await using (IAsyncEnumerator<StreamResult<Employee>> streamResults =
                                 await asyncSession.Advanced.StreamAsync(query))
                    {
                        while (await streamResults.MoveNextAsync())
                        {
                            StreamResult<Employee> currentResult = streamResults.Current;
                            Employee employee = streamResults.Current.Document;
                        }
                    }
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region stream_4
                    // Define a query with projected results
                    // Each query result is not an Emplyee document but an entity of type 'NameProjection'.
                    IRavenQueryable<NameProjection> query = session
                        .Query<Employee>()
                        .ProjectInto<NameProjection>();

                    // Call 'Stream' to execute the query
                    IEnumerator<StreamResult<NameProjection>> streamResults = session.Advanced.Stream(query);

                    while (streamResults.MoveNext())
                    {
                        StreamResult<NameProjection> currentResult = streamResults.Current;
                        NameProjection employeeName = streamResults.Current.Document;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_4_async
                    // Define a query with projected results
                    // Each query result is not an Employee document but an entity of type 'NameProjection'.
                    IRavenQueryable<NameProjection> query = asyncSession
                        .Query<Employee>()
                        .ProjectInto<NameProjection>();

                    // Call 'StreamAsync' to execute the query
                    await using (IAsyncEnumerator<StreamResult<NameProjection>> streamResults =
                                 await asyncSession.Advanced.StreamAsync(query))
                    {
                        while (await streamResults.MoveNextAsync())
                        {
                            StreamResult<NameProjection> currentResult = streamResults.Current;
                            NameProjection employeeName = streamResults.Current.Document;
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stream_5
                    // Define a query on an index
                    IQueryable<Employee> query = session.Query<Employee, Employees_ByFirstName>()
                        .Where(employee => employee.FirstName == "Robert");

                    // Call 'Stream' to execute the query
                    IEnumerator<StreamResult<Employee>> streamResults = session.Advanced.Stream(query);

                    while (streamResults.MoveNext())
                    {
                        StreamResult<Employee> currentResult = streamResults.Current;
                        Employee employee = streamResults.Current.Document;
                    }
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_5_async
                    // Define a query on an index
                    IQueryable<Employee> query = asyncSession.Query<Employee, Employees_ByFirstName>()
                        .Where(employee => employee.FirstName == "Robert");

                    // Call 'StreamAsync' to execute the query
                    await using (IAsyncEnumerator<StreamResult<Employee>> streamResults =
                                 await asyncSession.Advanced.StreamAsync(query))
                    {
                        while (await streamResults.MoveNextAsync())
                        {
                            StreamResult<Employee> currentResult = streamResults.Current;
                            Employee employee = streamResults.Current.Document;
                        }
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stream_6
                    // Define a query with a 'select' clause to project the results.
                    
                    // The related Company & Employee documents are 'loaded',
                    // and returned in the projection together with the Order document itself.
                    
                    // Each query result is not an Order document but an entity of type 'AllDocsProjection'.
                    
                    IRawDocumentQuery<AllDocsProjection> query = session
                        .Advanced
                        .RawQuery<AllDocsProjection>(@"from Orders as o 
                                                       where o.ShipTo.City = 'London'
                                                       load o.Company as c, o.Employee as e
                                                       select {
                                                           Order: o,
                                                           Company: c,
                                                           Employee: e
                                                       }");

                    // Call 'Stream' to execute the query
                    IEnumerator<StreamResult<AllDocsProjection>> streamResults = session.Advanced.Stream(query);

                    while (streamResults.MoveNext())
                    {
                        StreamResult<AllDocsProjection> currentResult = streamResults.Current;
                        AllDocsProjection projection = streamResults.Current.Document;
                        
                        Order theOrderDoc = projection.Order;
                        Company theRelatedCompanyDoc = projection.Company;
                        Employee theRelatedEmployeeDoc = projection.Employee;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_6_async
                    // Define a query with a 'select' clause to project the results.
                    
                    // The related Company & Employee documents are 'loaded',
                    // and returned in the projection together with the Order document itself.
                    
                    // Each query result is not an Order document but an entity of type 'AllDocsProjection'.
                    
                    IAsyncRawDocumentQuery<AllDocsProjection> query = asyncSession
                        .Advanced
                        .AsyncRawQuery<AllDocsProjection>(@"from Orders as o 
                                                       where o.ShipTo.City = 'London'
                                                       load o.Company as c, o.Employee as e
                                                       select {
                                                           Order: o,
                                                           Company: c,
                                                           Employee: e
                                                       }");

                    // Call 'StreamAsync' to execute the query
                    await using (IAsyncEnumerator<StreamResult<AllDocsProjection>> streamResults =
                                 await asyncSession.Advanced.StreamAsync(query))
                    {
                        while (await streamResults.MoveNextAsync())
                        {
                            StreamResult<AllDocsProjection> currentResult = streamResults.Current;
                            AllDocsProjection projection = streamResults.Current.Document;
                            
                            Order theOrderDoc = projection.Order;
                            Company theRelatedCompanyDoc = projection.Company;
                            Employee theRelatedEmployeeDoc = projection.Employee;
                        }
                    }
                    #endregion
                }
            }
        }
    }

    #region stream_5_index
    // The index:
    public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
    {
        public Employees_ByFirstName()
        {
            Map = employees => from employee in employees
                select new
                {
                    FirstName = employee.FirstName
                };
        }
    }
    #endregion

    #region stream_4_class
    // Each query result will be of this class type
    public class NameProjection
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    #endregion
    
    #region stream_6_class
    // Each query result will be of this class type
    public class AllDocsProjection
    {
        public Order Order { get; set; }
        public Employee Employee { get; set; }
        public Company Company { get; set; }
    }
    #endregion
}
