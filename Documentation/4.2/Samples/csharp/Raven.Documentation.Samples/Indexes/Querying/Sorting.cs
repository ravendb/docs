using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations.Sorters;
using Raven.Client.Documents.Queries.Sorting;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes.Querying
{
    public class Sorting
    {
        #region sorting_5_6
        public class Employee_ByFirstName : AbstractIndexCreationTask<Employee>
        {
            public Employee_ByFirstName()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                       employee.FirstName
                                   };

                Store(x => x.FirstName, FieldStorage.Yes);
            }
        }
        #endregion

        #region sorting_1_4
        public class Products_ByUnitsInStock : AbstractIndexCreationTask<Product>
        {
            public Products_ByUnitsInStock()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      product.UnitsInStock
                                  };
            }
        }
        #endregion

        #region sorting_1_5
        public class Products_ByUnitsInStockAndName : AbstractIndexCreationTask<Product>
        {
            public Products_ByUnitsInStockAndName()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      product.UnitsInStock,
                                      product.Name
                                  };
            }
        }
        #endregion

        #region sorting_6_4
        public class Products_ByName_Search : AbstractIndexCreationTask<Product>
        {
            public class Result
            {
                public string Name { get; set; }

                public string NameForSorting { get; set; }
            }

            public Products_ByName_Search()
            {
                Map = products => from product in products
                                  select new
                                  {
                                      Name = product.Name,
                                      NameForSorting = product.Name
                                  };

                Indexes.Add(x => x.Name, FieldIndexing.Search);
            }
        }
        #endregion

        #region sorting_9_3
        public class Events_ByCoordinates : AbstractIndexCreationTask<Event>
        {
            public Events_ByCoordinates()
            {
                Map = events => from e in events
                                select new
                                {
                                    Coordinates = CreateSpatialField(e.Latitude, e.Longitude)
                                };
            }
        }
        #endregion

        public Sorting()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_1_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_1_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_2_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OrderByDescending(x => x.UnitsInStock)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_2_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OrderByDescending(x => x.UnitsInStock)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_8_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OrderByDescending(x => x.UnitsInStock, OrderingType.String)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_8_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OrderByDescending("UnitsInStock", OrderingType.String)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_3_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Customize(x => x.RandomOrdering())
                        .Where(x => x.UnitsInStock > 10)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_3_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .RandomOrdering()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_4_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OrderByScore()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_4_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OrderByScore()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_4_3
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStockAndName>()
                        .Where(x => x.UnitsInStock > 10)
                        .OrderBy(x => x.UnitsInStock)
                        .ThenByScore()
                        .ThenByDescending(x => x.Name)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_6_1
                    IList<Product> results = session
                        .Query<Products_ByName_Search.Result, Products_ByName_Search>()
                        .Search(x => x.Name, "Louisiana")
                        .OrderByDescending(x => x.NameForSorting)
                        .OfType<Product>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_6_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByName_Search>()
                        .Search("Name", "Louisiana")
                        .OrderByDescending("NameForSorting")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_7_1
                    IList<Product> results = session
                        .Query<Product, Products_ByUnitsInStock>()
                        .Where(x => x.UnitsInStock > 10)
                        .OrderBy(x => x.Name, OrderingType.AlphaNumeric)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_7_2
                    IList<Product> results = session
                        .Advanced
                        .DocumentQuery<Product, Products_ByUnitsInStock>()
                        .WhereGreaterThan(x => x.UnitsInStock, 10)
                        .OrderBy("Name", OrderingType.AlphaNumeric)
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region sorting_9_1
                    List<Event> results = session
                        .Query<Event, Events_ByCoordinates>()
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .OrderByDistance(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region sorting_9_2
                    List<Event> results = session
                        .Advanced
                        .DocumentQuery<Event, Events_ByCoordinates>()
                        .Spatial(
                            "Coordinates",
                            criteria => criteria.WithinRadius(500, 30, 30))
                        .OrderByDistance(
                            factory => factory.Point(x => x.Latitude, x => x.Longitude), 32.1234, 23.4321)
                        .ToList();
                    #endregion
                }
            }

            using (var documentStore = new DocumentStore())
            {
                /*
                #region custom_1
                PutSortersOperation(params SorterDefinition[] sortersToAdd)
                #endregion

                #region custom_2
                public class SorterDefinition
                {
                    public string Name { get; set; }
                    public string Code { get; set; }
                }
                #endregion
                */

                #region custom_3
                //Assign the code of your new sorter as a `string`
                var sorterCode = "<code of custom sorter>";

                //Create one or more `SorterDefinition` objects
                var customSorterDefinition = new SorterDefinition
                {
                    Name = "MySorter",
                    Code = sorterCode
                };

                //Send the new sorters to the server
                documentStore.Maintenance.Send(new PutSortersOperation(customSorterDefinition));
                #endregion
            }
        }
    }
}
