from datetime import datetime

from ravendb.documents.indexes.time_series import AbstractTimeSeriesIndexCreationTask

from examples_base import ExampleBase


class QueryingTsIndex(ExampleBase):
    def setUp(self):
        super().setUp()

    # region sample_ts_index
    class TsIndex(AbstractTimeSeriesIndexCreationTask):
        # The index-entry:
        # ===============
        class IndexEntry:
            def __init__(
                self,
                bpm: float = None,
                date: datetime = None,
                tag: str = None,
                employee_id: str = None,
                employee_name: str = None,
            ):
                # The index-fields:
                # =================
                self.bpm = bpm
                self.date = date
                self.tag = tag
                self.employee_id = employee_id
                self.employee_name = employee_name

        def __init__(self):
            super().__init__()
            self.map = """
            from ts in timeSeries.Employees.HeartRates
            from entry in ts.Entries
            let employee = LoadDocument(ts.DocumentId, "Employees")
            select new 
            {
                bpm = entry.Values[0],
                date = entry.Timestamp.Date,
                tag = entry.Tag,
                employee_id = ts.DocumentId,
                employee_name = employee.FirstName + ' ' + employee.LastName
            }
            """

    # endregion

    # region employee_details_class
    # This class is used when projecting index-fields via DocumentQuery
    class EmployeeDetails:
        def __init__(self, employee_name: str = None, employee_id: str = None):
            self.employee_name = employee_name
            self.employee_id = employee_id

    # endregion

    def test_querying_ts_index(self):
        with self.embedded_server.get_document_store("TSIndexQuerying") as store:
            self.TsIndex().execute(store)
            # region query_index_1
            with store.open_session() as session:
                results = list(session.query_index_type(self.TsIndex, self.TsIndex.IndexEntry))

                # Access results:
                entry_result = results[0]
                employee_name = entry_result.employee_name
                bmp = entry_result.bpm
            # endregion
            # region query_index_4
            with store.open_session() as session:
                results = list(session.advanced.raw_query("from index 'TsIndex'", self.TsIndex.IndexEntry))
            # endregion

            # region query_index_5
            with store.open_session() as session:
                results = list(
                    session.query_index_type(self.TsIndex, self.TsIndex.IndexEntry)
                    .where_equals("employee_name", "Robert King")
                    .and_also()
                    .where_greater_than("bpm", 85)
                )
            # endregion

            # region query_index_8
            with store.open_session() as session:
                results = list(
                    session.advanced.raw_query(
                        "from index 'TsIndex' where employee_name == 'Robert King' and bpm > 85.0",
                        self.TsIndex.IndexEntry,
                    )
                )
            # endregion

            # region query_index_9
            with store.open_session() as session:
                results = list(
                    session.query_index_type(self.TsIndex, self.TsIndex.IndexEntry)
                    .where_less_than("bpm", 58)
                    .order_by_descending("date")
                )
            # endregion

            # region query_index_12

            with store.open_session() as session:
                results = list(
                    session.advanced.raw_query(
                        "from index 'TsIndex' where bpm < 58.0 order by date desc",
                        self.TsIndex.IndexEntry,
                    )
                )
            # endregion

            # region query_index_13

            with store.open_session() as session:
                results = list(
                    session.query_index_type(self.TsIndex, self.TsIndex.IndexEntry)
                    .where_greater_than("bpm", 100)
                    .select_fields(self.EmployeeDetails, "employee_id")
                    .distinct()
                )
            # endregion

            # region query_index_16

            with store.open_session() as session:
                results = list(
                    session.advanced.raw_query(
                        "from index 'TsIndex' where bpm > 100.0 select distinct employee_id",
                        self.TsIndex.IndexEntry,
                    )
                )
            # endregion
