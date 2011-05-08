# Spatial search

RavenDB supports performing queries based on spatial coordinates. That is, searching for locations which are within a specified radius from an origin.

Let's assume we build a restaurant-finder application, and to that end we store restaurant entities using the following structure:

{CODE spatial0@Consumer\Spatial.cs /}

To allow spatial searches we will need to define an index like so:

{CODE spatial1@Consumer\Spatial.cs /}

The key part is the call to `SpatialIndex.Generate()`. The field name assigned to it will be ignored, so we recommend using the underscore letter to mark it, which is a conventions to an unimportant variable.

This index gives us the ability to make spatial searches, using the following code:

{CODE spatial1@Consumer\Spatial.cs /}

This will return all the restaurants within 5 miles radius with a rating of 4 or more.