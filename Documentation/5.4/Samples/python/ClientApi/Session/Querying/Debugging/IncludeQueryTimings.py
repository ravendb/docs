from typing import Optional, Callable, TypeVar, Dict

from ravendb import QueryTimings, DocumentQuery

from examples_base import ExampleBase, Product

_T = TypeVar("_T")


class Foo:
    # region syntax
    def timings(self, timings_callback: Callable[[QueryTimings], None]) -> DocumentQuery[_T]: ...

    # endregion

    # region syntax_2
    class QueryTimings:
        def __init__(self, duration_in_ms: int = None, timings: Dict[str, QueryTimings] = None):
            self.duration_in_ms = duration_in_ms
            self.timings = timings or {}

    # endregion


class QueryTimingsExample(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_timings(self):
        with self.embedded_server.get_document_store("Timings") as store:
            with store.open_session() as session:
                # region timing_2
                timings: Optional[QueryTimings] = None

                # Prepare a callback
                def timings_callback(timings_from_server: QueryTimings):
                    timings = timings_from_server

                results = list(
                    session.advanced.document_query(object_type=Product)
                    # Call timings, provide a callback function that will be called with result timings
                    .timings(timings_callback)
                    # Define query criteria
                    # i.e. search for docs containing Syrup -or- Lager in their Name field
                    .search("Name", "Syrup Lager")
                    # Execute the query
                )

                # Get total query duration:
                # =========================
                total_query_duration = timings.duration_in_ms

                # Get specific parts duration:
                # ============================
                timings_dictionary = timings.timings
                optimizer_duration = timings_dictionary["Optimizer"].duration_in_ms
                lucene_duration = timings_dictionary["Query"].timings["lucene"].duration_in_ms
                # endregion
