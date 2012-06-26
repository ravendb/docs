using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Linq;


namespace RavenCodeSamples.Consumer
{
	#region intersectPOCO
	public class TShirt
	{
		public String Id { get; set; }
		public String Name { get; set; }
		public int BarcodeNumber { get; set; }
		public List<TShirtType> Types { get; set; }
	}

	public class TShirtType
	{
		public String Colour { get; set; }
		public String Size { get; set; }
	}
	#endregion

	class IntersectionQueries : CodeSampleBase
	{
		public void IntersectionQueriesSample()
		{
			using (var documentStore = NewDocumentStore())
			{
				using (var session = documentStore.OpenSession())
				{
					#region intersectIndex
					
					documentStore.DatabaseCommands.PutIndex("TShirtNested",
											new IndexDefinition
											{
												Map =
												@"from s in docs.TShirts
														from t in s.Types
														select new 
														{ 
															s.Name, 
															Types_Colour = t.Colour, 
															Types_Size = t.Size, 
															s.BarcodeNumber 
														}",
												SortOptions = new Dictionary<String, SortOptions>
												{
													{ "BarcodeNumber", SortOptions.Int }
												}
											});
					
					#endregion

					#region intersectQuery

					var shirts = session.Query<TShirt>("TShirtNested")
						.OrderBy(x => x.BarcodeNumber)
						.Where(x => x.Name == "Wolf")
						.Intersect()
						.Where(x => x.Types.Any(t => t.Colour == "Blue" && t.Size == "Small"))
						.Intersect()
						.Where(x => x.Types.Any(t => t.Colour == "Gray" && t.Size == "Large"))
						.ToList();

					#endregion
				}
			}
		}
	}
}
