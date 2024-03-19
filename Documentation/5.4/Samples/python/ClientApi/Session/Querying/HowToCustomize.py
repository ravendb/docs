import logging
from datetime import timedelta
from enum import Enum
from typing import Callable, TypeVar

from ravendb import IndexQuery, QueryResult, AbstractIndexCreationTask, ProjectionBehavior, QueryTimings, DocumentQuery
from ravendb.documents.indexes.definitions import FieldStorage
from ravendb.infrastructure.orders import Employee

from examples_base import ExampleBase

_T = TypeVar("_T")


class HowToCustomize(ExampleBase):
    def setUp(self):
        super().setUp()
        with self.embedded_server.get_document_store("CustomizeQuery") as store:
            Employees_ByFullName().execute(store)

    def test_how_to_customize_examples(self):
        logger = logging.Logger("")

        with self.embedded_server.get_document_store("CustomizeQuery") as store:
            with store.open_session() as session:
                # region customize_1_1

                def __before_query_executed_callback(query: IndexQuery):
                    # Can modify query parameters
                    query.skip_duplicate_checking = True
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"Query to be executed is: {query.query}")

                results = list(
                    session.query(object_type=Employee)
                    # Call 'add_before_query_executed_listener'
                    .add_before_query_executed_listener(__before_query_executed_callback).where_equals(
                        "first_name", "Robert"
                    )
                )
                # endregion

            with store.open_session() as session:
                # region customize_1_3

                def __before_query_executed_callback(query: IndexQuery):
                    # Can modify query parameters
                    query.skip_duplicate_checking = True
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"Query to be executed is: {query.query}")

                results = list(
                    session.advanced.document_query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'add_before_query_executed_listener'
                    .add_before_query_executed_listener(__before_query_executed_callback)
                )

            with store.open_session() as session:
                # region customize_1_4

                def __before_query_executed_callback(query: IndexQuery):
                    # Can modify query parameters
                    query.skip_duplicate_checking = True
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"Query to be executed is: {query.query}")

                results = list(
                    session.advanced.raw_query("from 'Employees' where FirstName == 'Robert'")
                    # Call 'add_before_query_executed_listener'
                    .add_before_query_executed_listener(__before_query_executed_callback)
                )
                # endregion

            with store.open_session() as session:
                # region customize_2_1

                def __after_query_executed_callback(raw_result: QueryResult):
                    # Can access the raw query result
                    query_duration = raw_result.duration_in_ms
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"{raw_result.last_query_time}")

                results = list(
                    session.query(object_type=Employee)
                    # Call 'add_after_query_executed_listener'
                    .add_after_query_executed_listener(__after_query_executed_callback)
                )

                # endregion

            with store.open_session() as session:
                # region customize_2_3
                def __after_query_executed_callback(raw_result: QueryResult):
                    # Can access the raw query result
                    query_duration = raw_result.duration_in_ms
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"{raw_result.last_query_time}")

                result = list(
                    session.advanced.document_query(object_type=Employee)
                    # Call 'add_after_query_executed_listener'
                    .add_after_query_executed_listener(__after_query_executed_callback)
                )
                # endregion

            with store.open_session() as session:
                # region customize_2_4
                def __after_query_executed_callback(raw_result: QueryResult):
                    # Can access the raw query result
                    query_duration = raw_result.duration_in_ms
                    # Can apply any needed action, e.g. write to log
                    logger.info(f"{raw_result.last_query_time}")

                result = list(
                    session.advanced.raw_query("from 'Employees'")
                    # Call 'add_after_query_executed_listener'
                    .add_after_query_executed_listener(__after_query_executed_callback)
                )
                # endregion

            with store.open_session() as session:
                # region customize_4_1
                results = list(
                    session.query(object_type=Employee)
                    # Call 'no_caching'
                    .no_caching().where_equals("first_name", "Robert")
                )

                # endregion

            with store.open_session() as session:
                # region customize_4_3
                results = list(
                    session.advanced.document_query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'no_caching'
                    .no_caching()
                )
                # endregion

            with store.open_session() as session:
                # region customize_4_4
                results = list(
                    session.advanced.raw_query("from 'Employees' where first_name == 'Robert'")
                    # Call 'no_caching'
                    .no_caching()
                )
                # endregion

            with store.open_session() as session:
                # region customize_5_1
                results = list(
                    session.query(object_type=Employee)
                    # Call 'no_tracking`
                    .no_tracking().where_equals("first_name", "Robert")
                )
                # endregion

            with store.open_session() as session:
                # region customize_5_3
                results = list(
                    session.advanced.document_query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'no_tracking'
                    .no_tracking()
                )
                # endregion

            with store.open_session() as session:
                # region customize_5_4
                results = list(
                    session.advanced.raw_query("from 'Employees' where first_name == 'Robert'")
                    # Call 'no_tracking'
                    .no_tracking()
                )
                # endregion

            with store.open_session() as session:
                # region customize_6_1
                results = list(
                    session.query_index_type(Employees_ByFullName, Employees_ByFullName.IndexEntry)
                    # Pass the requested projection behavior to the 'select_fields' method
                    # and specify the field that will be returned to the projection
                    .select_fields(Employee, "full_name", projection_behavior=ProjectionBehavior.FROM_DOCUMENT_OR_THROW)
                )
                # endregion

            with store.open_session() as session:
                # region customize_6_3
                results = list(
                    session.advanced.document_query("Employees/ByFullName", object_type=Employee)
                    # Pass the requested projection behavior to the 'select_fields' method
                    # and specify the field that will be returned to the projection
                    .select_fields(Employee, "full_name", projection_behavior=ProjectionBehavior.FROM_DOCUMENT_OR_THROW)
                )
                # endregion

            with store.open_session() as session:
                try:
                    # region customize_6_4
                    results = list(
                        session.advanced
                        # Define an RQL query that returns a projection
                        .raw_query("from index 'Employees/ByFullName' select full_name", Employee)
                        # Pass the requested projection behavior to the 'projection' method
                        # WARNING: Not implemented yet!
                        # https://issues.hibernatingrhinos.com/issue/RDBC-817/Implement-query.projection
                        .projection(ProjectionBehavior.FROM_DOCUMENT_OR_THROW)
                    )
                    # endregion
                except NotImplementedError as e:
                    pass

            with store.open_session() as session:
                # region customize_7_1
                results = list(
                    session.query(object_type=Employee)
                    # Call 'random_ordering'
                    .random_ordering()
                )
                # endregion

            with store.open_session() as session:
                # region customize_7_3
                results = list(
                    session.advanced.document_query(object_type=Employee)
                    # Call 'random_ordering'
                    .random_ordering()
                )
                # endregion

            with store.open_session() as session:
                # region customize_7_4
                results = list(
                    session.advanced
                    # Define an RQL query that orders the results randomly
                    .raw_query("from 'Employees' order by random()", Employee)
                )
                # endregion

            with store.open_session() as session:
                # region customize_8_1
                results = list(
                    session.query(object_type=Employee)
                    # Call 'wait_for_non_stale_results`
                    .wait_for_non_stale_results(timedelta(seconds=10)).where_equals("first_name", "Robert")
                )
                # endregion

            with store.open_session() as session:
                # region customize_8_3
                results = list(
                    session.document_query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'wait_for_non_stale_results`
                    .wait_for_non_stale_results(timedelta(seconds=10))
                )
                # endregion

            with store.open_session() as session:
                # region customize_8_4
                results = list(
                    session.advanced.raw_query("from 'Employees' where first_name == 'Robert'", Employee)
                    # Call 'wait_for_non_stale_results`
                    .wait_for_non_stale_results(timedelta(seconds=10))
                )
                # endregion

            with store.open_session() as session:
                # region customize_9_1
                def __timings_callback(timings: QueryTimings):
                    logger.log(logging.DEBUG, timings.duration_in_ms)

                results = list(
                    session.query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'timings'.
                    # Provide a callback for the timings result - interact with QueryTimings inside the callback
                    .timings(__timings_callback)
                )
                # endregion

            with store.open_session() as session:
                # region customize_9_3
                def __timings_callback(timings: QueryTimings):
                    logger.log(logging.DEBUG, timings.duration_in_ms)

                results = list(
                    session.advanced.document_query(object_type=Employee).where_equals("first_name", "Robert")
                    # Call 'timings'.
                    # Provide a callback for the timings result - interact with QueryTimings inside the callback
                    .timings(__timings_callback)
                )
                # endregion

            with store.open_session() as session:
                # region customize_9_4
                def __timings_callback(timings: QueryTimings):
                    logger.log(logging.DEBUG, timings.duration_in_ms)

                results = list(
                    session.advanced.raw_query("from 'Employees' where first_name == 'Robert'")
                    # Call 'timings'.
                    # Provide a callback for the timings result - interact with QueryTimings inside the callback
                    .timings(__timings_callback)
                )
                # endregion


