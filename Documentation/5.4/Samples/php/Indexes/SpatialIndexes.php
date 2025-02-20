<?php

namespace RavenDB\Samples\Indexes;

use RavenDB\Documents\DocumentStore;
use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Documents\Indexes\IndexFieldOptions;
use RavenDB\Documents\Indexes\Spatial\CartesianSpatialOptionsFactory;
use RavenDB\Documents\Indexes\Spatial\GeographySpatialOptionsFactory;
use RavenDB\Documents\Indexes\Spatial\SpatialBounds;
use RavenDB\Documents\Indexes\Spatial\SpatialFieldType;
use RavenDB\Documents\Indexes\Spatial\SpatialOptions;
use RavenDB\Documents\Indexes\Spatial\SpatialRelation;
use RavenDB\Documents\Indexes\Spatial\SpatialSearchStrategy;
use RavenDB\Documents\Indexes\Spatial\SpatialUnits;

# region spatial_index_1
class Event
{
    private ?string $id = null;
    private ?string $name = null;
    private ?float $latitude = null;
    private ?float $longitude = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getLatitude(): ?float
    {
        return $this->latitude;
    }

    public function setLatitude(?float $latitude): void
    {
        $this->latitude = $latitude;
    }

    public function getLongitude(): ?float
    {
        return $this->longitude;
    }

    public function setLongitude(?float $longitude): void
    {
        $this->longitude = $longitude;
    }
}

class Events_ByNameAndCoordinates extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Events.Select(e => new { " .
            "    name = e.name, " .
            "    coordinates = this.CreateSpatialField(((double ? ) e.latitude), ((double ? ) e.longitude)) " .
            "})";
    }
}
# endregion

# region spatial_index_2
class EventWithWKT {
    private ?string $id = null;
    private ?string $name = null;
    private ?string $wkt = null;

    public function getId(): ?string
    {
        return $this->id;
    }

    public function setId(?string $id): void
    {
        $this->id = $id;
    }

    public function getName(): ?string
    {
        return $this->name;
    }

    public function setName(?string $name): void
    {
        $this->name = $name;
    }

    public function getWkt(): ?string
    {
        return $this->wkt;
    }

    public function setWkt(?string $wkt): void
    {
        $this->wkt = $wkt;
    }
}

class EventsWithWKT_ByNameAndWKT extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.EventWithWKTs.Select(e => new { " .
            "    name = e.name, " .
            "    wkt = this.CreateSpatialField(e.wkt) " .
            "})";
    }
}
# endregion

# region spatial_index_3
class Events_ByNameAndCoordinates_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('events', function (e) {
                        return { 
                            Name: e.Name,
                            Coordinates: createSpatialField(e.Latitude, e.Longitude)
                        };
                })"
        ]);
    }
}
# endregion

# region spatial_index_4
class Events_ByNameAndCoordinates_Custom extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "docs.Events.Select(e => new { " .
            "    name = e.name, " .
            "    coordinates = this.CreateSpatialField(((double ? ) e.latitude), ((double ? ) e.longitude)) " .
            "})";

        $this->spatial("coordinates", function($factory) { return $factory->cartesian()->boundingBoxIndex(); });
    }
}
# endregion

# region spatial_index_5
class Events_ByNameAndCoordinates_Custom_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        // Define index fields
        $this->setMaps([
            "map('events', function (e) {
                    return { 
                        Name: e.Name,
                        Coordinates: createSpatialField(e.Latitude, e.Longitude)
                    };
            })"
        ]);


        // Customize index fields
        $options = new IndexFieldOptions();

        $spatialOptions = new SpatialOptions();
        $spatialOptions->setType(SpatialFieldType::cartesian());
        $spatialOptions->setStrategy(SpatialSearchStrategy::boundingBox());
        $options->setSpatial($spatialOptions);

        $this->setFields([
            "Coordinates" => $options
        ]);
    }
}
# endregion

