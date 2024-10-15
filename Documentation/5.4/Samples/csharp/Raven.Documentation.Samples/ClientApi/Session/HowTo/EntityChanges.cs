using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Session.HowTo
{
    public class EntityChanges
    {
        public EntityChanges()
        {
            using (var store = new DocumentStore())
            {
                #region changes_1
                using (var session = store.OpenSession())
                {
                    // Store a new entity within the session
                    // =====================================
                    
                    Employee employee = new Employee {FirstName = "John", LastName = "Doe"};
                    session.Store(employee, "employees/1-A");
                    
                    // 'HasChanged' will be TRUE 
                    Assert.True(session.Advanced.HasChanged(employee));
                    
                    // 'HasChanged' will reset to FALSE after saving changes 
                    session.SaveChanges();
                    Assert.False(session.Advanced.HasChanged(employee));
                    
                    // Load & modify entity within the session
                    // =======================================
                    
                    employee = session.Load<Employee>("employees/1-A");
                    Assert.False(session.Advanced.HasChanged(employee)); // FALSE
                    
                    employee.LastName = "Brown";
                    Assert.True(session.Advanced.HasChanged(employee)); // TRUE
                    
                    session.SaveChanges();
                    Assert.False(session.Advanced.HasChanged(employee)); // FALSE
                }
                #endregion
                
                #region changes_2
                using (var session = store.OpenSession())
                {
                    // Store (add) a new entity, it will be tracked by the session
                    Employee employee = new Employee {FirstName = "John", LastName = "Doe"};
                    session.Store(employee, "employees/1-A");

                    // Get the changes for the entity in the session
                    // Call 'WhatChangedFor', pass the entity object in the param
                    DocumentsChanges[] changesForEmployee = session.Advanced.WhatChangedFor(employee);
                    Assert.Equal(changesForEmployee.Length, 1); // a single change for this entity (adding)
    
                    // Get the change type
                    DocumentsChanges.ChangeType changeType = changesForEmployee[0].Change;
                    Assert.Equal(changeType, DocumentsChanges.ChangeType.DocumentAdded);
    
                    session.SaveChanges();
                }
                #endregion
                
                #region changes_3
                using (var session = store.OpenSession())
                {
                    // Load the entity, it will be tracked by the session
                    Employee employee = session.Load<Employee>("employees/1-A");
                    
                    // Modify the entity
                    employee.FirstName = "Jim";
                    employee.LastName = "Brown";
                    
                    // Get the changes for the entity in the session
                    // Call 'WhatChangedFor', pass the entity object in the param
                    DocumentsChanges[] changesForEmployee = session.Advanced.WhatChangedFor(employee);
                   
                    Assert.Equal(changesForEmployee[0].FieldName, "FirstName");                           // Field name
                    Assert.Equal(changesForEmployee[0].FieldNewValue, "Jim");                             // New value
                    Assert.Equal(changesForEmployee[0].Change, DocumentsChanges.ChangeType.FieldChanged); // Change type
                    
                    Assert.Equal(changesForEmployee[1].FieldName, "LastName");
                    Assert.Equal(changesForEmployee[1].FieldNewValue, "Brown");
                    Assert.Equal(changesForEmployee[1].Change, DocumentsChanges.ChangeType.FieldChanged);
                    
                    session.SaveChanges();
                }
                #endregion
            }
        }
        
        private interface IFoo
        {
            #region syntax_1
            // HasChanged
            bool HasChanged(object entity);
            #endregion
        }
        
        private interface IFoo2
        {
            #region syntax_2
            // WhatChangedFor
            DocumentsChanges[] WhatChangedFor(object entity);
            #endregion
            
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