class IFoo:
    # region customize_1_5
    def add_before_query_executed_listener(self, action: Callable[[IndexQuery], None]) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_2_5
    def add_after_query_executed_listener(self, action: Callable[[QueryResult], None]) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_3_5
    def add_after_stream_executed_listener(self, action: Callable[[dict], None]) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_4_5
    def no_caching(self) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_5_5
    def no_tracking(self) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_6_5

    # WARNING: Not implemented yet!
    # https://issues.hibernatingrhinos.com/issue/RDBC-817/Implement-query.projection

    def projection(self, projection_behavior: ProjectionBehavior) -> DocumentQuery[_T]: ...

    class ProjectionBehavior(Enum):
        DEFAULT = "Default"
        FROM_INDEX = "FromIndex"
        FROM_INDEX_OR_THROW = "FromIndexOrThrow"
        FROM_DOCUMENT = "FromDocument"
        FROM_DOCUMENT_OR_THROW = "FromDocumentOrThrow"

    # endregion

    # region customize_7_5
    def random_ordering(self, seed: str = None) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_8_5
    def wait_for_non_stale_results(self, wait_timeout: timedelta = None) -> DocumentQuery[_T]: ...

    # endregion

    # region customize_9_5
    def timings(self, timings_callback: Callable[[QueryTimings], None]) -> DocumentQuery[_T]: ...

    # endregion


# region the_index
class Employees_ByFullName(AbstractIndexCreationTask):
    class IndexEntry:
        def __init__(self, full_name: str = None):
            self.full_name = full_name

    def __init__(self):
        super().__init__()
        self.map = (
            "docs.Employees.Select(employee => new { "
            "    full_name = (employee.first_name + ' ') + employee.last_name"
            "})"
        )
        self._store("full_name", FieldStorage.YES)


# endregion
