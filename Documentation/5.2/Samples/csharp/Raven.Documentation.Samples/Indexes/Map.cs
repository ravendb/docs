using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class Map
    {
        /*
        #region indexes_1
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            // ...
        }
        #endregion
        */

        /*
        #region javaScriptindexes_1
        public class Employees_ByFirstAndLastName : AbstractJavaScriptIndexCreationTask
        {
            // ...
        }
        #endregion
        */
        /*
        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            #region indexes_2
            public Employees_ByFirstAndLastName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       FirstName = employee.FirstName,
                                       LastName = employee.LastName
                                   };
            }
            #endregion
        }
        */

        public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
        {
            #region indexes_3
            public Employees_ByFirstAndLastName()
            {
                Map = employees => employees
                    .Select(employee => new
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName
                    });
            }
            #endregion
        }

        /*
        #region javaScriptindexes_2
        public Employees_ByFirstAndLastName()
        {
            Maps = new HashSet<string>
            {
                @"map('Employees', function (employee){ 
                        return { 
                            FirstName : employee.FirstName, 
                            LastName : employee.LastName
                        };
                    })",
            };
        }
        #endregion
        */

        /*
        #region indexes_6
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
        */

        /*
        #region javaScriptindexes_6
        public class Employees_ByFirstAndLastName : AbstractJavaScriptIndexCreationTask
        {
            public Employees_ByFirstAndLastName()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                            return { 
                                FirstName : employee.FirstName, 
                                LastName : employee.LastName
                            };
                        })",
                };
            }
        }
        #endregion
        */

        #region indexes_7
        public class Employees_ByFullName : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public string FullName { get; set; }
            }

            public Employees_ByFullName()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       FullName = employee.FirstName + " " + employee.LastName
                                   };
            }
        }
        #endregion
        /*
        #region javaScriptindexes_7
        public class Employees_ByFullName : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string FullName { get; set; }
            }

            public Employees_ByFullName()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                            return { 
                                FullName  : employee.FirstName + ' ' + employee.LastName
                            };
                        })",
                };
            }
        }
        #endregion
        */
        #region indexes_1_0
        public class Employees_ByYearOfBirth : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public int YearOfBirth { get; set; }
            }

            public Employees_ByYearOfBirth()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       YearOfBirth = employee.Birthday.Year
                                   };
            }
        }
        #endregion

        /*
        #region javaScriptindexes_1_0
        public class Employees_ByYearOfBirth : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public int YearOfBirth { get; set; }
            }

            public Employees_ByYearOfBirth()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Birthday : employee.Birthday.Year 
                                } 
                           })"
                };
            }
        }
        #endregion
        */

        #region indexes_1_6
        public class Companies_ByAddress_Country : AbstractIndexCreationTask<Company>
        {
            public class Result
            {
                public string City { get; set; }
                public string Company { get; set; }
                public string Phone { get; set; }
            }

            public Companies_ByAddress_Country()
            {
                Map = companies => from company in companies
                                   where company.Address.Country == "USA"
                                   select new Result
                                   {
                                       Company = company.Name,
                                       City = company.Address.City,
                                       Phone = company.Phone
                                   };
            }
        }
        #endregion

        #region indexes_1_7
        public class Companies_ByAddress_Latitude : AbstractIndexCreationTask<Company>
        {
            public class Result
            {
                public double latitude { get; set; }
                public double longitude { get; set; }
                public string companyName { get; set; }
                public string companyAddress { get; set; }
                public string companyPhone { get; set; }
            }

            public Companies_ByAddress_Latitude()
            {
                Map = companies => from company in companies
                                   where (company.Address.Location.Latitude > 20 && company.Address.Location.Latitude < 50)
                                   select new
                                   {
                                       latitude = company.Address.Location.Latitude,
                                       longitude = company.Address.Location.Latitude,
                                       companyName = company.Name,
                                       companyAddress = company.Address,
                                       companyPhone = company.Phone
                                   };
            }
        }
        #endregion


        #region indexes_1_2
        public class Employees_ByBirthday : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public DateTime Birthday { get; set; }
            }

            public Employees_ByBirthday()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       Birthday = employee.Birthday
                                   };
            }
        }
        #endregion

        /*
        #region javaScriptindexes_1_2
        public class Employees_ByBirthday : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public DateTime Birthday { get; set; }
            }

            public Employees_ByBirthday()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Birthday : employee.Birthday 
                                } 
                           })"
                };
            }
        }
        #endregion
        */
        #region indexes_1_4
        public class Employees_ByCountry : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public string Country { get; set; }
            }

            public Employees_ByCountry()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       Country = employee.Address.Country
                                   };
            }
        }
        #endregion

        /*
        #region javaScriptindexes_1_4
        public class Employees_ByCountry : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string Country { get; set; }
            }

            public Employees_ByCountry()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee){ 
                                return {
                                    Country : employee.Address.Country 
                                 } 
                           })"
                };
            }
        }
        #endregion
        */
        #region indexes_1_6
        public class Employees_Query : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public string[] Query { get; set; }
            }

            public Employees_Query()
            {
                Map = employees => from employee in employees
                                   select new Result
                                   {
                                       Query = new[]
                                        {
                                            employee.FirstName,
                                            employee.LastName,
                                            employee.Title,
                                            employee.Address.City
                                        }
                                   };

                Index("Query", FieldIndexing.Search);
            }
        }
        #endregion

        /*
        #region javaScriptindexes_1_6
        public class Employees_Query : AbstractJavaScriptIndexCreationTask
        {
            public class Result
            {
                public string[] Query { get; set; }
            }

            public Employees_Query()
            {
                Maps = new HashSet<string>
                {
                    @"map('Employees', function (employee) { 
                            return { 
                                Query : [employee.FirstName, 
                                         employee.LastName,
                                         employee.Title,
                                         employee.Address.City] } })"
                };
                Fields = new Dictionary<string, IndexFieldOptions>()
                {
                    {"Query", new IndexFieldOptions(){ Indexing = FieldIndexing.Search} }
                };
            }
        }
        #endregion
        */

        public Map()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region indexes_4
                    IList<Employee> employees1 = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Where(x => x.FirstName == "Robert")
                        .ToList();

                    IList<Employee> employees2 = session
                        .Query<Employee>("Employees/ByFirstAndLastName")
                        .Where(x => x.FirstName == "Robert")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_8
                    // notice that we're 'cheating' here
                    // by marking result type in 'Query' as 'Employees_ByFullName.Result' to get strongly-typed syntax
                    // and changing type using 'OfType' before sending query to server
                    IList<Employee> employees = session
                        .Query<Employees_ByFullName.Result, Employees_ByFullName>()
                        .Where(x => x.FullName == "Robert King")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_9
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employee, Employees_ByFullName>()
                        .WhereEquals("FullName", "Robert King")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_6_1
                    IList<Employee> employees = session
                        .Query<Employees_ByYearOfBirth.Result, Employees_ByYearOfBirth>()
                        .Where(x => x.YearOfBirth == 1963)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_6_2
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_ByYearOfBirth.Result, Employees_ByYearOfBirth>()
                        .WhereEquals(x => x.YearOfBirth, 1963)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_5_1
                    DateTime startDate = new DateTime(1963, 1, 1);
                    DateTime endDate = startDate.AddYears(1).AddMilliseconds(-1);
                    IList<Employee> employees = session
                        .Query<Employees_ByBirthday.Result, Employees_ByBirthday>()
                        .Where(x => x.Birthday >= startDate && x.Birthday <= endDate)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_5_2
                    DateTime startDate = new DateTime(1963, 1, 1);
                    DateTime endDate = startDate.AddYears(1).AddMilliseconds(-1);
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_ByBirthday.Result, Employees_ByBirthday>()
                        .WhereBetween(x => x.Birthday, startDate, endDate)
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_7_1
                    IList<Employee> employees = session
                        .Query<Employees_ByCountry.Result, Employees_ByCountry>()
                        .Where(x => x.Country == "USA")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_7_2
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_ByCountry.Result, Employees_ByCountry>()
                        .WhereEquals(x => x.Country, "USA")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_1_7
                    IList<Employee> employees = session
                        .Query<Employees_Query.Result, Employees_Query>()
                        .Search(x => x.Query, "John Doe")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_1_8
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_Query.Result, Employees_Query>()
                        .Search(x => x.Query, "John Doe")
                        .SelectFields<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_query_1_6
                    IList<Company> orders = session
                        .Query<Companies_ByAddress_Country.Result, Companies_ByAddress_Country>()
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_query_1_7
                    IList<Company> orders = session
                        .Query<Companies_ByAddress_Latitude.Result, Companies_ByAddress_Latitude>()
                        .OfType<Company>()
                        .ToList();
                    #endregion
                }

            }
        }
    }
}