class QuerySpatialIndex
{
    public function samples(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->OpenSession();
            try {
                # region spatial_query_1
                // Define a spatial query on index 'Events_ByNameAndCoordinates'
                /** @var array<Event> $employeesWithinRadius */
                $employeesWithinRadius = $session
                    ->query(Event::class, Events_ByNameAndCoordinates::class)
                     // Call 'Spatial' method
                    ->spatial(
                        // Pass the spatial index-field containing the spatial data
                        "Coordinates",
                        // Set the geographical area in which to search for matching documents
                        // Call 'withinRadius', pass the radius and the center points coordinates
                        function ($criteria) { return $criteria->withinRadius(20, 47.623473, -122.3060097); })
                    ->toList();

                // The query returns all matching Event entities
                // that are located within 20 kilometers radius
                // from point (47.623473 latitude, -122.3060097 longitude).
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->OpenSession();
            try {
                # region spatial_query_2
                // Define a spatial query on index 'Events_ByNameAndCoordinates'
                $employeesWithinRadius = $session->advanced()
                    ->documentQuery(Event::class, Events_ByNameAndCoordinates::class)
                     // Call 'Spatial' method
                    ->spatial(
                        // Pass the spatial index-field containing the spatial data
                        "Coordinates",
                        // Set the geographical area in which to search for matching documents
                        // Call 'WithinRadius', pass the radius and the center points coordinates
                        function($criteria) { return $criteria->withinRadius(20, 47.623473, -122.3060097); })
                    ->toList();

                // The query returns all matching Event entities
                // that are located within 20 kilometers radius
                // from point (47.623473 latitude, -122.3060097 longitude).
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->OpenSession();
            try {
                # region spatial_query_3
                // Define a spatial query on index 'EventsWithWKT_ByNameAndWKT'
                /** @var array<EventWithWKT> $employeesWithinShape */
                $employeesWithinShape = $session
                    ->query(EventWithWKT::class, EventsWithWKT_ByNameAndWKT::class)
                    // Call 'spatial' method
                    ->spatial(
                        // Pass the spatial index-field containing the spatial data
                        "WKT",
                        // Set the geographical search criteria, call 'RelatesToShape'
                        function($criteria) { return $criteria->relatesToShape(
                            // Specify th   e WKT string
                            "POLYGON ((
                                           -118.6527948 32.7114894,
                                           -95.8040242 37.5929338,
                                           -102.8344151 53.3349629,
                                           -127.5286633 48.3485664,
                                           -129.4620208 38.0786067,
                                           -118.7406746 32.7853769,
                                           -118.6527948 32.7114894
                                      ))",
                            // Specify the relation between the WKT shape and the documents spatial data
                            SpatialRelation::within()); })
                    ->toList();

                // The query returns all matching Event entities
                // that are located within the specified polygon.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->OpenSession();
            try {
                # region spatial_query_4
                // Define a spatial query on index 'EventsWithWKT_ByNameAndWKT'
                /** @var array<EventWithWKT> $employeesWithinShape */
                $employeesWithinShape = $session->advanced()
                    ->documentQuery(EventWithWKT::class, EventsWithWKT_ByNameAndWKT::class)
                    // Call 'Spatial' method
                    ->spatial(
                        // Pass the spatial index-field containing the spatial data
                        "WKT",
                        // Set the geographical search criteria, call 'RelatesToShape'
                        function($criteria) { return $criteria->relatesToShape(
                            // Specify the WKT string
                            "POLYGON ((
                                   -118.6527948 32.7114894,
                                   -95.8040242 37.5929338,
                                   -102.8344151 53.3349629,
                                   -127.5286633 48.3485664,
                                   -129.4620208 38.0786067,
                                   -118.7406746 32.7853769,
                                   -118.6527948 32.7114894
                              ))",
                            // Specify the relation between the WKT shape and the documents spatial data
                            SpatialRelation::within()); })
                    ->toList();

                // The query returns all matching Event entities
                // that are located within the specified polygon.
               # endregion
            } finally {
                $session->close();
            }

