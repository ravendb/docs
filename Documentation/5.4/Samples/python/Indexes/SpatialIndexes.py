from __future__ import annotations
from abc import ABC, abstractmethod
from typing import Callable

from ravendb import AbstractIndexCreationTask, IndexFieldOptions, DynamicSpatialField
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask
from ravendb.documents.indexes.spatial.configuration import (
    SpatialOptions,
    SpatialFieldType,
    SpatialSearchStrategy,
    SpatialRelation,
    SpatialUnits,
)

from examples_base import ExampleBase


# region spatial_index_1
# Define an index with a spatial field
class Events_ByNameAndCoordinates(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        # Call 'CreateSpatialField' to create a spatial index-field
        # Field 'coordinates' will be composed of lat & lng supplied from the document
        self.map = (
            "from e in docs.Events select new {"
            "    name = e.name,"
            "    coordinates = CreateSpatialField(e.latitude, e.longitude)"
            "}"
        )
        # Documents can be retrieved
        # by making a spatial query on the 'coordinates' index-field


class Event:
    def __init__(self, Id: str = None, name: str = None, latitude: float = None, longitude: float = None):
        self.Id = Id
        self.name = name
        self.latitude = latitude
        self.longitude = longitude


# endregion


# region spatial_index_2
# Define an index with a spatial field
class EventsWithWKT_ByNameAndWKT(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from e in docs.Events select new {" "    name = e.name," "    WKT = CreateSpatialField(e.WKT)" "}"


class EventWithWKT:
    def __init__(self, Id: str = None, name: str = None, WKT: str = None):
        self.Id = Id
        self.name = name
        self.WKT = WKT


# endregion
# region spatial_index_3
class Events_ByNameAndCoordinates_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('events', function (e) {
                    return { 
                        name: e.name,
                        coordinates: createSpatialField(e.latitude, e.longitude)
                    };
            })
            """
        }


# endregion
# region spatial_index_4
class Events_ByNameAndCoordinates_Custom(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = "from e in docs.Events select new { name = e.name, coordinates = CreateSpatialField(e.latitude, e.longitude)}"

        # Set the spatial indexing strategy for the spatial field 'coordinates'
        self._spatial("coordinates", lambda factory: factory.cartesian.bounding_box_index())


# endregion
# region spatial_index_5
class Event_ByNameAndCoordinates_Custom_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = """
        map('events', function (e) {
                return { 
                    Name: e.Name,
                    Coordinates: createSpatialField(e.Latitude, e.Longitude)
                };
        })
        """

        # Customize index fields
        self.fields = {
            "coordinates": IndexFieldOptions(
                spatial=SpatialOptions(
                    field_type=SpatialFieldType.CARTESIAN, strategy=SpatialSearchStrategy.BOUNDING_BOX
                )
            )
        }


# endregion


class QuerySpatialIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_index(self):
        with self.embedded_server.get_document_store("QuerySpatialIndex") as store:
            with store.open_session() as session:
                # region spatial_query_1
                # Define a spatail query on index 'Events_ByNameAndCoordinates'
                employees_within_radius = list(
                    session.query_index_type(Events_ByNameAndCoordinates, Event)
                    # Call 'spatial' method
                    .spatial(
                        # Pass the spatial index-field containing the spatial data
                        "coordinates",
                        # Set the geographical area in which to search for matching documents
                        # Call 'within_radius', pass the radius and the center points coordinates
                        lambda criteria: criteria.within_radius(20, 47.623473, -122.3060097),
                    )
                )

                # The query returns all matching Event entities
                # that are located within 20 kilometers radius
                # from point (47.623473 latitude, -122.3060097 longitude)
                # endregion

                # region spatial_query_3
                # Define a spatial query on index 'EventsWithWKT_ByNameAndWKT'
                employees_within_radius = list(
                    session.query_index_type(EventsWithWKT_ByNameAndWKT, EventWithWKT)
                    # Call 'spatial' method
                    .spatial(
                        # Pass the spatial index-field containing the spatial data,
                        "WKT",
                        # Set the geographical search criteria, call 'relates_to_shape'
                        lambda criteria: criteria.relates_to_shape(
                            # Specify the WKT string
                            shape_wkt="""POLYGON ((
                                               -118.6527948 32.7114894,
                                               -95.8040242 37.5929338,
                                               -102.8344151 53.3349629,
                                               -127.5286633 48.3485664,
                                               -129.4620208 38.0786067,
                                               -118.7406746 32.7853769,
                                               -118.6527948 32.7114894
                                          ))""",
                            # Specify the relation between the WKT shape and the documents spatial data
                            relation=SpatialRelation.WITHIN,
                        ),
                    )
                )
                # The query returns all matching Event properties
                # that are located within the specified polygon.
                # endregion

                # region spatial_query_5
                # Define a spatial query on index 'Events_ByNameAndCoordinates'
                employees_sorted_by_distance = list(
                    session.query_index_type(Events_ByNameAndCoordinates, Event)
                    # Filter results by geographical criteria
                    .spatial("coordinates", lambda criteria: criteria.within_radius(20, 47.623473, -122.3060097))
                    # Sort results, call 'order_by_distance'
                    .order_by_distance(
                        # Pass the spatial index-field containing the spatial data
                        "coordinates",
                        # Sort the results by their distance from this point
                        47.623473,
                        -122.3060097,
                    )
                )
                # Return all matching Event entities located within 20 kilometers radius
                # from point (47.623473 latitude, -122.3060097 longitude).

                # Sort the results by their distance from a specified point,
                # the closest results will be listed first.

                # endregion


# region spatial_syntax_1
class DynamicSpatialField(ABC):
    def __init__(self, round_factor: float = 0): ...
class PointField(DynamicSpatialField):  # Latitude/Longitude coordinates
    def __init__(self, latitude: str, longitude: str): ...


class WktField(DynamicSpatialField):  # Shape in WKT string format
    def __init__(self, wkt: str): ...


# endregion


# region spatial_syntax_2
class SpatialOptionsFactory:
    def geography(self) -> GeographySpatialOptionsFactory:
        return SpatialOptionsFactory.GeographySpatialOptionsFactory()

    def cartesian(self) -> CartesianSpatialOptionsFactory:
        return SpatialOptionsFactory.CartesianSpatialOptionsFactory()


# endregion
class GeographySpatialOptionsFactory:
    # region spatial_syntax_3
    # Default is GeohashPrefixTree strategy with max_tree_level set to 9
    def default_option(self, circle_radius_units: SpatialUnits = SpatialUnits.KILOMETERS) -> SpatialOptions: ...

    def bounding_box_index(self, circle_radius_units: SpatialUnits = SpatialUnits.KILOMETERS) -> SpatialOptions: ...
    def geohash_prefix_tree_index(
        self, max_tree_level: int, circle_radius_units: SpatialUnits = SpatialUnits.KILOMETERS
    ) -> SpatialOptions: ...
    def quad_prefix_tree_index(
        self, max_tree_level: int, circle_radius_units: SpatialUnits = SpatialUnits.KILOMETERS
    ) -> SpatialOptions: ...

    # endregion


class CartesianSpatialOptionsFactory:
    # region spatial_syntax_4
    def bounding_box_index(self) -> SpatialOptions: ...
    def quad_prefix_tree_index(
        self, max_tree_level: int, bounds: SpatialOptionsFactory.SpatialBounds
    ) -> SpatialOptions: ...

    class SpatialBounds:
        def __init__(self, min_x: float, min_y: float, max_x: float, max_y: float): ...

    # endregion
