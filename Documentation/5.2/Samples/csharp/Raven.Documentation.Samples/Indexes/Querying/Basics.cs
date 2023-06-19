using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Employees_ByFirstName : AbstractIndexCreationTask<Employee>
    {
    }

    public class Basics
    {
        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region basics_0_0
                    // load all entities from 'Employees' collection
                    IList<Employee> results = session
                        .Query<Employee>()
                        .ToList(); // send query
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_1_0
                    // load all entities from 'Employees' collection
                    IList<Employee> results = await asyncSession
                        .Query<Employee>()
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_0_1
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    IList<Employee> results = session
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList(); // send query
                    #endregion
                }


                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_1_1
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    IList<Employee> results = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_0_2
                    // load up to 10 entities from 'Products' collection
                    // where there are more than 10 units in stock
                    // skip first 5 results
                    IList<Product> results = session
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                        .Skip(5)
                        .Take(10)
                        .ToList(); // send query
                    #endregion
                }


                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_1_2
                    // load up to 10 entities from 'Products' collection
                    // where there are more than 10 units in stock
                    // skip first 5 results
                    IList<Product> results = await asyncSession
                        .Query<Product>()
                        .Where(x => x.UnitsInStock > 10)
                        .Skip(5)
                        .Take(10)
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_0_3
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = session
                        .Query<Employee, Employees_ByFirstName>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList(); // send query
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_1_3
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = await asyncSession
                        .Query<Employee, Employees_ByFirstName>()
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_0_4
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = session
                        .Query<Employee>("Employees/ByFirstName")
                        .Where(x => x.FirstName == "Robert")
                        .ToList(); // send query
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_1_4
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = await asyncSession
                        .Query<Employee>("Employees/ByFirstName")
                        .Where(x => x.FirstName == "Robert")
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_2_0
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee, Employees_ByFirstName>()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToList(); // send query
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_2_1
                    // load all entities from 'Employees' collection
                    // where 'FirstName' is 'Robert'
                    // using 'Employees/ByFirstName' index
                    IList<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee, Employees_ByFirstName>()
                        .WhereEquals(x => x.FirstName, "Robert")
                        .ToListAsync(); // send query
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region basics_3_0
                    // load up entity from 'Employees' collection
                    // with ID matching 'employees/1-A'
                    Employee result = session
                        .Query<Employee>()
                        .Where(x => x.Id == "employees/1-A")
                        .FirstOrDefault(); // send query
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region basics_3_1
                    // load up entity from 'Employees' collection
                    // with ID matching 'employees/1-A'
                    Employee result = await asyncSession
                        .Query<Employee>()
                        .Where(x => x.Id == "employees/1-A")
                        .FirstOrDefaultAsync(); // send query
                    #endregion
                }
            }
        }
    }
}
