using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Session;
using Raven.Client.Util;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    public class HowToStream
    {
        private interface IFoo
        {
            #region stream_1
            IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(
                IQueryable<T> query,
                out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(
                IDocumentQuery<T> query,
                out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(
                IRawDocumentQuery<T> query,
                out StreamQueryStatistics streamQueryStats);

            #endregion
        }

        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region stream_2
                    IQueryable<Employee> query = session
                        .Query<Employee, Employees_ByFirstName>()
                        .Where(x => x.FirstName == "Robert");

                    IEnumerator<StreamResult<Employee>> results = session.Advanced.Stream(query);

                    while (results.MoveNext())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_2_async
                    IQueryable<Employee> query = asyncSession
                        .Query<Employee, Employees_ByFirstName>()
                        .Where(x => x.FirstName == "Robert");

                    IAsyncEnumerator<StreamResult<Employee>> results = await asyncSession.Advanced.StreamAsync(query);

                    while (await results.MoveNextAsync())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }


                using (var session = store.OpenSession())
                {
                    #region stream_3
                    IDocumentQuery<Employee> query = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert");

                    StreamQueryStatistics streamQueryStats;
                    IEnumerator<StreamResult<Employee>> results = session.Advanced.Stream(query, out streamQueryStats);

                    while (results.MoveNext())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_3_async
                    IAsyncDocumentQuery<Employee> query = asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereEquals(x => x.FirstName, "Robert");

                    IAsyncEnumerator<StreamResult<Employee>> results = await asyncSession.Advanced.StreamAsync(query);

                    while (await results.MoveNextAsync())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region stream_4
                    IRawDocumentQuery<Employee> query = session
                        .Advanced
                        .RawQuery<Employee>("from Employees where FirstName = 'Robert'");

                    IEnumerator<StreamResult<Employee>> results = session.Advanced.Stream(query);

                    while (results.MoveNext())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region stream_4_async
                    IAsyncRawDocumentQuery<Employee> query = asyncSession
                        .Advanced
                        .AsyncRawQuery<Employee>("from Employees where FirstName = 'Robert'");

                    IAsyncEnumerator<StreamResult<Employee>> results = await asyncSession.Advanced.StreamAsync(query);

                    while (await results.MoveNextAsync())
                    {
                        StreamResult<Employee> employee = results.Current;
                    }
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region includes
                    IRawDocumentQuery<MyProjection> query = session
                        .Advanced
                        .RawQuery<MyProjection>(@"from Orders as o 
                                                where o.ShipTo.City = 'London'
                                                load o.Company as c, o.Employee as e
                                                select {
                                                    order: o,
                                                    company: c,
                                                    employee: e
                                                }");


                    IEnumerator<StreamResult<MyProjection>> results = session.Advanced.Stream(query);

                    while (results.MoveNext())
                    {
                        StreamResult<MyProjection> projection = results.Current;
                    }
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region includes_async
                    IAsyncRawDocumentQuery<MyProjection> query = asyncSession
                        .Advanced
                        .AsyncRawQuery<MyProjection>(@"from Orders as o 
                                                       where o.ShipTo.City = 'London'
                                                       load o.Company as c, o.Employee as e
                                                       select {
                                                           order: o,
                                                           company: c,
                                                           employee: e
                                                       }");


                    IAsyncEnumerator<StreamResult<MyProjection>> results = await asyncSession.Advanced.StreamAsync(query);

                    while (await results.MoveNextAsync())
                    {
                        StreamResult<MyProjection> projection = results.Current;
                    }
                    #endregion
                }
            }
        }
    }

    public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
    {
        public Employees_ByFirstName()
        {
            Map = employees => from employee in employees
                               select new
                               {
                                   employee.FirstName
                               };
        }
    }

    #region class
    public class MyProjection
    {
        public Order order { get; set; }
        public Employee employee { get; set; }
        public Company company { get; set; }
    }
    #endregion
}
