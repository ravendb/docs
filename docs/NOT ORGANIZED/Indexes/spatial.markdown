#Spatial search support

RavenDB support spatial searches, or geo location searches, using its indexing system.

Let us assume that we have a set of documents similar to the following one:

    { // doc id restaurants/1293
        "Name": "Jimmy's Old Town Tavern", 
        "Longtitude": 38.9690000,
        "Langtitude":  -77.3862000,
        "Rating": 5,
        "Reviews": [ ... ]
    }

We can define an index that will allow spatial searches using the following syntax:

    from r in docs.Restaurants
    select new { r.Rating,  _ = SpatialIndex.Generate(r.Latitude, r.Longitude) }

The key part is the SpatialIndex.Generate() call. Note that the field name you assigned to it will be ignored, so we recommend using the underscore letter to mark it, which is a conventions to an unimportant variable.

This index gives us the ability to make spatial searches, using this code:

    var matchingResturants  = 
        session.Advanced.LuceneQuery<Resturant>("Restaurants/ByRatingAndLocaton")
            .WhereGreaterThanOrEquals("Rating", 4)
            .WithinRadiusOf(radius: 5, latitude: 38.9103000, longtitude: -77.3942)
            .ToList();

This will give us all the restaurants within 5 miles radius with a rating of 4 or more.
