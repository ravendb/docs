using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class SessionChanges
    {
        public SessionChanges()
        {
            using (var store = new DocumentStore())
            {
                #region changes_1
                using (IDocumentSession session = store.OpenSession())
                {
                    // No changes made yet - 'HasChanges' will be FALSE 
                    Assert.False(session.Advanced.HasChanges);
                    
                    // Store a new entity within the session
                    session.Store(new Employee { FirstName = "John", LastName = "Doe" });
                    
                    // 'HasChanges' will now be TRUE 
                    Assert.True(session.Advanced.HasChanges);
                    
                    // 'HasChanges' will reset to FALSE after saving changes 
                    session.SaveChanges();
                    Assert.False(session.Advanced.HasChanges);
                }
                #endregion

                #region changes_2
                using (IDocumentSession session = store.OpenSession())
                {
                    // Store (add) new entities, they will be tracked by the session
                    session.Store(new Employee { FirstName = "John", LastName = "Doe" }, "employees/1-A");
                    session.Store(new Employee { FirstName = "Jane", LastName = "Doe" }, "employees/2-A");

                    // Call 'WhatChanged' to get all changes in the session
                    IDictionary<string, DocumentsChanges[]> changes = session.Advanced.WhatChanged();
                    Assert.Equal(changes.Count, 2); // 2 entities were added
                    
                    // Get the change details for an entity, specify the entity ID
                    DocumentsChanges[] changesForEmployee = changes["employees/1-A"];
                    Assert.Equal(changesForEmployee.Length, 1); // a single change for this entity (adding)
                    
                    // Get the change type
                    DocumentsChanges.ChangeType changeType = changesForEmployee[0].Change;
                    Assert.Equal(changeType, DocumentsChanges.ChangeType.DocumentAdded);
                    
                    session.SaveChanges();
                }
                #endregion

                #region changes_3
                using (IDocumentSession session = store.OpenSession())
                {
                    // Load entities, they will be tracked by the session
                    Employee employee1 = session.Load<Employee>("employees/1-A");
                    Employee employee2 = session.Load<Employee>("employees/2-A");
                    
                    // Modify entities
                    employee1.FirstName = "Jim";
                    employee1.LastName = "Brown";
                    employee2.LastName = "Smith";
                    
                    // Delete an entity
                    session.Delete(employee2);

                    // Call 'WhatChanged' to get all changes in the session
                    IDictionary<string, DocumentsChanges[]> changes = session.Advanced.WhatChanged();
                    
                    // Get the change details for an entity, specify the entity ID
                    DocumentsChanges[] changesForEmployee = changes["employees/1-A"];
                    
                    Assert.Equal(changesForEmployee[0].FieldName, "FirstName");
                    Assert.Equal(changesForEmployee[0].FieldNewValue, "Jim");
                    Assert.Equal(changesForEmployee[0].Change, DocumentsChanges.ChangeType.FieldChanged);
                    
                    Assert.Equal(changesForEmployee[1].FieldName, "LastName");
                    Assert.Equal(changesForEmployee[1].FieldNewValue, "Brown");
                    Assert.Equal(changesForEmployee[1].Change, DocumentsChanges.ChangeType.FieldChanged);
                    
                    // Note: for employee2 - even though the LastName was changed to 'Smith',
                    // the only reported change is the latest modification, which is the delete action. 
                    changesForEmployee = changes["employees/2-A"];
                    Assert.Equal(changesForEmployee[0].Change, DocumentsChanges.ChangeType.DocumentDeleted);
                    
                    session.SaveChanges();
                }
                #endregion
            }
        }
        
        public interface IFoo
        {
            #region syntax_1
            // HasChanges
            bool HasChanges { get; }
            #endregion

            #region syntax_2
            // WhatChanged
            IDictionary<string, DocumentsChanges[]> WhatChanged();
            #endregion
        }
        
        public class Foo
        {
            #region syntax_3
            public class DocumentsChanges
            {
                public object FieldOldValue { get; set; }  // Previous field value
                public object FieldNewValue { get; set; }  // Current field value
                public ChangeType Change { get; set; }     // Type of change that occurred
                public string FieldName { get; set; }      // Name of field on which the change occurred
                public string FieldPath { get; set; }      // Path of field on which the change occurred
                public string FieldFullName { get; }       // Path + Name of field on which the change occurred
            }
            
            public enum ChangeType
            {
                DocumentDeleted,
                DocumentAdded,
                FieldChanged,
                NewField,
                RemovedField,
                ArrayValueChanged,
                ArrayValueAdded,
                ArrayValueRemoved
            }
            #endregion
        }
    }
}
