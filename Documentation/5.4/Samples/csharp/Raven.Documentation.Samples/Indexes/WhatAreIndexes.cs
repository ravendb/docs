using System.Collections.Generic;
using System.Linq;

using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class WhatAreIndexes
    {
        #region indexes_1
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            public Employees_ByFirstAndLastName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       FirstName = employee.FirstName,
                                       LastName = employee.LastName
                                   };
            }
        }
        #endregion

        public WhatAreIndexes()
        {
            using (var store = new DocumentStore())
            {
                #region indexes_2
                // save index on server
                new Employees_ByFirstAndLastName().Execute(store);
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexes_3
                    IList<Employee> results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
