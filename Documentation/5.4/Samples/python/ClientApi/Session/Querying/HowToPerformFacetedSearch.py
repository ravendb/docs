import enum
from abc import ABC
from typing import Dict, Union, Set, Optional, List, Callable, TypeVar

from ravendb import (
    Facet,
    RangeFacet,
    FacetAggregationField,
    FacetBase,
    AggregationDocumentQuery,
    FacetBuilder,
    FacetOperations,
    RangeBuilder,
    IndexDefinition,
    PutIndexesOperation,
)
from ravendb.documents.queries.facets.definitions import FacetSetup
from ravendb.documents.queries.facets.misc import FacetOptions, FacetTermSortMode, FacetAggregation
from ravendb.primitives import constants

from cameras_helper import get_cameras, Camera
from examples_base import ExampleBase

_T = TypeVar("_T")


class HowToPerformFacetedSearch(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("FacetedSearch") as store:
            with store.bulk_insert() as bulk_insert:
                for camera in get_cameras(100):
                    bulk_insert.store(camera)

            index_definition = IndexDefinition()
            index_definition.name = "Camera/Costs"
            index_definition.maps = [
                "from camera in docs select new { camera.manufacturer, camera.model, camera.cost, camera.date_of_listing, camera.megapixels } "
            ]
            store.maintenance.send(PutIndexesOperation(index_definition))

    def test_how_to_perform_faceted_search(self):
        with self.embedded_server.get_document_store("FacetedSearch") as store:
            with store.open_session() as session:
                # region facet_2_1
                facet_options = FacetOptions.default_options()
                facet_options.term_sort_mode = FacetTermSortMode.COUNT_DESC
                facet_options.start = 0

                facet1 = Facet("manufacturer")
                facet1.options = facet_options

                facet2 = RangeFacet()
                facet2.ranges = [
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800",
                ]
                facet2.aggregations = {FacetAggregation.AVERAGE: {FacetAggregationField("cost")}}

                facet3 = RangeFacet()
                facet3.ranges = [
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10",
                ]

                facets = (
                    session.query_index("Camera/Costs", Camera)
                    .aggregate_by(facet1)
                    .and_aggregate_by(facet2)
                    .and_aggregate_by(facet3)
                    .execute()
                )
                # endregion

            with store.open_session() as session:
                # region facet_3_1
                options = FacetOptions()
                options.start = 0
                options.term_sort_mode = FacetTermSortMode.COUNT_DESC

                cost_builder = RangeBuilder.for_path("cost")
                megapixels_builder = RangeBuilder.for_path("megapixels")

                facet_result = (
                    session.query_index("Camera/Costs", Camera)
                    .aggregate_by(lambda builder: builder.by_field("manufacturer").with_options(options))
                    .and_aggregate_by(
                        lambda builder: builder.by_ranges(
                            cost_builder.is_less_than(200),
                            cost_builder.is_greater_than_or_equal_to(200).is_less_than(400),
                            cost_builder.is_greater_than_or_equal_to(400).is_less_than(600),
                            cost_builder.is_greater_than_or_equal_to(600).is_less_than(800),
                            cost_builder.is_greater_than_or_equal_to(800),
                        ).average_on("cost")
                    )
                    .and_aggregate_by(
                        lambda builder: builder.by_ranges(
                            megapixels_builder.is_less_than(3),
                            megapixels_builder.is_greater_than_or_equal_to(3).is_less_than(7),
                            megapixels_builder.is_greater_than_or_equal_to(7).is_less_than(10),
                            megapixels_builder.is_greater_than_or_equal_to(10),
                        )
                    )
                ).execute()
                # endregion

            with store.open_session() as session:
                # region facet_4_1
                facet_setup = FacetSetup()

                facet_manufacturer = Facet()
                facet_manufacturer.field_name = "manufacturer"
                facet_setup.facets = [facet_manufacturer]

                camera_facet = RangeFacet()
                camera_facet.ranges = [
                    "cost < 200",
                    "cost between 200 and 400",
                    "cost between 400 and 600",
                    "cost between 600 and 800",
                    "cost >= 800",
                ]

                megapixels_facet = RangeFacet()
                megapixels_facet.ranges = [
                    "megapixels < 3",
                    "megapixels between 3 and 7",
                    "megapixels between 7 and 10",
                    "megapixels >= 10",
                ]

                facet_setup.range_facets = [camera_facet, megapixels_facet]

                session.store(facet_setup, "facets/CameraFacets")
                session.save_changes()

                facets = session.query_index("Camera/Costs", Camera).aggregate_using("facets/CameraFacets").execute()
                # endregion

    # region facet_7_3
    class FacetBase(ABC):
        def __init__(self):
            self.display_field_name: Union[None, str] = None
            self.options: Union[None, FacetOptions] = None
            self.aggregations: Dict[FacetAggregation, Set[FacetAggregationField]] = {}

    class Facet(FacetBase):
        def __init__(self, field_name: str = None):
            super().__init__()
            self.field_name = field_name

    # endregion

    # region facet_7_4
    class RangeFacet(FacetBase):
        def __init__(self, parent: Optional[FacetBase] = None):
            super().__init__()
            self.ranges: List[str] = []

    # endregion

    # region facet_7_5
    class FacetAggregation(enum.Enum):
        NONE = "None"
        MAX = "Max"
        MIN = "Min"
        AVERAGE = "Average"
        SUM = "Sum"

    # endregion

    class Foo:
        # region facet_1
        def aggregate_by(
            self, builder_or_facet: Union[Callable[[FacetBuilder], None], FacetBase]
        ) -> AggregationDocumentQuery[_T]: ...

        def aggregate_by_facets(self, facets: List[FacetBase]) -> AggregationDocumentQuery[_T]: ...

        def aggregate_using(self, facet_setup_document_id: str) -> AggregationDocumentQuery[_T]: ...

        # endregion

        # region facet_7_1
        def by_ranges(self, range_: RangeBuilder, *ranges: RangeBuilder) -> FacetOperations[_T]: ...

        def by_field(self, field_name: str) -> FacetOperations[_T]: ...

        def with_display_name(self, display_name: str) -> FacetOperations[_T]: ...

        def with_options(self, options: FacetOptions) -> FacetOperations[_T]: ...

        def sum_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def min_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def max_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        def average_on(self, path: str, display_name: Optional[str] = None) -> FacetOperations[_T]: ...

        # endregion

    class Foo1:
        # region facet_7_2
        def __init__(self):
            self.page_size: int = constants.int_max
            self.start: Union[None, int] = None
            self.term_sort_mode: FacetTermSortMode = FacetTermSortMode.VALUE_ASC
            self.include_remaining_terms: bool = False

        # endregion
