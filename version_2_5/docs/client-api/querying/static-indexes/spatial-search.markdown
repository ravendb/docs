#### Spatial Search

To support the ability to retrieve the data based on spatial coordinates, the spatial search have been introduced.

##### Creating Indexes

To take an advantage of spatial search, first we need to create an index with spatial field. To mark field as a spatial field, we need to use `SpatialGenerate` method

	object SpatialGenerate(double lat, double lng);
	 
	object SpatialGenerate(string fieldName, double lat, double lng);
	 
	object SpatialGenerate(string fieldName, string shapeWKT);
	 
	object SpatialGenerate(string fieldName, string shapeWKT, SpatialSearchStrategy strategy);
	 
	object SpatialGenerate(string fieldName, string shapeWKT, SpatialSearchStrategy strategy, int maxTreeLevel);

	public enum SpatialSearchStrategy
	{
	    GeohashPrefixTree,
	    QuadPrefixTree,
	}

where:   

*	**fieldName** is a name of the field containing the shape to use for filtering (if the overload with no `fieldName` is used, then the name is set to default value: `__spatial`)          
*	**lat/lng** are latitude/longitude coordinates   
*	**shapeWKT** is a shape in [WKT](http://en.wikipedia.org/wiki/Well-known_text) format    
*	**strategy** is a spatial search strategy
*	**maxTreeLevel** is a integer that indicates the maximum number of levels to be used in `PrefixTree` and controls the precision of shape representation   

In our example we will use `Event` class and very simple index defined below.

	public class Event
	{
	    public string Id { get; set; }
	 
	    public string Name { get; set; }
	 
	    public double Latitude { get; set; }
	 
	    public double Longitude { get; set; }
	}

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

{NOTE `GeohashPrefixTree` is a default `SpatialSearchStrategy`. Doing any changes to the strategy after index has been created will trigger re-indexation process. /}

##### Radius search

The most basic usage and probably most common one is to search for all points or shapes within provided distance from the given center point. To perform this search we will use `WithinRadiusOf` method that is a part of query customizations.

	session.Query<Event>()
	       .Customize(x => x.WithinRadiusOf(
	           fieldName: "Coordinates",
	           radius: 10,
	           latitude: 32.1234,
	           longitude: 23.4321))
	       .ToList();

The method can be used also when using `LuceneQuery`.

	session.Advanced.LuceneQuery<Event>()
	       .WithinRadiusOf(fieldName: "Coordinates", radius: 10, latitude: 32.1234, longitude: 23.4321)
	       .ToList();

##### Advanced search

The `WithinRadiusOf` method is a wrapper around `RelatesToShape` method.

	IDocumentQueryCustomization RelatesToShape(string fieldName, string shapeWKT, SpatialRelation rel);

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

where first parameter is a name of the field containing the shape to use for filtering, next one is a shape in [WKT](http://en.wikipedia.org/wiki/Well-known_text) format and the last one is a spatial relation type.

So to perform a radius search from the above example and use `RelatesToShape` method, we do as follows

	session.Query<Event>()
        .Customize(x => x.RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within))
        .ToList();

or when we want to use `LuceneQuery` then

	session.Advanced.LuceneQuery<Event>()
	       .RelatesToShape("Coordinates", "Circle(32.1234, 23.4321, d=10.0000)", SpatialRelation.Within)
	       .ToList();

{WARNING From RavenDB 2.0 the distance is measured in **kilometers** in contrast to the miles used in previous versions. /}
