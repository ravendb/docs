using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raven.Tests.Helpers;
using Xunit;

namespace RavenTestSample
{
	#region RavenTestSample1
	class RavenTestSample : RavenTestBase
	{
		[Fact]
		public void ThisIsMyTest()
		{
			//Write you test here
			//Don't forget to use Asserts
		}
	}
	#endregion

	#region RavenTestSample2
	public class CanIndexOnNull : RavenTestBase
	{
		[Fact]
		public void CanIndexOnMissingProps()
		{
			using (var store = NewDocumentStore())
			{
				store.DatabaseCommands.PutIndex("test",
												new IndexDefinition
												{
													Map = "from doc in docs select new { doc.Type, doc.Houses.Wheels} "
												});

				for (int i = 0; i < 50; i++)
				{
					store.DatabaseCommands.Put("item/" + i, null,
											   new RavenJObject { { "Type", "Car" } }, new RavenJObject());
				}

				using (var s = store.OpenSession())
				{
					s.Advanced.LuceneQuery<dynamic>("test")
						.WaitForNonStaleResults()
						.WhereGreaterThan("Wheels_Range", 4)
						.ToArray();
				}

				Assert.Empty(store.DocumentDatabase.Statistics.Errors);
			}
		}
	}
	#endregion
}
