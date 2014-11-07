using System.Linq;
using Raven.Client.Indexes;
using RavenDBSamples.BaseForSamples;

namespace RavenDBSamples.SpatialSearch
{
	public class SpatialSearch : SampleBase
	{
		public void Spatial()
		{
			using (var session = DocumentStore.OpenSession())
			{
				var matchingResturants =
					session.Advanced.LuceneQuery<Restaurant>("Restaurants/ByRatingAndLocation")
					       .WhereGreaterThanOrEqual("Rating", 4)
					       .WithinRadiusOf(radius: 5, latitude: 38.9103000, longitude: -77.3942)
					       .ToList();
			}
		}
	}

	public class Restaurants_ByRatingAndLocation : AbstractIndexCreationTask<Restaurant>
	{
		public Restaurants_ByRatingAndLocation()
		{
			Map = restaurants => from r in restaurants
									  select new { r.Rating, _ = SpatialIndex.Generate(r.Latitude, r.Longitude) };
		}
	}
}
