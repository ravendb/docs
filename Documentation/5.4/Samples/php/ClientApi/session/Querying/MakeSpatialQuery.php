<?php


use RavenDB\Constants\DocumentsIndexingSpatial;
use RavenDB\Documents\Indexes\Spatial\SpatialRelation;
use RavenDB\Documents\Indexes\Spatial\SpatialUnits;
use RavenDB\Documents\Queries\Spatial\DynamicSpatialField;
use RavenDB\Documents\Queries\Spatial\PointField;
use RavenDB\Documents\Queries\Spatial\SpatialCriteria;
use RavenDB\Documents\Queries\Spatial\SpatialCriteriaFactory;
use RavenDB\Documents\Session\DocumentQueryInterface;
use RavenDB\Samples\Infrastructure\DocumentStoreHolder;

interface FooInterface {
    //region spatial_1
    public function spatial(string|DynamicSpatialField $field, Closure $clause): DocumentQueryInterface;

    // Usage
    //    public function spatial(string $fieldName, function(SpatialCriteriaFactory $x) {...})
    //    public function spatial(DynamicSpatialField $field, function(SpatialCriteriaFactory $x) {...})
    //endregion

    /*
    //region spatial_2
    $field = new PointField(?string $latitude, ?string $longitude);
    $field = new WktField(?string $wkt);
    //endregion
    */

    //region spatial_3
    public function relatesToShape(?string $shapeWkt, ?SpatialRelation $relation, ?SpatialUnits $units = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;

    public function intersects(?string $shapeWkt, ?SpatialUnits $units = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;

    public function contains(?string $shapeWkt, ?SpatialUnits $units = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;

    public function disjoint(?string $shapeWkt, ?SpatialUnits $units = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;

    public function within(?string $shapeWkt, ?SpatialUnits $units = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;

    public function withinRadius(float $radius, float $latitude, float $longitude, ?SpatialUnits $radiusUnits = null, float $distErrorPercent = DocumentsIndexingSpatial::DEFAULT_DISTANCE_ERROR_PCT): SpatialCriteria;
    //endregion

    //region spatial_6
    function orderByDistance(DynamicSpatialField|string $field, float|string $latitudeOrShapeWkt, ?float $longitude = null, float $roundFactor = 0): DocumentQueryInterface;

    // Usage
    //    orderByDistance(DynamicSpatialField $field, float $latitude, float $longitude);
    //    orderByDistance(DynamicSpatialField $field, ?string $shapeWkt);
    //    orderByDistance(?string $fieldName, float $latitude, float $longitude);
    //    orderByDistance(?string $fieldName, ?string $shapeWkt);
    //endregion

    //region spatial_8
    function orderByDistanceDescending(DynamicSpatialField|string $field, float|string $latitudeOrShapeWkt, ?float $longitude = null, float $roundFactor = 0): DocumentQueryInterface;

    // Usage
    //    orderByDistanceDescending(?DynamicSpatialField $field, float $latitude, float $longitude);
    //    orderByDistanceDescending(?DynamicSpatialField $field, ?string $shapeWkt);
    //    orderByDistanceDescending(?string $fieldName, float $latitude, float $longitude);
    //    orderByDistanceDescending(?string $fieldName, ?string $shapeWkt);
    //endregion
    }


class HowToQuerySpatialIndex
{
    public function examples(): void
    {
        $store = DocumentStoreHolder::getStore();
        try {
            $session = $store->openSession();
            try {
                //region spatial_4
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                /** @var array<House> $results */
                $results = $session
                    ->query(House::class)
                    ->spatial(
                        new PointField("latitude", "longitude"),
                        function($f) { $f->withinRadius(10, 32.1234, 23.4321); }
                    )
                    ->toList();
                //endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                //region spatial_5
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // this equals to WithinRadius(10, 32.1234, 23.4321)
                /** @var array<House> $results */
                $results = $session
                    ->query(House::class)
                    ->spatial(
                        new PointField("latitude", "longitude"),
                        function($f) { $f->relatesToShape("Circle(32.1234 23.4321 d=10.0000)", SpatialRelation::within()); }
                    )
                    ->toList();
                //endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                //region spatial_7
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                /** @var array<House> $results */
                $results = $session
                    ->query(House::class)
                    ->spatial(
                        new PointField("latitude", "longitude"),
                        function($f) { $f->withinRadius(10, 32.1234, 23.4321); }
                    )
                    ->orderByDistance(
                        new PointField("latitude", "longtude"),
                        32.12324, 23.4321)
                    ->toList();
                //endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                //region spatial_9
                // return all matching entities
                // within 10 kilometers radius
                // from 32.1234 latitude and 23.4321 longitude coordinates
                // sort results by distance from 32.1234 latitude and 23.4321 longitude point
                /** @var array<House> $results */
                $results = $session
                    ->query(House::class)
                    ->spatial(
                        new PointField("latitude", "longitude"),
                        function($f) { $f->withinRadius(10, 32.1234, 23.4321); }
                    )
                    ->orderByDistanceDescending(
                        new PointField("latitude", "longtude"),
                        32.12324, 23.4321)
                    ->toList();
                //endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

class House {
    private ?string $name = null;
    private ?float $latitude = null;
    private ?float $longitude = null;

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