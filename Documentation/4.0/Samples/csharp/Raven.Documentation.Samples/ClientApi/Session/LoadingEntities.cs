
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Session;
using Raven.Client.Documents.Session.Loaders;
using Raven.Client.Util;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session
{
    using System.Linq;

    public class LoadingEntities
    {
        private interface IFoo
        {
            #region loading_entities_1_0
            TResult Load<TResult>(string id);
            #endregion

            #region loading_entities_1_0_async
            Task<TResult> LoadAsync<TResult>(string id);
            #endregion

            #region loading_entities_2_0
            ILoaderWithInclude<object> Include(string path);

            ILoaderWithInclude<T> Include<T>(Expression<Func<T, string>> path);

            ILoaderWithInclude<T> Include<T>(Expression<Func<T, IEnumerable<string>>> path);

            ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, string>> path);

            ILoaderWithInclude<T> Include<T, TInclude>(Expression<Func<T, IEnumerable<string>>> path);
            #endregion

            #region loading_entities_3_0
            Dictionary<string, TResult> Load<TResult>(IEnumerable<string> ids);
            #endregion

            #region loading_entities_3_0_async
            Task<Dictionary<string, TResult>> LoadAsync<TResult>(IEnumerable<string> ids);
            #endregion

            #region loading_entities_4_0
            T[] LoadStartingWith<T>(
                string idPrefix,
                string matches = null,
                int start = 0,
                int pageSize = 25,
                string exclude = null,
                string startAfter = null);

            void LoadStartingWithIntoStream(
                string idPrefix,
                Stream output,
                string matches = null,
                int start = 0,
                int pageSize = 25,
                string exclude = null,
                string startAfter = null);
            #endregion

            #region loading_entities_4_0_async
            Task<T[]> LoadStartingWithAsync<T>(
                string idPrefix,
                string matches = null,
                int start = 0,
                int pageSize = 25,
                string exclude = null,
                string startAfter = null);

            Task LoadStartingWithIntoStreamAsync(
                string idPrefix,
                Stream output,
                string matches = null,
                int start = 0,
                int pageSize = 25,
                string exclude = null,
                string startAfter = null);
            #endregion

            #region loading_entities_5_0
            IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IQueryable<T> query, out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query);

            IEnumerator<StreamResult<T>> Stream<T>(IRawDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(IDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);

            IEnumerator<StreamResult<T>> Stream<T>(string startsWith, string matches = null, int start = 0, int pageSize = int.MaxValue, string startAfter = null);
            #endregion

            /*
            #region loading_entities_5_0_async
            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IQueryable<T> query);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IQueryable<T> query, out StreamQueryStatistics streamQueryStats);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IDocumentQuery<T> query);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IRawDocumentQuery<T> query);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IRawDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(IDocumentQuery<T> query, out StreamQueryStatistics streamQueryStats);

            Task<IAsyncEnumerator<StreamResult<T>>> StreamAsync<T>(string startsWith, string matches = null, int start = 0, int pageSize = int.MaxValue, string startAfter = null);
            #endregion
            */

            #region loading_entities_6_0
            bool IsLoaded(string id);
            #endregion
        }

        public async Task LoadingEntitiesXY()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region loading_entities_1_1

                    Employee employee = session.Load<Employee>("employees/1");

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_1_1_async

                    Employee employee = await asyncSession.LoadAsync<Employee>("employees/1");

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_2_1

                    // loading 'products/1'
                    // including document found in 'Supplier' property
                    Product product = session
                        .Include("Supplier")
                        .Load<Product>("products/1");

                    Supplier supplier = session.Load<Supplier>(product.Supplier); // this will not make server call

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_2_1_async

                    // loading 'products/1'
                    // including document found in 'Supplier' property
                    Product product = await asyncSession
                        .Include("Supplier")
                        .LoadAsync<Product>("products/1");

                    Supplier supplier = await asyncSession.LoadAsync<Supplier>(product.Supplier); // this will not make server call

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_2_2

                    // loading 'products/1'
                    // including document found in 'Supplier' property
                    Product product = session
                        .Include<Product>(x => x.Supplier)
                        .Load<Product>("products/1");

                    Supplier supplier = session.Load<Supplier>(product.Supplier); // this will not make server call

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_2_2_async

                    // loading 'products/1'
                    // including document found in 'Supplier' property
                    Product product = await asyncSession
                        .Include<Product>(x => x.Supplier)
                        .LoadAsync<Product>("products/1");

                    Supplier supplier = await asyncSession.LoadAsync<Supplier>(product.Supplier); // this will not make server call

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_3_1

                    Dictionary<string, Employee> employees = session.Load<Employee>(new[]
                    {
                        "employees/1",
                        "employees/2",
                        "employees/3"
                    });

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_3_1_async

                    Dictionary<string, Employee> employees = await asyncSession.LoadAsync<Employee>(new[]
                    {
                        "employees/1",
                        "employees/2",
                    });

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_4_1
                    // return up to 128 entities with Id that starts with 'employees'
                    Employee[] result = session
                        .Advanced
                        .LoadStartingWith<Employee>("employees", null, 0, 128);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_4_1_async
                    // return up to 128 entities with Id that starts with 'employees'
                    Employee[] result = (await asyncSession
                        .Advanced
                        .LoadStartingWithAsync<Employee>("employees", null, 0, 128))
                        .ToArray();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_4_2
                    // return up to 128 entities with Id that starts with 'employees/' 
                    // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
                    Employee[] result = session
                        .Advanced
                        .LoadStartingWith<Employee>("employees/", "1*|2*", 0, 128);
                    #endregion
                }


                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_4_2_async
                    // return up to 128 entities with Id that starts with 'employees/' 
                    // and rest of the key begins with "1" or "2" e.g. employees/10, employees/25
                    Employee[] result = (await asyncSession
                        .Advanced
                        .LoadStartingWithAsync<Employee>("employees/", "1*|2*", 0, 128))
                        .ToArray();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_5_1

                    IEnumerator<StreamResult<Employee>> enumerator = session
                        .Advanced
                        .Stream<Employee>("employees/");

                    while (enumerator.MoveNext())
                    {
                        StreamResult<Employee> employee = enumerator.Current;
                    }

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_5_1_async

                    Raven.Client.Util.IAsyncEnumerator<StreamResult<Employee>> enumerator = await asyncSession
                        .Advanced
                        .StreamAsync<Employee>("employees/");

                    while (await enumerator.MoveNextAsync())
                    {
                        StreamResult<Employee> employee = enumerator.Current;
                    }

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_5_2

                    using (var outputStream = new MemoryStream())
                    {
                        session
                            .Advanced
                            .LoadStartingWithIntoStream("employees/", outputStream);
                    }

                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_5_2_async

                    using (var outputStream = new MemoryStream())
                    {
                        await asyncSession
                            .Advanced
                            .LoadStartingWithIntoStreamAsync("employees/", outputStream);
                    }

                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region loading_entities_6_1
                    bool isLoaded = session.Advanced.IsLoaded("employees/1"); // false
                    Employee employee = session.Load<Employee>("employees/1");
                    isLoaded = session.Advanced.IsLoaded("employees/1"); // true
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region loading_entities_6_1_async
                    bool isLoaded = asyncSession.Advanced.IsLoaded("employees/1"); // false
                    Employee employee = await asyncSession.LoadAsync<Employee>("employees/1");
                    isLoaded = asyncSession.Advanced.IsLoaded("employees/1"); // true
                    #endregion
                }
            }
        }
    }
}
