using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    class HowToFilterByField
    {
        public interface IFoo<T>
        {
            #region whereexists_1
            IDocumentQuery<T> WhereExists(string fieldName);

            IDocumentQuery<T> WhereExists<TValue>(Expression<Func<T, TValue>> propertySelector);
            #endregion
        }

        public async void Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_2
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("FirstName")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_2_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("FirstName")
                        .ToListAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region whereNotexists_1
                    List<Order> results = session
                        .Advanced
                        .DocumentQuery<Order>()
                        .WhereExists("Company")
                        .AndAlso()
                        .Not
                        .WhereExists("Employee")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region whereNotexists_signature
                    List<T> results = session
                        .Advanced
                        .DocumentQuery<T>()
                        .WhereExists("fieldName")
                        .AndAlso()
                        .Not
                        .WhereExists("fieldName2")
                        .ToList();
                    #endregion
                }


            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_3
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_3_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                        .ToListAsync();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereexists_4
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists(x => x.Address.Location.Latitude)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereexists_4_async
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists(x => x.Address.Location.Latitude)
                        .ToListAsync();
                    #endregion
                }
            }
        }
        public class Employee
        {
            public string FirstName { get; set; }
            public Address Address { get; set; }

        }

        public class Address
        {
            public Location Location { get; set; }
        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }

    internal class T
    {
    }
}
