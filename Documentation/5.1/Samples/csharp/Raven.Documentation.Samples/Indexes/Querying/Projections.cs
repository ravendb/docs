using System;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Queries;
using Raven.Documentation.Samples.Orders;
using Raven.Client.Documents.Linq;
using System.Collections.Generic;

namespace Raven.Documentation.Samples.Indexes.Querying
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

    #region indexes_1_stored
    public class Employees_ByFirstAndLastNameWithStoredFields : AbstractIndexCreationTask<Employee>
    {
        public Employees_ByFirstAndLastNameWithStoredFields()
        {
            Map = employees => from employee in employees
                               select new
                               {
                                   FirstName = employee.FirstName,
                                   LastName = employee.LastName
                               };
            StoreAllFields(FieldStorage.Yes); // FirstName and LastName fields can be retrieved directly from index
        }
    }
    #endregion

    #region indexes_2
    public class Employees_ByFirstNameAndBirthday : AbstractIndexCreationTask<Employee>
    {
        public Employees_ByFirstNameAndBirthday()
        {
            Map = employees => from employee in employees
                               select new
                               {
                                   FirstName = employee.FirstName,
                                   Birthday = employee.Birthday
                               };
        }
    }
    #endregion

    #region indexes_3
    public class Orders_ByShipToAndLines : AbstractIndexCreationTask<Order>
    {
        public Orders_ByShipToAndLines()
        {
            Map = orders => from order in orders
                            select new
                            {
                                ShipTo = order.ShipTo,
                                Lines = order.Lines
                            };
        }
    }
    #endregion

    #region indexes_4
    public class Orders_ByShippedAtAndCompany : AbstractIndexCreationTask<Order>
    {
        public Orders_ByShippedAtAndCompany()
        {
            Map = orders => from order in orders
                            select new
                            {
                                ShippedAt = order.ShippedAt,
                                Company = order.Company
                            };
        }
    }
    #endregion

    public class Projections
    {
        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region projections_1
                    var results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Select(x => new
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName
                        })
                        .ToList();
                    #endregion
                }
                using (var session = store.OpenSession())
                {
                    #region projections_1_stored
                    var results = session
                        .Query<Employee, Employees_ByFirstAndLastNameWithStoredFields>()
                        .Select(x => new
                        {
                            FirstName = x.FirstName,
                            LastName = x.LastName
                        })
                        .ToList();
                    #endregion
                }
                using (var session = store.OpenSession())
                {
                    #region projections_2
                    var results = session
                        .Query<Order, Orders_ByShipToAndLines>()
                        .Select(x => new
                        {
                            ShipTo = x.ShipTo,
                            Products = x.Lines.Select(y => y.ProductName),
                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_3
                    var results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Select(x => new
                        {
                            FullName = x.FirstName + " " + x.LastName
                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_4
                    var results = (from e in session.Query<Employee, Employees_ByFirstAndLastName>()
                                   let format = (Func<Employee, string>)(p => p.FirstName + " " + p.LastName)
                                   select new
                                   {
                                       FullName = format(e)
                                   }).ToList();
                    #endregion
                }
                using (var session = store.OpenSession())
                {
                    #region projections_5
                    var results = (from o in session.Query<Order, Orders_ByShippedAtAndCompany>()
                                   let c = RavenQuery.Load<Company>(o.Company)
                                   select new
                                   {
                                       CompanyName = c.Name,
                                       ShippedAt = o.ShippedAt
                                   }).ToList();
                    #endregion
                }
                using (var session = store.OpenSession())
                {
                    #region projections_6
                    var results = session
                        .Query<Employee, Employees_ByFirstNameAndBirthday>()
                        .Select(e => new
                        {
                            DayOfBirth = e.Birthday.Day,
                            MonthOfBirth = e.Birthday.Month,
                            Age = DateTime.Today.Year - e.Birthday.Year
                        }).ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_7
                    var results = session
                        .Query<Employee, Employees_ByFirstNameAndBirthday>()
                        .Select(e => new
                        {
                            Date = RavenQuery.Raw<DateTime>("new Date(Date.parse(e.Birthday))"),
                            Name = RavenQuery.Raw(e.FirstName, "substr(0,3)")
                        }).ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_8
                    var results = session
                        .Query<Employee, Employees_ByFirstAndLastName>()
                        .Select(e => new
                        {
                            Name = e.FirstName,
                            Metadata = RavenQuery.Metadata(e),
                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_9
                    var results = session
                        .Query<Order, Orders_ByShipToAndLines>()
                        .Select(x => new
                        {
                            Total = x.Lines.Sum(l => l.PricePerUnit * l.Quantity)

                        })
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region selectfields_1
                    var fields = new string[]{
                        "Name",
                        "Phone"
                    };

                    var results = session
                        .Advanced
                        .DocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>(fields)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region selectfields_2
                        var results = session
                        .Advanced
                        .DocumentQuery<Company, Companies_ByContact>()
                        .SelectFields<ContactDetails>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region projections_10
                    var results = session.Query<Company, Companies_ByContact>()
                        .ProjectInto<ContactDetails>()
                        .ToList();
                    #endregion
                }
            }
        }
    }

    #region index_10
    public class Companies_ByContact : AbstractIndexCreationTask<Company>
    {
        public Companies_ByContact()
        {
            Map = companies => companies
                .Select(x => new
                {
                    Name = x.Contact.Name,
                    x.Phone
                });

            StoreAllFields(FieldStorage.Yes); // Name and Phone fields can be retrieved directly from index
        }
    }
    #endregion

    #region projections_10_class
    public class ContactDetails
    {
        public string Name { get; set; }

        public string Phone { get; set; }
    }
    #endregion
}

