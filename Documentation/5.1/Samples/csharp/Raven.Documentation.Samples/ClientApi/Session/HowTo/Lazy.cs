using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class Lazy
    {
        public Lazy()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region lazy_Load
                    Lazy<Employee> employeeLazy = session
                        .Advanced
                        .Lazily
                        .Load<Employee>("employees/1-A");

                    Employee employee = employeeLazy.Value; // load operation will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_LoadWithIncludes
                    Lazy<User> userLazy = session
                        .Advanced
                        .Lazily
                        .Include<User>(x => x.associateId)
                        .Load<User>("users/1-A");

                    User user = userLazy.Value; // load operation will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_Query
                    Lazy<IEnumerable<Employee>> employeesLazy = session
                        .Query<Employee>()
                        .Where( x => x.FirstName == "John")
                        .Lazily();

                    IEnumerable<Employee> employees = employeesLazy.Value; // The query will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_LoadStartingWith
                    // return entities whose Id starts with 'employees/'
                    Lazy<Dictionary<string, Employee>> employees = session
                        .Advanced
                        .Lazily
                        .LoadStartingWith<Employee>("employees/");
                    #endregion

                    int employeesCount = employees.Value.Count; // The operation will be executed here

                }

                using (var session = store.OpenSession())
                {
                    User user = new User();

                    #region lazy_ConditionalLoad
                    var lazy = session
                        .Advanced
                        .Lazily
                        // Load only if the change vector has changed
                        .ConditionalLoad<User>(user.Id, user.ChangeVector);
                    
                    var load = lazy.Value; // The operation will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_Revisions
                    var revisionsLazy = session
                    .Advanced
                    .Revisions
                    .Lazily
                    .GetFor<User>("users/1-A");

                    var revisionsLazyResult = revisionsLazy.Value; // The operation will be executed here
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region lazy_ExecuteAllPendingLazyOperations
                    Lazy<User> LazyLoad1 = 
                        session.Advanced.Lazily.Load<User>("users/1-A");
                    Lazy<User> LazyLoad2 =
                        session.Advanced.Lazily.Load<User>("users/2-A");
                    Lazy<IEnumerable<Employee>> LazyQuery = 
                        session.Query<Employee>().Lazily();

                    // Execute all pending lazy operations
                    session.Advanced.Eagerly.ExecuteAllPendingLazyOperations(); 
                    #endregion
                }
            }
        }

        #region lazy_UserDefinition
        internal class User
        {
            public string Id { get; set; }
            public string associateId { get; set; }
            public string Name { get; internal set; }
            public string ChangeVector { get; set; }
        }
        #endregion
    }
}
