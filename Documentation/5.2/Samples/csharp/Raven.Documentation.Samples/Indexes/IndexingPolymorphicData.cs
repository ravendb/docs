using System.Collections.Generic;
using System.Linq;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;

namespace Raven.Documentation.Samples.Indexes
{
    public class IndexingPolymorphicData
    {
        #region multi_map_1
        public class Animals_ByName : AbstractMultiMapIndexCreationTask
        {
            public Animals_ByName()
            {
                AddMap<Cat>(cats => from c in cats select new { c.Name });

                AddMap<Dog>(dogs => from d in dogs select new { d.Name });
            }
        }
        #endregion

        public void MultiMapIndexes()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region multi_map_2
                    List<IAnimal> results = session
                        .Advanced
                        .DocumentQuery<IAnimal, Animals_ByName>()
                        .WhereEquals("Name", "Mitzy")
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region multi_map_3
                    IList<IAnimal> results = session
                        .Query<IAnimal, Animals_ByName>()
                        .Where(x => x.Name == "Mitzy")
                        .ToList();
                    #endregion
                }
            }
        }

        public void OtherWays()
        {
            #region other_ways_1
            DocumentStore store = new DocumentStore()
            {
                Conventions =
                {
                    FindCollectionName = type =>
                    {
                        if (typeof(Animal).IsAssignableFrom(type))
                            return "Animals";
                        return DocumentConventions.DefaultGetCollectionName(type);
                    }
                }
            };
            #endregion
        }

        public interface IAnimal
        {
            string Name { get; set; }
        }

        private abstract class Animal : IAnimal
        {
            public string Name { get; set; }
        }

        private class Cat : Animal
        {
        }

        private class Dog : Animal
        {
        }
    }
}