            $session = $store->OpenSession();
            try {
                # region spatial_query_5
                // Define a spatial query on index 'Events_ByNameAndCoordinates'
                /** @var array<Event> $employeesSortedByDistance */
                $employeesSortedByDistance = $session
                    ->query(Event::class, Events_ByNameAndCoordinates::class)
                     // Filter results by geographical criteria
                    ->spatial(
                        "Coordinates",
                        function($criteria) { return $criteria->withinRadius(20, 47.623473, -122.3060097); })
                     // Sort results, call 'OrderByDistance'
                    ->orderByDistance(
                        // Pass the spatial index-field containing the spatial data
                        "Coordinates",
                        // Sort the results by their distance from this point:
                        47.623473, -122.3060097)
                    ->toList();

                // Return all matching Event entities located within 20 kilometers radius
                // from point (47.623473 latitude, -122.3060097 longitude).

                // Sort the results by their distance from a specified point,
                // the closest results will be listed first.
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->OpenSession();
            try {
                # region spatial_query_6
                // Define a spatial query on index 'Events_ByNameAndCoordinates'
                /** @var array<Event> $employeesSortedByDistance */
                $employeesSortedByDistance = $session->advanced()
                    ->documentQuery(Event::class, Events_ByNameAndCoordinates::class)
                     // Filter results by geographical criteria
                    ->spatial(
                        "Coordinates",
                        function($criteria) { return $criteria->withinRadius(20, 47.623473, -122.3060097); })
                     // Sort results, call 'OrderByDistance'
                    ->orderByDistance(
                        // Pass the spatial index-field containing the spatial data
                        "Coordinates",
                        // Sort the results by their distance from this point:
                        47.623473, -122.3060097)
                    ->toList();

                // Return all matching Event entities located within 20 kilometers radius
                // from point (47.623473 latitude, -122.3060097 longitude).

                // Sort the results by their distance from a specified point,
                // the closest results will be listed first.
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

/*
interface Foo
{
    # region spatial_syntax_1
    object CreateSpatialField(double? lat, double? lng); // Latitude/Longitude coordinates
    object CreateSpatialField(string shapeWkt);          // Shape in WKT string format
    # endregion
}
*/

# region spatial_syntax_2
class SpatialOptionsFactory
{
    public function geography(): GeographySpatialOptionsFactory
    {
        return new GeographySpatialOptionsFactory();
    }

    public function cartesian(): CartesianSpatialOptionsFactory
    {
        return new CartesianSpatialOptionsFactory();
    }
}
# endregion

/*
# region spatial_syntax_3
interface GeographySpatialOptionsFactory
{
    // if $circleRadiusUnits is not set SpatialUnits::kilometers() will be used

    // Default is GeohashPrefixTree strategy with maxTreeLevel set to 9
    public function defaultOptions(?SpatialUnits $circleRadiusUnits = null): SpatialOptions;

    public function boundingBoxIndex(?SpatialUnits $circleRadiusUnits = null): SpatialOptions;

    public function geohashPrefixTreeIndex(int $maxTreeLevel, ?SpatialUnits $circleRadiusUnits = null): SpatialOptions;

    public function quadPrefixTreeIndex(int $maxTreeLevel, ?SpatialUnits $circleRadiusUnits = null): SpatialOptions;
}
# endregion
*/

/*
# region spatial_syntax_4
interface CartesianSpatialOptionsFactory
{
    public function boundingBoxIndex(): SpatialOptions;
    public function quadPrefixTreeIndex(int $maxTreeLevel, SpatialBounds $bounds): SpatialOptions;
}

class SpatialBounds
{
    private float $minX;
    private float $maxX;
    private float $minY;
    private float $maxY;

    // ... getters and setters
}
# endregion
*/




