using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class GetTrackedEntities
    {
        public GetTrackedEntities()
        {
            using (var store = new DocumentStore())
            {
                #region get_tracked_entities_1
                using (var session = store.OpenSession())
                {
                    // Store entities within the session:
                    Employee employee1 = new Employee {FirstName = "John", LastName = "Doe"};
                    Employee employee2 = new Employee {FirstName = "David", LastName = "Brown"};
                    Employee employee3 = new Employee {FirstName = "Tom", LastName = "Miller"};
                    session.Store(employee1, "employees/1-A");
                    session.Store(employee2, "employees/2-A");
                    session.Store(employee3, "employees/3-A");

                    // Get tracked entities:
                    IDictionary<string, EntityInfo> trackedEntities = session.Advanced.GetTrackedEntities();

                    // The session tracks the 3 new stored entities:
                    Assert.Equivalent(3, trackedEntities.Keys.Count);
                    var entityInfo = trackedEntities["employees/1-A"];
                    Assert.Equal("employees/1-A", entityInfo.Id);
                    Assert.True(entityInfo.Entity is Employee);
                    
                    // Save changes:
                    session.SaveChanges();
                    
                    // The session keeps tracking the entities even after SaveChanges is called:
                    trackedEntities = session.Advanced.GetTrackedEntities();
                    Assert.Equivalent(3, trackedEntities.Keys.Count);
                }
                #endregion
                
                #region get_tracked_entities_2
                using (var session = store.OpenSession())
                {
                    // Load entity:
                    Employee employee1 = session.Load<Employee>("employees/1-A");
                    
                    // Delete entity:
                    session.Delete("employees/3-A");
                    
                    // Get tracked entities:
                    IDictionary<string, EntityInfo> trackedEntities = session.Advanced.GetTrackedEntities();
                    
                    // The session tracks the 2 entities:
                    Assert.Equivalent(2, trackedEntities.Keys.Count);
                    
                    // Note the 'IsDeleted' property that is set for deleted entities:
                    var entityInfo = trackedEntities["employees/3-A"];
                    Assert.True(entityInfo.IsDeleted);
                    
                    // Save changes:
                    session.SaveChanges();
                }
                #endregion
                
                #region get_tracked_entities_3
                using (var session = store.OpenSession())
                {
                    // Query for all employees:
                    var employees = session.Query<Employee>().ToList();
                    
                    // Get tracked entities:
                    IDictionary<string, EntityInfo> trackedEntities = session.Advanced.GetTrackedEntities();
                    
                    // The session tracks the entities loaded via the query:
                    Assert.Equivalent(2, trackedEntities.Keys.Count); 
                }
                #endregion
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            IDictionary<string, EntityInfo> GetTrackedEntities();
            #endregion
        }
        
        private interface IFoo2
        {
            #region syntax_2
            public class EntityInfo
            {
                public string Id { get; set; }
                public object Entity { get; set; }
                public bool IsDeleted { get; set; }
            }
            #endregion
        }
    }
}
