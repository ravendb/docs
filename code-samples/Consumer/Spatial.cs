using System.Linq;
using Raven.Client.Indexes;

namespace RavenCodeSamples.Consumer
{
	public class Spatial : CodeSampleBase
	{
		#region spatial0
		public class Restaurant
		{
			public string Name { get; set; }
			public double Longitude { get; set; }
			public double Latitude { get; set; }
			public short Rating { get; set; }
		}
		#endregion

		public class Restaurants_ByRatingAndLocation : AbstractIndexCreationTask<Restaurant>
		{
			public Restaurants_ByRatingAndLocation()
			{
				#region spatial1
				Map = restaurants => from r in restaurants
				                     select new {r.Rating, _ = SpatialIndex.Generate(r.Latitude, r.Longitude)};
				#endregion
			}
		}

		public void BasicSpatialSearch()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region spatial2
					var matchingResturants =
						session.Advanced.LuceneQuery<Restaurant>("Restaurants/ByRatingAndLocaton")
							.WhereGreaterThanOrEqual("Rating", 4)
							.WithinRadiusOf(radius: 5, latitude: 38.9103000, longitude: -77.3942)
							.ToList();
					#endregion
				}
			}
		}
	}
}
