using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes
{
	public class IndexingPolymorphicData
	{
		#region multi_map_1
		public class AnimalsIndex : AbstractMultiMapIndexCreationTask
		{
			public AnimalsIndex()
			{
				AddMap<Cat>(cats => from c in cats
									select new { c.Name });

				AddMap<Dog>(dogs => from d in dogs
									select new { d.Name });
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
					var results = session.Advanced.LuceneQuery<object>("AnimalsIndex").WhereEquals("Name", "Mitzy");

					#endregion

					#region multi_map_3
					session.Query<IAnimal>("AnimalsIndex").Where(x => x.Name == "Mitzy");

					#endregion
				}
			}
		}

		public void OtherWays()
		{
			#region other_ways_1
			var documentStore = new DocumentStore()
			{
				Conventions =
				{
					FindTypeTagName = type =>
					{
						if (typeof(Animal).IsAssignableFrom(type))
							return "Animals";
						return DocumentConvention.DefaultTypeTagName(type);
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