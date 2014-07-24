using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Spatial;

namespace Raven.Documentation.CodeSamples.Indexes
{
	namespace Foo
	{

		#region spatial_search_enhancements_3
		public class SpatialOptionsFactory
		{
			public GeographySpatialOptionsFactory Geography;

			public CartesianSpatialOptionsFactory Cartesian;
		}

		#endregion

		public interface GeographySpatialOptionsFactory
		{
			#region spatial_search_enhancements_4
			// GeohashPrefixTree strategy with maxTreeLevel set to 9
			SpatialOptions Default(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

			SpatialOptions BoundingBoxIndex(SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

			SpatialOptions GeohashPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

			SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel, SpatialUnits circleRadiusUnits = SpatialUnits.Kilometers);

			#endregion
		}

		public interface SpatialCriteriaFactory
		{
			#region spatial_search_enhancements_a
			SpatialCriteria RelatesToShape(object shape, SpatialRelation relation);

			SpatialCriteria Intersects(object shape);

			SpatialCriteria Contains(object shape);

			SpatialCriteria Disjoint(object shape);

			SpatialCriteria Within(object shape);

			SpatialCriteria WithinRadiusOf(double radius, double x, double y);

			#endregion
		}

		public interface CartesianSpatialOptionsFactory
		{
			#region spatial_search_enhancements_5
			SpatialOptions BoundingBoxIndex();

			SpatialOptions QuadPrefixTreeIndex(int maxTreeLevel, SpatialBounds bounds);

			#endregion
		}

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

	public class SpatialDoc
	{
		public object Shape { get; set; }

		public object Point { get; set; }
	}

	#region spatial_search_enhancements_8
	public class SpatialDoc_Index : AbstractIndexCreationTask<SpatialDoc>
	{
		public SpatialDoc_Index()
		{
			Map = docs => from spatial in docs
						  select new
						  {
							  Shape = spatial.Shape,
							  Point = spatial.Point
						  };

			Spatial(x => x.Shape, options => options.Geography.Default());
			Spatial(x => x.Point, options => options.Cartesian.BoundingBoxIndex());
		}
	}

	#endregion

	#region spatial_search_enhancements_1
	public class EventWithWKT
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string WKT { get; set; }
	}

	#endregion

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
			Map = events => from e in events
							select new
							{
								Name = e.Name,
								__ = SpatialGenerate("Coordinates", e.Latitude, e.Longitude)
							};
		}
	}

	#endregion

	#region spatial_search_enhancements_2
	public class EventsWithWKT_SpatialIndex : AbstractIndexCreationTask<EventWithWKT>
	{
		public EventsWithWKT_SpatialIndex()
		{
			Map = events => from e in events
							select new
							{
								Name = e.Name,
								WKT = e.WKT
							};

			Spatial(x => x.WKT, options => options.Geography.Default());
		}
	}

	#endregion

	public class SpatialIndexes
	{
		public void Sample()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region spatial_search_3
					session.Query<Event, Events_SpatialIndex>()
						   .Customize(x => x.WithinRadiusOf(
							   fieldName: "Coordinates",
							   radius: 10,
							   latitude: 32.1234,
							   longitude: 23.4321))
						   .ToList();
					#endregion

					#region spatial_search_8
					session.Advanced.LuceneQuery<Event, Events_SpatialIndex>()
						   .WithinRadiusOf(fieldName: "Coordinates", radius: 10, latitude: 32.1234, longitude: 23.4321)
						   .ToList();

					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region spatial_search_4
					session.Query<Event, Events_SpatialIndex>()
							.Customize(x => x.RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within))
							.ToList();

					#endregion

					#region spatial_search_9
					session.Advanced.LuceneQuery<Event, Events_SpatialIndex>()
						   .RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within)
						   .ToList();

					#endregion

					#region spatial_search_enhancements_6
					var point = new
					{
						type = "Point",
						coordinates = new[] { -10d, 45d }
					};

					session.Store(new SpatialDoc { Shape = point });

					#endregion

					#region spatial_search_enhancements_7
					session.Store(new SpatialDoc { Point = new[] { -10d, 45d } });
					session.Store(new SpatialDoc { Point = new { X = -10d, Y = 45d } });
					session.Store(new SpatialDoc { Point = new { Latitude = 45d, Longitude = -10d } });
					session.Store(new SpatialDoc { Point = new { lat = 45d, lon = -10d } });
					session.Store(new SpatialDoc { Point = new { lat = 45d, lng = -10d } });
					session.Store(new SpatialDoc { Point = new { Lat = 45d, Long = -10d } });
					session.Store(new SpatialDoc { Point = "geo:45.0,-10.0;u=2.0" }); // Geo URI

					#endregion

					object someWktShape = null;

					#region spatial_search_enhancements_9
					session.Query<SpatialDoc, SpatialDoc_Index>()
						.Spatial(x => x.Shape, criteria => criteria.WithinRadiusOf(500, 30, 30))
						.ToList();

					session.Query<SpatialDoc, SpatialDoc_Index>()
						.Spatial(x => x.Shape, criteria => criteria.Intersects(someWktShape))
						.ToList();

					#endregion
				}
			}
		}
	}
}