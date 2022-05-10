using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Indexes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
    public class MultiMap
    {
        #region multi_map_1
        public class Dog : Animal
        {

        }
        #endregion

        #region multi_map_2
        public class Cat : Animal
        {

        }
        #endregion

        #region multi_map_3
        public abstract class Animal : IAnimal
        {
            public string Name { get; set; }
        }
        #endregion

        #region multi_map_6
        public interface IAnimal
        {
            string Name { get; set; }
        }
        #endregion

        #region multi_map_4
        public class Animals_ByName : AbstractMultiMapIndexCreationTask
        {
            public Animals_ByName()
            {
                AddMap<Cat>(cats => from c in cats select new { c.Name });

                AddMap<Dog>(dogs => from d in dogs select new { d.Name });
            }
        }
        #endregion

        #region multi_map_5
        public class Animals_ByName_ForAll : AbstractMultiMapIndexCreationTask
        {
            public Animals_ByName_ForAll()
            {
                AddMapForAll<Animal>(parents => from p in parents select new { p.Name });
            }
        }
        #endregion

        #region multi_map_1_0

        public class Smart_Search : AbstractMultiMapIndexCreationTask<Smart_Search.Result>
        {
            public class Result
            {
                public string Id { get; set; }

                public string DisplayName { get; set; }

                public object Collection { get; set; }

                public string[] Content { get; set; }
            }

            public class Projection
            {
                public string Id { get; set; }

                public string DisplayName { get; set; }

                public string Collection { get; set; }
            }

            public Smart_Search()
            {
                AddMap<Company>(companies => from c in companies
                    select new Result
                    {
                        Id = c.Id,
                        Content = new[]
                        {
                            c.Name
                        },
                        DisplayName = c.Name,
                        Collection = MetadataFor(c)["@collection"]
                    });

                AddMap<Product>(products => from p in products
                    select new Result
                    {
                        Id = p.Id,
                        Content = new[]
                        {
                            p.Name
                        },
                        DisplayName = p.Name,
                        Collection = MetadataFor(p)["@collection"]
                    });

                AddMap<Employee>(employees => from e in employees
                    select new Result
                    {
                        Id = e.Id,
                        Content = new[]
                        {
                            e.FirstName,
                            e.LastName
                        },
                        DisplayName = e.FirstName + " " + e.LastName,
                        Collection = MetadataFor(e)["@collection"]
                    });

                // mark 'Content' field as analyzed which enables full text search operations
                Index(x => x.Content, FieldIndexing.Search);

                // storing fields so when projection (e.g. ProjectInto)
                // requests only those fields
                // then data will come from index only, not from storage
                Store(x => x.Id, FieldStorage.Yes);
                Store(x => x.DisplayName, FieldStorage.Yes);
                Store(x => x.Collection, FieldStorage.Yes);
            }
        }
        #endregion

        public MultiMap()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region multi_map_7
                    IList<IAnimal> results = session
                        .Query<IAnimal, Animals_ByName>()
                        .Where(x => x.Name == "Mitzy")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region multi_map_8
                    IList<IAnimal> results = session
                        .Advanced
                        .DocumentQuery<IAnimal, Animals_ByName>()
                        .WhereEquals(x => x.Name, "Mitzy")
                        .ToList();
                    #endregion
                }
            }

            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region multi_map_1_1
                    IList<Smart_Search.Projection> results = session
                        .Query<Smart_Search.Result, Smart_Search>()
                        .Search(x => x.Content, "Lau*")
                        .ProjectInto<Smart_Search.Projection>()
                        .ToList();

                    foreach (Smart_Search.Projection result in results)
                    {
                        Console.WriteLine(result.Collection + ": " + result.DisplayName);
                        // Companies: Laughing Bacchus Wine Cellars
                        // Products: Laughing Lumberjack Lager
                        // Employees: Laura Callahan
                    }
                    #endregion
                }
            }
        }
    }
}
