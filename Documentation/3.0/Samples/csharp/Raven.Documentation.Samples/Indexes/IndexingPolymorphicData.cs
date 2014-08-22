using System.Collections.Generic;
using System.Linq;

using Raven.Client.Document;
using Raven.Client.Indexes;

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
					var results = session
						.Advanced
						.DocumentQuery<object, Animals_ByName>()
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
			var store = new DocumentStore()
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