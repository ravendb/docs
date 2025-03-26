import enum
import time
from abc import ABC
from typing import Union, Callable, List, TypeVar, Dict, Set, Optional

from ravendb import (
    AbstractIndexCreationTask,
    RangeFacet,
    Facet,
    RangeBuilder,
    FacetAggregationField,
    FacetBuilder,
    AggregationDocumentQuery,
    FacetOperations,
)
from ravendb.documents.queries.facets.definitions import FacetSetup, FacetBase
from ravendb.documents.queries.facets.misc import FacetOptions, FacetTermSortMode, FacetAggregation
from ravendb.primitives import constants
from ravendb.serverwide.operations.common import DeleteDatabaseOperation

from examples_base import ExampleBase

_T = TypeVar("_T")


# region camera_class
class Camera:
    def __init__(
        self,
        manufacturer: str = None,
        cost: float = None,
        mega_pixels: float = None,
        max_focal_length: int = None,
        units_in_stock: int = None,
    ):
        self.manufacturer = manufacturer
        self.cost = cost
        self.mega_pixels = mega_pixels
        self.max_focal_length = max_focal_length
        self.units_in_stock = units_in_stock


# endregion


# region camera_index
class Cameras_ByFeatures(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(
            self,
            brand: str = None,
            price: float = None,
            mega_pixels: float = None,
            max_focal_length: int = None,
            units_in_stock: int = None,
        ):
            self.brand = brand
            self.price = price
            self.mega_pixels = mega_pixels
            self.max_focal_length = max_focal_length
            self.units_in_stock = units_in_stock

    def __init__(self):
        super().__init__()
        self.map = (
            "from camera in docs.Cameras "
            "select new "
            "{ "
            " brand = camera.manufacturer,"
            " price = camera.cost,"
            " mega_pixels = camera.mega_pixels,"
            " max_focal_length = camera.max_focal_length,"
            " units_in_stock = camera.units_in_stock"
            "}"
        )


# endregion


class FacetsBasics(ExampleBase):
    def setUp(self):
        super().setUp()

    def tearDown(self):
        super().tearDown()
        with self.embedded_server.get_document_store("Cameras") as store:
            store.maintenance.send(DeleteDatabaseOperation("FacetsCameras", hard_delete=True))

    def test_facets_basics(self):

        with self.embedded_server.get_document_store("FacetsCameras") as store:
            Cameras_ByFeatures().execute(store)
            # region camera_sample_data
            # Creating sample data for the examples in this article:
            # ======================================================

            cameras = [
                Camera(manufacturer="Sony", cost=100, mega_pixels=20.1, max_focal_length=200, units_in_stock=10),
                Camera(manufacturer="Sony", cost=200, mega_pixels=29, max_focal_length=250, units_in_stock=15),
                Camera(manufacturer="Nikon", cost=120, mega_pixels=22.3, max_focal_length=300, units_in_stock=2),
                Camera(manufacturer="Nikon", cost=180, mega_pixels=32, max_focal_length=300, units_in_stock=5),
                Camera(manufacturer="Nikon", cost=220, mega_pixels=40, max_focal_length=300, units_in_stock=20),
                Camera(manufacturer="Canon", cost=200, mega_pixels=30.4, max_focal_length=400, units_in_stock=30),
                Camera(manufacturer="Olympus", cost=250, mega_pixels=32.5, max_focal_length=600, units_in_stock=4),
                Camera(manufacturer="Olympus", cost=390, mega_pixels=40, max_focal_length=600, units_in_stock=6),
                Camera(manufacturer="Fuji", cost=410, mega_pixels=45, max_focal_length=700, units_in_stock=1),
                Camera(manufacturer="Fuji", cost=590, mega_pixels=45, max_focal_length=700, units_in_stock=5),
                Camera(manufacturer="Fuji", cost=650, mega_pixels=61, max_focal_length=800, units_in_stock=17),
                Camera(manufacturer="Fuji", cost=850, mega_pixels=102, max_focal_length=800, units_in_stock=19),
            ]

            with store.open_session() as session:
                for camera in cameras:
                    session.store(camera)
                session.save_changes()
            # endregion
            # region facets_1
            # Define a Facet:
            # ===============
            facet = Facet(
                # Specify the index-field for which to get count of documents per unique ITEM
                # e.g. get the number of Camera documents for each unique brand
                field_name="brand",
            )

            # Set a display name for this field in the results (optional)
            facet.display_field_name = "Camera Brand"

            # Define a RangeFacet:
            # ====================
            range_facet = RangeFacet()
            # Specify ranges within an index-field in order to get count per RANGE
            # e.g. get the number of Camera documents that cost below 200, between 200 & 400, etc...
            range_facet.ranges = [
                "price < 200",
                "price between 200 and 400",
                "price between 400 and 600",
                "price between 600 and 800",
                "price >= 800",
            ]

            # Set a display name for this field in the results (optional)
            range_facet.display_field_name = "Camera Price"

            # Define a list of facets to query by:
            # ====================================
            facets = [facet, range_facet]

            # endregion

            # region facets_2
            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by' to aggregate the data by facets
                # Pass the defined facets from above
                .aggregate_by_facets(facets).execute()
            )
            # endregion

            # region facets_2_rawQuery
            results = (
                session.advanced
                # Query the index
                # Provide the RQL string to the raw_query method
                .raw_query(
                    """from index 'Cameras/ByFeatures'
                   select
                       facet(brand) as 'Camera Brand',
                       facet(price < 200.0,
                             price >= 200.0 and price < 400.0,
                             price >= 400.0 and price < 600.0,
                             price >= 600.0 and price < 800.0,
                             price >= 800.0) as 'Camera Price'""",
                    object_type=Camera,
                )
                # Execute the query
                .execute_aggregation()
            )
            # endregion

            time.sleep(2)
            # region facets_5
            # Query the index
            results = (
                session.query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by' to aggregate the data by facets
                # Use a builder as follows:
                .aggregate_by(
                    lambda builder: builder
                    # Specify the index-field (e.g. 'brand') for which to get count per unique ITEM
                    .by_field("brand")
                    # Set a display name for the field in the results (optional)
                    .with_display_name("Camera Brand")
                )
                .and_aggregate_by(
                    lambda builder: builder
                    # Specify ranges within an index field (e.g. 'Price') in order to get count per RANGE
                    .by_ranges(
                        RangeBuilder("price").is_less_than(200),
                        RangeBuilder("price").is_greater_than_or_equal_to(200).is_less_than(400),
                        RangeBuilder("price").is_greater_than_or_equal_to(400).is_less_than(600),
                        RangeBuilder("price").is_greater_than_or_equal_to(600).is_less_than(800),
                        RangeBuilder("price").is_greater_than_or_equal_to(800),
                    )
                    # Set a display name for the field in the results (optional)
                    .with_display_name("Camera Price")
                )
                .execute()
            )
            # endregion

            # region facets_6
            # The resulting aggregations per display name will contain:
            # =========================================================

            # For the "Camera Brand" Facet:
            #     "canon"   - Count: 1
            #     "fuji"    - Count: 4
            #     "nikon"   - Count: 3
            #     "olympus" - Count: 2
            #     "sony"    - Count: 2

            # For the "Camera Price" Ranges:
            #     "price < 200"                      - Count: 3
            #     "200 <= price < 400" - Count: 5
            #     "400 <= price < 600" - Count: 2
            #     "600 <= price < 800" - Count: 1
            #     "price >= 800"                   - Count: 1
            # endregion

            # region facets_7
            # Get facets results for index-field 'brand' using the display name specified:
            # ============================================================================
            brand_facets = results["Camera Brand"]
            number_of_brands = len(brand_facets.values)  # 5 unique brands

            # Get the aggregated facet value for a specific brand:
            facet_value = brand_facets.values[0]
            # The brand name is available in the 'Range' property
            # Note: value is lower-case since the default RavenDB analyzer was used by the index
            self.assertEqual("canon", facet_value.range_)
            # Number of documents for 'Canon' is available in the 'Count' property
            self.assertEqual(1, facet_value.count_)

            # Get facets results for index-field 'Price' using the display name specified:
            # ============================================================================
            price_facets = results["Camera Price"]
            number_of_ranges = len(price_facets.values)  # 5 different ranges

            # Get the aggregated facet value for a specific Range:
            facet_value = price_facets.values[0]
            self.assertEqual("price < 200", facet_value.range_)  # The range string
            self.assertEqual(3, facet_value.count_)
            # endregion

            # region facets_8
            filtered_results = list(
                session.query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                .where_in("brand", ["Fuji", "Nikon"])
                .aggregate_by_facets(facets)
                .execute()
            )
            # endregion

            # region facets_9
            # Define the list of facets to query by:
            # ======================================
            facets_with_options = [
                # Define a Facet:
                Facet(
                    # Specify the index-field for which to get count of documents per unique ITEM
                    field_name="brand",
                )
            ]
            # Set some facets options
            # Assign facet options after creating the object
            facets_with_options[0].options = FacetOptions()
            # Return the top 3 brands with most items count:
            facets_with_options[0].options.page_size = 3
            facets_with_options[0].options.term_sort_mode = FacetTermSortMode.COUNT_DESC
            facets_with_options[0].options.start = 0
            # endregion
            time.sleep(1)
            # region facets_10
            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by' to aggregate the data by facets
                # Pass the defined facets from above
                .aggregate_by_facets(facets_with_options).execute()
            )
            # endregion

            # region facets_10_rawQuery
            results = (
                session.advanced
                # Query the index
                # Provide the RQL string to the raw_query method
                .raw_query(
                    """from index 'Cameras/ByFeatures'
                   select facet(brand, $p0)""",
                    object_type=Camera,
                )
                # Add the facet options to the "p0" parameter
                .add_parameter("p0", {"PageSize": 3, "TermSortMode": FacetTermSortMode.COUNT_DESC})
                # Execute the query
                .execute_aggregation()
            )
            # endregion

            # region facets_13

            # Return the top 3 brands with most items count:
            facet_options = FacetOptions()
            facet_options.start = 0
            facet_options.page_size = 3
            facet_options.term_sort_mode = FacetTermSortMode.COUNT_DESC

            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by' to aggregate the data by facets
                # Use a builder as follows:
                .aggregate_by(
                    lambda builder: builder
                    # Specify an index-field (e.g. 'brand') for which to get count per unique ITEM
                    .by_field("brand")
                    # Specify the facets options
                    .with_options(facet_options)
                ).execute()
            )
            # endregion

            # region facets_14
            # The resulting items will contain:
            # =================================
            # For the "brand" Facet:
            #     "fuji"    - Count: 4
            #     "nikon"   - Count: 3
            #     "olympus" - Count: 2
            # As requested, only 3 unique items are returned, ordered by documents count descending:
            # endregion

            # region facets_15
            # Get facets results for index-field 'brand':
            # ===========================================
            brand_facets = results["brand"]
            number_of_brands = len(brand_facets.values)  # 3 brands

            # Get the aggregated facet value for a specific brand:
            facet_value = brand_facets.values[0]
            # The brand name is available in the 'Range' property
            # Note: value is lower-case since the default RavenDB analyzer was used by the index
            self.assertEqual("fuji", facet_value.range_)
            # Number of documents for 'Fuji' is available in the 'Count' property
            self.assertEqual(4, facet_value.count_)
            # endregion
            # region facets_16
            # Define the list of facets to query by:
            # =====================================

            # Define a facet:
            # ===============
            facet_with_aggregations = Facet()
            facet_with_aggregations.field_name = "brand"
            facet_with_aggregations.aggregations = {
                # Set the aggregation operation:
                FacetAggregation.SUM:
                # Create a set specifying the index-fields for which to perform the aggregation
                {
                    # Get total number of units_in_stock per brand
                    FacetAggregationField("units_in_stock")
                },
                FacetAggregation.AVERAGE: {
                    # Get average price per brand
                    FacetAggregationField("price")
                },
                FacetAggregation.MIN: {
                    # Get min price per brand
                    FacetAggregationField("price")
                },
                FacetAggregation.MAX: {
                    # Get max mega_pixels per brand
                    FacetAggregationField("mega_pixels"),
                    # Get max max_focal_length per brand
                    FacetAggregationField("max_focal_length"),
                },
            }

            # Define a RangeFacet:
            # ===================
            range_facet_with_aggregations = RangeFacet()
            range_facet_with_aggregations.ranges = [
                "price < 200",
                "price between 200 and 400",
                "price between 400 and 600",
                "price between 600 and 800",
                "price >= 800",
            ]
            range_facet_with_aggregations.aggregations = {
                FacetAggregation.SUM: {
                    # Get total number of units_in_stock for each group of documents per range specified
                    FacetAggregationField("units_in_stock")
                },
                FacetAggregation.AVERAGE: {
                    # Get average price of each group of documents per range specified
                    FacetAggregationField("price")
                },
                FacetAggregation.MIN: {
                    # Get min price of each group of documents per range specified
                    FacetAggregationField("price")
                },
                FacetAggregation.MAX: {
                    # Get max mega_pixels for each group of documents per range specified
                    FacetAggregationField("mega_pixels"),
                    # Get max max_focal_length for each group of documents per range specified
                    FacetAggregationField("max_focal_length"),
                },
            }

            facets_with_aggregations = [facet_with_aggregations, range_facet_with_aggregations]
            # endregion

            # region facets_17
            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by_facets' to aggregate the data by facets
                # Pass the defined facets from above
                .aggregate_by_facets(facets_with_aggregations).execute()
            )
            # endregion

            # region facets_17_rawQuery
            results = (
                session.advanced
                # Query the index
                # Provide the RQL string to the raw_query method
                .raw_query(
                    """
                       from index 'Cameras/ByFeatures'
                                            select
                                                facet(brand,
                                                      sum(units_in_stock),
                                                      avg(price),
                                                      min(price),
                                                      max(mega_pixels),
                                                      max(max_focal_length)),
                                                facet(price < $p0,
                                                      price >= $p1 and price < $p2,
                                                      price >= $p3 and price < $p4,
                                                      price >= $p5 and price < $p6,
                                                      price >= $p7,
                                                      sum(units_in_stock),
                                                      avg(price),
                                                      min(price),
                                                      max(mega_pixels),
                                                      max(max_focal_length))
                       """
                )
                .add_parameter("p0", 200.0)
                .add_parameter("p1", 200.0)
                .add_parameter("p2", 400.0)
                .add_parameter("p3", 400.0)
                .add_parameter("p4", 600.0)
                .add_parameter("p5", 600.0)
                .add_parameter("p6", 800.0)
                .add_parameter("p7", 800.0)
                # Execute the query
                .execute_aggregation()
            )
            # endregion

            # region facets_20
            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_by' to aggregate the data by facets
                # Use a builder as follows:
                .aggregate_by(
                    lambda builder: builder
                    # Specify an index-field (e.g. 'brand') for which to get count per unique ITEM
                    .by_field("brand")
                    # Specify the aggregations per the brand facet:
                    .sum_on("units_in_stock")
                    .average_on("price")
                    .min_on("price")
                    .max_on("mega_pixels")
                    .max_on("max_focal_length")
                )
                .and_aggregate_by(
                    lambda builder: builder
                    # Specify ranges within an index field (e.g. 'price') in order to get count per RANGE
                    .by_ranges(
                        RangeBuilder("price").is_less_than(200),
                        RangeBuilder("price").is_greater_than_or_equal_to(200).is_less_than(400),
                        RangeBuilder("price").is_greater_than_or_equal_to(400).is_less_than(600),
                        RangeBuilder("price").is_greater_than_or_equal_to(600).is_less_than(800),
                        RangeBuilder("price").is_greater_than_or_equal_to(800),
                    )
                    # Specify the aggregations per the price range:
                    .sum_on("units_in_stock")
                    .average_on("price")
                    .min_on("price")
                    .max_on("mega_pixels")
                    .max_on("max_focal_length")
                )
                .execute()
            )
            # endregion

            # region facets_21
            # The resulting items will contain (Showing partial results):
            # ===========================================================

            # For the "brand" Facet:
            #     "canon" Count:1, Sum: 30, Name: UnitsInStock
            #     "canon" Count:1, Min: 200, Average: 200, Name: Price
            #     "canon" Count:1, Max: 30.4, Name: MegaPixels
            #     "canon" Count:1, Max: 400, Name: MaxFocalLength

            #     "fuji" Count:4, Sum: 42, Name: UnitsInStock
            #     "fuji" Count:4, Min: 410, Name: Price
            #     "fuji" Count:4, Max: 102, Name: MegaPixels
            #     "fuji" Count:4, Max: 800, Name: MaxFocalLength

            #     etc.....
            #
            # For the "Price" Ranges:
            #     "Price < 200.0" Count:3, Sum: 17, Name: UnitsInStock
            #     "Price < 200.0" Count:3, Min: 100, Average: 133.33, Name: Price
            #     "Price < 200.0" Count:3, Max: 32, Name: MegaPixels
            #     "Price < 200.0" Count:3, Max: 300, Name: MaxFocalLength

            #     "Price < 200.0 and Price > 400.0" Count:5, Sum: 75, Name: UnitsInStock
            #     "Price < 200.0 and Price > 400.0" Count:5, Min: 200, Average: 252, Name: Price
            #     "Price < 200.0 and Price > 400.0" Count:5, Max: 40, Name: MegaPixels
            #     "Price < 200.0 and Price > 400.0" Count:5, Max: 600, Name: MaxFocalLength

            #     etc.....
            # endregion

            # region facets_22
            # Get results for the 'brand' facets:
            # ========================================
            brand_facets = results["brand"]

            # Get the aggregated facet value for a specific brand:
            facet_value = brand_facets.values[0]
            # The brand name is available in the 'Range' property:
            self.assertEqual("canon", facet_value.range_)
            # The index-field on which aggregation was done is in the 'name' property:
            self.assertEqual("units_in_stock", facet_value.name)
            # The requested aggregation result
            self.assertEqual(30, facet_value.sum_)

            # Get results for the 'price' RangeFacets:
            # ========================================
            price_range_facets = results["price"]

            # Get the aggregated facet value for a specific brand:
            facet_value = price_range_facets.values[0]
            # The range string is available in the 'Range' property:
            self.assertEqual("price < 200", facet_value.range_)
            # The index-field on which aggregation was done is in the 'Name' property:
            self.assertEqual("units_in_stock", facet_value.name)
            # The requested aggregation result:
            self.assertEqual(17, facet_value.sum_)
            # endregion

            # region facets_23
            facet_setup = FacetSetup()
            # Provide  the ID of the document in which the facet setup will be stored.
            # This is optional -
            # if not provided then the session will assign an ID for the stored document.
            facet_setup.Id = "customDocumentID"

            # Define Facets and RangeFacets to query by:
            facet = Facet("brand")
            range_facet = RangeFacet()
            range_facet.ranges = [
                "mega_pixels < 20",
                "mega_pixels between 20 and 30",
                "mega_pixels between 30 and 50",
                "mega_pixels >= 50",
            ]

            facet_setup.facets = [facet]
            facet_setup.range_facets = [range_facet]

            # Store the facet setup document and save changes:
            # ===============================================
            session.store(facet_setup)
            session.save_changes()

            # The document will be stored under the 'FacetSetups' collection
            # endregion

            # region facets_24
            results = (
                session
                # Query the index
                .query_index_type(Cameras_ByFeatures, Cameras_ByFeatures.IndexEntry)
                # Call 'aggregate_using'
                # Pass the ID of the document that contains your facets setup
                .aggregate_using("customDocumentID").execute()
            )
            # endregion
            # region facets_24_rawQuery
            results = (
                session.advanced
                # Query the index
                # Provide the RQL string to the raw_query method
                .raw_query("from index 'Cameras/ByFeatures' select facet(id('customDocumentID'))", Camera)
                # Execute the query
                .execute_aggregation()
            )
            # endregion


