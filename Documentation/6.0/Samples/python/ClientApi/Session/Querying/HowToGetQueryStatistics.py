from typing import Callable, TypeVar

from ravendb import DocumentQuery, QueryStatistics
from ravendb.infrastructure.orders import Employee

from examples_base import ExampleBase

_T = TypeVar("_T")


class GetQueryStatistics(ExampleBase):
    def setUp(self):
        super().setUp()

    class Foo:
        # region stats_1
        def statistics(self, stats_callback: Callable[[QueryStatistics], None]) -> DocumentQuery[_T]: ...

    def test_get_query_statistics(self):
        with self.embedded_server.get_document_store("QueryStatistics") as store:
            with store.open_session() as session:
                # region stats_2
                def __statistics_callback(statistics: QueryStatistics) -> None:
                    # Read and interact with QueryStatistics here
                    total_results = statistics.total_results
                    duration_milliseconds = statistics.duration_in_ms
                    ...

                employees = list(
                    session.query(object_type=Employee)
                    .where_equals("first_name", "Robert")
                    .statistics(__statistics_callback)
                )
                # endregion
