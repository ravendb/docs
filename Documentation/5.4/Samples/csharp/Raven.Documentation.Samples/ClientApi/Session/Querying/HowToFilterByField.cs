using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq.Indexing;
using Raven.Client.Documents.Session;

namespace Raven.Documentation.Samples.ClientApi.Session.Querying
{
    class HowToFilterByField
    {
        public interface IFoo<T>
        {
            #region whereExists_syntax
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
                    #region whereExists_1
                    // Only documents that contain field 'FirstName' will be returned
                    
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("FirstName")
                         // Or use lambda expression: .WhereExists(x => x.FirstName)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereExists_1_async
                    // Only documents that contain field 'FirstName' will be returned
                    
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("FirstName")
                         // Or use lambda expression: .WhereExists(x => x.FirstName)
                        .ToListAsync();
                    #endregion
                }
            }
            
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region whereExists_2
                    // Only documents that contain the 'Latitude' property in the specified path will be returned
                    
                    List<Employee> results = session
                        .Advanced
                        .DocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                         // Or use lambda expression: .WhereExists(x => x.Address.Location.Latitude)
                        .ToList();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region whereExists_2_async
                    // Only documents that contain the 'Latitude' property in the specified path will be returned
                    
                    List<Employee> results = await asyncSession
                        .Advanced
                        .AsyncDocumentQuery<Employee>()
                        .WhereExists("Address.Location.Latitude")
                         // Or use lambda expression: .WhereExists(x => x.Address.Location.Latitude)
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

    internal class TIndexCreator
    {
    }

    internal class T
    {
    }
}
