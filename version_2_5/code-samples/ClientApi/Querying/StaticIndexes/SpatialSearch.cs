using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Indexes;

namespace RavenCodeSamples.ClientApi.Querying.StaticIndexes
{
	namespace Foo
	{
		public interface Foo
		{
			#region spatial_search_0
			object SpatialGenerate(double lat, double lng);

			object SpatialGenerate(string fieldName, double lat, double lng);

			object SpatialGenerate(string fieldName, string shapeWKT);

			object SpatialGenerate(string fieldName, string shapeWKT, SpatialSearchStrategy strategy);

			object SpatialGenerate(string fieldName, string shapeWKT, SpatialSearchStrategy strategy, int maxTreeLevel);

			#endregion

			#region spatial_search_5
			IDocumentQueryCustomization RelatesToShape(string fieldName, string shapeWKT, SpatialRelation rel);

			#endregion
		}

		#region spatial_search_6
		public enum SpatialSearchStrategy
		{
			GeohashPrefixTree,
			QuadPrefixTree,
		}

		#endregion

		#region spatial_search_7
		public enum SpatialRelation
		{
			Within,
			Contains,
			Disjoint,
			Intersects,

			/// <summary>
			/// Does not filter the query, merely sort by the distance
			/// </summary>
			Nearby
		}

		#endregion
	}

	#region spatial_search_1
	public class Event
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}

	#endregion

	#region spatial_search_2
	public class Events_SpatialIndex : AbstractIndexCreationTask<Event>
	{
		public Events_SpatialIndex()
		{
			this.Map = events => from e in events
								 select new
									 {
										 Name = e.Name,
										 __ = SpatialGenerate("Coordinates", e.Latitude, e.Longitude)
									 };
		}
	}

	#endregion

	public class SpatialSearch : CodeSampleBase
	{
		public void Sample()
		{
			using (var store = NewDocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region spatial_search_3
					session.Query<Event>()
					       .Customize(x => x.WithinRadiusOf(
						       fieldName: "Coordinates",
						       radius: 10,
						       latitude: 32.1234,
						       longitude: 23.4321))
					       .ToList();
					#endregion

					#region spatial_search_8
					session.Advanced.LuceneQuery<Event>()
						   .WithinRadiusOf(fieldName: "Coordinates", radius: 10, latitude: 32.1234, longitude: 23.4321)
						   .ToList();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region spatial_search_4
					session.Query<Event>()
							.Customize(x => x.RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within))
							.ToList();

					#endregion

					#region spatial_search_9
					session.Advanced.LuceneQuery<Event>()
						   .RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within)
						   .ToList();

					#endregion
				}
			}
		}
	}
}