class FacetsSyntax:
    class Foo1:
        # region syntax_1
        def aggregate_by(
            self, builder_or_facet: Union[Callable[[FacetBuilder], None], FacetBase]
        ) -> AggregationDocumentQuery[_T]: ...

        def aggregate_by_facets(self, facets: List[FacetBase]) -> AggregationDocumentQuery[_T]: ...

        def aggregate_using(self, facet_setup_document_id: str) -> AggregationDocumentQuery[_T]: ...

        # endregion

    class Foo:
        # region syntax_2
        class Facet(FacetBase):
            def __init__(self, field_name: str = None):
                super().__init__()
                self.field_name = field_name

        # endregion
        # region syntax_3
        class RangeFacet(FacetBase):
            def __init__(self, parent: Optional[FacetBase] = None):
                super().__init__()
                self.ranges: List[str] = []

        # endregion
        # region syntax_4
        class FacetBase(ABC):
            def __init__(self):
                self.display_field_name: Union[None, str] = None
                self.options: Union[None, FacetOptions] = None
                self.aggregations: Dict[FacetAggregation, Set[FacetAggregationField]] = {}

        # endregion
        # region syntax_5

        class FacetAggregation(enum.Enum):
            NONE = "None"
            MAX = "Max"
            MIN = "Min"
            AVERAGE = "Average"
            SUM = "Sum"

        # endregion
        # region syntax_6
        def by_ranges(self, range_: RangeBuilder, *ranges: RangeBuilder) -> FacetOperations[_T]: ...

        def by_field(self, field_name: str) -> FacetOperations[_T]: ...

        def with_display_name(self, display_name: str) -> FacetOperations[_T]: ...

        def with_options(self, options: FacetOptions) -> FacetOperations[_T]: ...

        def sum_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def min_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def max_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def average_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        # endregion

    class Foo2:
        # region syntax_7
        class FacetOptions:
            def __init__(self):
                self.page_size: int = constants.int_max
                self.start: Union[None, int] = None
                self.term_sort_mode: FacetTermSortMode = FacetTermSortMode.VALUE_ASC
                self.include_remaining_terms: bool = False

        # endregion
