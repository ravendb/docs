using System.Collections.Generic;
using System.Linq;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Storing
    {
        #region storing_1
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstAndLastName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       employee.FirstName,
                                       employee.LastName
                                   };

                Stores.Add(x => x.FirstName, FieldStorage.Yes);
                Stores.Add(x => x.LastName, FieldStorage.Yes);
            }
        }
        #endregion

        public Storing()
        {
            using (var store = new DocumentStore())
            {
                #region storing_2
                store
                    .Maintenance
                    .Send(new PutIndexesOperation(
                        new IndexDefinition
                        {
                            Name = "Employees_ByFirstAndLastName",
                            Maps =
                            {
                                @"from employee in docs.Employees
                                  select new
                                  {
                                      employee.FirstName,
                                      employee.LastName
                                  }"
                            },
                            Fields = new Dictionary<string, IndexFieldOptions>
                            {
                                {"FirstName", new IndexFieldOptions
                                              {
                                                  Storage = FieldStorage.Yes
                                              }
                                },
                                {"LastName", new IndexFieldOptions
                                             {
                                                Storage = FieldStorage.Yes
                                             }
                                }
                            }
                        }));
                #endregion
            }
        }
    }
}
