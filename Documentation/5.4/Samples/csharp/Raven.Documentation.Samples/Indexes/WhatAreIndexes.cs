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
        // Define the index:
        // =================
        
        public class Employees_ByNameAndCountry : AbstractIndexCreationTask<Employee>
        {
            public class IndexEntry
            {
                // The index-fields
                public string LastName { get; set; }
                public string FullName { get; set; }
                public string Country { get; set; }
            }
            
            public Employees_ByNameAndCountry()
            {
                Map = employees => from employee in employees
                                   select new IndexEntry()
                                   {
                                       // Define the content for each index-field
                                       LastName = employee.LastName,
                                       FullName = employee.FirstName + " " + employee.LastName,
                                       Country = employee.Address.Country
                                   };
            }
        }
        #endregion

        public WhatAreIndexes()
        {
            using (var store = new DocumentStore())
            {
                #region indexes_2
                // Deploy the index to the server:
                // ===============================
                
                new Employees_ByNameAndCountry().Execute(store);
                #endregion

                using (var session = store.OpenSession())
                {
                    #region indexes_3
                    // Query the database using the index: 
                    // ===================================

                    IList<Employee> employeesFromUK = session
                        .Query<Employees_ByNameAndCountry.IndexEntry, Employees_ByNameAndCountry>()
                         // Here we query for all Employee documents that are from the UK
                         // and have 'King' in their LastName field:
                        .Where(x => x.LastName == "King" && x.Country == "UK")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }
            }
        }
    }
}
