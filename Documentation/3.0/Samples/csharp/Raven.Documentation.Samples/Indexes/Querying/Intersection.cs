using System.Collections.Generic;
using System.Linq;

using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Raven.Documentation.CodeSamples.Indexes.Querying
{
	#region intersection_1
	public class TShirt
	{
		public string Id { get; set; }

		public int ReleaseYear { get; set; }

		public string Manufacturer { get; set; }

		public List<TShirtType> Types { get; set; }
	}

	public class TShirtType
	{
		public string Color { get; set; }

		public string Size { get; set; }
	}

	#endregion

	#region intersection_2
	public class TShirtIndex : AbstractIndexCreationTask<TShirt>
	{
		public TShirtIndex()
		{
			this.Map = tshirts => from tshirt in tshirts
								  from type in tshirt.Types
								  select new
								  {
									  Manufacturer = tshirt.Manufacturer,
									  Color = type.Color,
									  Size = type.Size,
									  ReleaseYear = tshirt.ReleaseYear
								  };
		}
	}

	#endregion

	public class Intersection
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region intersection_3
					session.Store(new TShirt
					{
						Id = "tshirts/1",
						Manufacturer = "Raven",
						ReleaseYear = 2010,
						Types = new List<TShirtType>
								{
									new TShirtType {Color = "Blue", Size = "Small"},
									new TShirtType {Color = "Black", Size = "Small"},
									new TShirtType {Color = "Black", Size = "Medium"},
									new TShirtType {Color = "Gray", Size = "Large"}
								}
					});

					session.Store(new TShirt
					{
						Id = "tshirts/2",
						Manufacturer = "Wolf",
						ReleaseYear = 2011,
						Types = new List<TShirtType>
								{
									new TShirtType { Color = "Blue",  Size = "Small" },                                    
									new TShirtType { Color = "Black", Size = "Large" },
									new TShirtType { Color = "Gray",  Size = "Medium" }
								}
					});

					session.Store(new TShirt
					{
						Id = "tshirts/3",
						Manufacturer = "Raven",
						ReleaseYear = 2011,
						Types = new List<TShirtType>
								{
									new TShirtType { Color = "Yellow",  Size = "Small" },
									new TShirtType { Color = "Gray",  Size = "Large" }
								}
					});

					session.Store(new TShirt
					{
						Id = "tshirts/4",
						Manufacturer = "Raven",
						ReleaseYear = 2012,
						Types = new List<TShirtType>
								{
									new TShirtType { Color = "Blue",  Size = "Small" },
									new TShirtType { Color = "Gray",  Size = "Large" }
								}
					});

					#endregion

					#region intersection_4
					session.Query<TShirt>("TShirtIndex")
						   .Where(x => x.Manufacturer == "Raven")
						   .Intersect()
						   .Where(x => x.Types.Any(t => t.Color == "Blue" && t.Size == "Small"))
						   .Intersect()
						   .Where(x => x.Types.Any(t => t.Color == "Gray" && t.Size == "Large"))
						   .ToList();

					#endregion

					#region intersection_5
					session.Advanced.DocumentQuery<TShirt>("TShirtIndex")
						   .Where("Manufacturer:Raven INTERSECT Color:Blue AND Size:Small INTERSECT Color:Gray AND Size:Large")
						   .ToList();

					#endregion
				}
			}
		}
	}
}