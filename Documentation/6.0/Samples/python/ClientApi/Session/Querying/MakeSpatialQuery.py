from typing import Callable, Union, Optional, TypeVar

from ravendb import PointField, SpatialCriteriaFactory, SpatialCriteria, DynamicSpatialField, DocumentQuery
from ravendb.documents.indexes.spatial.configuration import SpatialRelation, SpatialUnits
from ravendb.primitives import constants

from examples_base import ExampleBase

_T = TypeVar("_T")


class Address:
    def __init__(self, location):
        self.Location = location


class Location:
    def __init__(self, lat, lng):
        self.Latitude = lat
        self.Longitude = lng


class Employee:
    def __init__(self, address, lastname: str = None):
        self.Address = address
        self.LastName = lastname


class Company:
    def __init__(self, address):
        self.Address = address


class MakeSpatialQuery(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("SpatialSamples") as store:
            with store.open_session() as session:
                session.store(Employee(Address(Location(47.623475, -122.3060097)), "Joe"))
                session.store(Employee(Address(Location(47.623474, -122.3060097)), "Moe"))
                session.store(Employee(Address(Location(47.623470, -122.3060092)), "Foe"))

                session.store(Company(Address(Location(47.623475, -122.3060097))))
                session.store(Company(Address(Location(47.623474, -122.3060097))))
                session.store(Company(Address(Location(47.623470, -122.3060092))))
                session.save_changes()

    def test_sample(self):
        with self.embedded_server.get_document_store("SpatialSamples") as store:
            with store.open_session() as session:
                # region spatial_1
                # This query will return all matching employee entities
                # that are located within 20 kilometers radius
                # from point (47.623473 latitude, -122.3060097 longitude).

                # Define a query on Employees collection
                employees_within_radius = list(
                    session.query(object_type=Employee)
                    # Call 'spatial' method
                    .spatial(
                        # Create 'PointField'
                        # Pass the path to document fields containing the spatial data
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"),
                        # Set the geographical area in which to search for matching documents
                        # Call 'within_radius', pass the radius and the center points coordinates
                        lambda criteria: criteria.within_radius(20, 47.623473, -122.3060097),
                    )
                )
                # endregion

            with store.open_session() as session:
                # region spatial_2
                # This query will return all matching employee entities
                # that are located within 20 kilometers radius
                # from point (47.623473 latitude, -122.3060097 longitude).

                # Define a query on Employees collection
                employees_within_shape = list(
                    session.query(object_type=Employee)
                    # Call 'spatial' method
                    .spatial(
                        # Create 'PointField'
                        # Pass the path to document fields containing the spatial data
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"),
                        # Set the geographical search criteria, call 'relates_to_shape'
                        lambda criteria: criteria.relates_to_shape(
                            # Specify the WKT string. Note: longitude is written FIRST
                            shape_wkt="CIRCLE(-122.3060097 47.623473 d=20)",
                            # Specify the relation between the WKT shape and the documents spatial data
                            relation=SpatialRelation.WITHIN,
                            # Optional: customize radius units (default is Kilometers)
                            units=SpatialUnits.MILES,
                        ),
                    )
                )
                # endregion

            with store.open_session() as session:
                # region spatial_3
                # This query will return all matching company entities
                # that are located within the specified polygon.

                # Define a query on Companies collection
                companies_within_shape = list(
                    session.query(object_type=Company)
                    # Call 'spatial' method
                    .spatial(
                        # Create 'PointField'
                        # Pass the path to document fields containing the spatial data
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"),
                        # Set the geographical search criteria, call 'relates_to_shape'
                        lambda criteria: criteria.relates_to_shape(
                            # Specify the WKT string. Note: longitude is written FIRST
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
                # endregion

            with store.open_session() as session:
                # region spatial_4
                # Return all matching employee entities located within 20 kilometers radius
                # from point (47.623473 latitude, -122.3060097 longitude)

                # Sort the results by their distance from a specified point,
                # the closest results will be listed first.

                employees_sorted_by_distance = list(
                    session.query(object_type=Employee)
                    # Provide the query criteria:
                    .spatial(
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"),
                        lambda criteria: criteria.within_radius(20, 47.623473, -122.3060097),
                    )
                    # Call 'order_by_distance'
                    .order_by_distance(
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"), 47.623473, -122.3060097
                    )
                )
                # endregion

                # region spatial_4_getDistance
                # Get the distance of the results:
                # ================================

                # Call 'get_metadata_for', pass an entity from the resulting employees list
                metadata = session.advanced.get_metadata_for(employees_sorted_by_distance[0])

                # The distance is available in the '@spatial' metadata property
                spatial_results = metadata["@spatial"]

                distance = spatial_results["Distance"]  # The distance of the entity from the queried location
                latitude = spatial_results["Latitude"]  # The entity's latitude value
                longitude = spatial_results["Longitude"]  # The entity's longitude value
                # endregion

            with store.open_session() as session:
                # region spatial_5
                # Return all employee entities sorted by their distance from a specified point.
                # The farthest results will be listed first.

                employees_sorted_by_distance_desc = list(
                    session.query(object_type=Employee)
                    # Call 'order_by_distance_descending'
                    .order_by_distance_descending(
                        # Pass the path to document fields containing the spatial data
                        PointField("Address.Location.Latitude", "Address.Location.Longitude"),
                        # Sort the results by their distance (descending) from this point:
                        47.623473,
                        -122.3060097,
                    )
                )
                # endregion

            with store.open_session() as session:
                # region spatial_6
                # Return all employee entities.
                # Results are sorted by their distance to a specified point rounded to the nearest 100 km interval.
                # A secondary sort can be applied within the 100 km range, e.g. by field LastName.

                employees_sorted_by_rounded_distance = list(
                    session.query(object_type=Employee)
                    # Call 'order_by_distance'
                    .order_by_distance(
                        # Pass the path to the document fields containing the spatial data
                        PointField("Address.Location.Latitude", "Address.Location.Longitude")
                        # Round up distance to 100 km
                        .round_to(100),
                        # Sort the results by their distance from this point:
                        47.623473,
                        -122.3060097,
                    ).order_by(
                        "LastName"
                    )  # todo gracjan: check if its possible to order by again without then_by
                    # todo reeb: skip this example for now, we'll get back to it later on
                    # A secondary sort can be applied
                )

                pass

        class Foo:
            # region spatial_7
            def spatial(
                self,
                field_name_or_field: Union[str, DynamicSpatialField],
                clause: Callable[[SpatialCriteriaFactory], SpatialCriteria],
            ): ...

            # endregion

            # region spatial_8
            class PointField(DynamicSpatialField):
                def __init__(self, latitude: str, longitude: str): ...

            class WktField(DynamicSpatialField):
                def __init__(self, wkt: str): ...

            # endregion

            # region spatial_9
            def relates_to_shape(
                self,
                shape_wkt: str,
                relation: SpatialRelation,
                units: SpatialUnits = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            def intersects(
                self,
                shape_wkt: str,
                units: Optional[SpatialUnits] = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            def contains(
                self,
                shape_wkt: str,
                units: Optional[SpatialUnits] = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            def disjoint(
                self,
                shape_wkt: str,
                units: Optional[SpatialUnits] = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            def within(
                self,
                shape_wkt: str,
                units: Optional[SpatialUnits] = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            def within_radius(
                self,
                radius: float,
                latitude: float,
                longitude: float,
                radius_units: Optional[SpatialUnits] = None,
                dist_error_percent: Optional[float] = constants.Documents.Indexing.Spatial.DEFAULT_DISTANCE_ERROR_PCT,
            ) -> SpatialCriteria: ...

            # endregion

            # region spatial_10

            # From point & rounding

            def order_by_distance(
                self,
                field_or_field_name: Union[str, DynamicSpatialField],
                latitude: float,
                longitude: float,
                round_factor: Optional[float] = 0.0,
            ) -> DocumentQuery[_T]: ...

            # From center of WKT shape

            def order_by_distance_wkt(
                self, field_or_field_name: Union[str, DynamicSpatialField], shape_wkt: str
            ) -> DocumentQuery[_T]: ...

            # endregion
            # region spatial_11

            # From point & rounding

            def order_by_distance_descending(
                self,
                field_or_field_name: Union[str, DynamicSpatialField],
                latitude: float,
                longitude: float,
                round_factor: Optional[float] = 0.0,
            ) -> DocumentQuery[_T]: ...

            # From center of WKT shape

            def order_by_distance_descending_wkt(
                self, field_or_field_name: Union[str, DynamicSpatialField], shape_wkt: str
            ) -> DocumentQuery[_T]: ...

            # endregion
