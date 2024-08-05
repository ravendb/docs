from datetime import datetime, timedelta

from ravendb.documents.queries.time_series import TimeSeriesRawResult

from examples_base import ExampleBase, Company, Employee


class FilterTimeSeriesQuery(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_filter(self):
        with self.embedded_server.get_document_store("FilterTS") as store:
            with store.open_session() as session:
                self.add_companies(session)
                self.add_employees(session)

                base_time = datetime(2020, 5, 17, 0, 0, 0, 0)
                session.time_series_for("employees/1-A", "HeartRates").append_single(
                    base_time + timedelta(minutes=1), 76.0, "watches/fitbit"
                )

                session.time_series_for("companies/3", "StockPrices").append_single(
                    base_time + timedelta(minutes=1), 76.0, "employees/stonksguy"
                )

                session.store(Employee(title="Sales Manager"), "employees/stonksguy")

                session.save_changes()
                # region filter_entries_3
                # For example, in the "HeartRates" time series,
                # retrieve only entries where the value exceeds 75 BPM
                base_time = datetime(2020, 5, 17, 0, 0, 0, 0)
                from_dt = base_time
                to_dt = base_time + timedelta(minutes=10)

                query_string = """
                from Employees
                select timeseries (
                    from HeartRates
                    between $from and $to
                    // Use the 'where Value' clause to filter by the value
                    where Value > 75
                )"""

                query = (
                    session.advanced.raw_query(query_string, TimeSeriesRawResult)
                    .add_parameter("from", from_dt)
                    .add_parameter("to", to_dt)
                )

                results = list(query)
                # endregion

                # region filter_entries_6
                # Retrieve only entries where the tag string content is "watches/fitbit"
                base_time = datetime(2020, 5, 17, 0, 0, 0, 0)
                from_dt = base_time
                to_dt = base_time + timedelta(minutes=10)

                query_string = """
                from Employees
                select timeseries (
                    from HeartRates 
                    between $from and $to 
                    // Use the 'where Tag' clause to filter entries by the tag string content
                    where Tag == 'watches/fitbit'
                )"""

                query = (
                    session.advanced.raw_query(query_string, TimeSeriesRawResult)
                    .add_parameter("from", from_dt)
                    .add_parameter("to", to_dt)
                )

                results = list(query)
                # endregion

                # region filter_entries_9
                # retrieve only entries where the tag string content is one of several options

                base_time = datetime(2020, 5, 17, 0, 0, 0, 0)
                from_dt = base_time
                to_dt = base_time + timedelta(minutes=10)

                optional_tags = ["watches/apple", "watches/samsung", "watches/xiaomi"]

                query_string = """
                from Employees 
                select timeseries (
                    from HeartRates
                    between $from and $to
                    // Use the 'where Tag in' clause to filter by various tag options
                    where Tag in ($optionalTags)
                )"""

                query = (
                    session.advanced.raw_query(query_string, TimeSeriesRawResult)
                    .add_parameter("from", from_dt)
                    .add_parameter("to", to_dt)
                    .add_parameter("optionalTags", optional_tags)
                )

                results = list(query)
                # endregion

                # region filter_entries_12
                # retrieve entries that reference a document that has "Sales Manager" in its 'Title' property

                query_string = """
                from Companies
                where Address.Country == 'USA'
                select timeseries (
                    from StockPrices
                    // Use 'load Tag' to load the employee document referenced in the tag
                    load Tag as employeeDoc
                    // Use 'where <property>' to filter entries by the properties of the loaded document
                    where employeeDoc.Title == 'Sales Manager'
                )"""

                query = session.advanced.raw_query(query_string, Company)

                results = list(query)
                # endregion
                pass
