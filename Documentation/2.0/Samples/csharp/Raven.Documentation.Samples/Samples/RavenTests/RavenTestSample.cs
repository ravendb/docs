using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Raven.Client.Indexes;
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
	public class IndexTest : RavenTestBase
	{
		[Fact]
		public void CanIndexAndQuery()
		{
			using (var store = NewDocumentStore())
			{
				new SampleData_Index().Execute(store);

				using (var session = store.OpenSession())
				{
					session.Store(new SampleData
					{
						Name = "RavenDB"
					});

					session.SaveChanges();
				}

				using (var session = store.OpenSession())
				{
					var result = session.Query<SampleData, SampleData_Index>()
						.Customize(customization => customization.WaitForNonStaleResultsAsOfNow())
						.FirstOrDefault();

					Assert.Equal(result.Name, "RavenDB");
				}
			}
		}
	}

	public class SampleData
	{
		public string Name { get; set; }
	}

	public class SampleData_Index : AbstractIndexCreationTask<SampleData>
	{
		public SampleData_Index()
		{
			Map = docs => from doc in docs
						  select new
						  {
							  doc.Name
						  };
		}
	}
	#endregion
}
