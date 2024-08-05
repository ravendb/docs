from datetime import datetime

from ravendb.documents.operations.time_series import (
    RawTimeSeriesPolicy,
    TimeSeriesPolicy,
    TimeSeriesCollectionConfiguration,
    TimeSeriesConfiguration,
    ConfigureTimeSeriesOperation,
)
from ravendb.primitives.time_series import TimeValue

from examples_base import ExampleBase


class RollupAndRetention(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_rollup_and_retention(self):
        with self.embedded_server.get_document_store("RollupAndRetention") as store:
            with store.open_session() as session:
                # region rollup_and_retention_0
                # Policy for the original ("raw") time-series,
                # to keep the data for one week
                one_week = TimeValue.of_days(7)
                raw_retention = RawTimeSeriesPolicy(one_week)

                # Roll-up the data for each day,
                # and keep the results for one year
                one_day = TimeValue.of_days(1)
                one_year = TimeValue.of_years(1)
                daily_rollup = TimeSeriesPolicy("DailyRollupForOneYear", one_day, one_year)

                # Enter the above policies into a
                # time-series collection configuration
                # for the collection 'Sales'
                sales_ts_config = TimeSeriesCollectionConfiguration(policies=[daily_rollup], raw_policy=raw_retention)

                # Enter the configuration for the Sales collection
                # into a time-series configuration for the whole database
                database_ts_config = TimeSeriesConfiguration()
                database_ts_config.collections["Sales"] = sales_ts_config

                # Send the time-series configuration to the server
                store.maintenance.send(ConfigureTimeSeriesOperation(database_ts_config))
                # endregion

                # region rollup_and_retention_1
                # Create local instance of the time-series "rawSales"
                # in the document "sales/1"
                raw_ts = session.time_series_for("sales/1", "rawSales")

                # Create local instance of the rollup time-series - first method:
                daily_rollup_TS = session.time_series_for("sales/1", "rawSales@DailyRollupForOneYear")

                # Create local instance of the rollup time-series - second method:
                # using the rollup policy itself and the raw time-series' name
                rollup_time_series_2 = session.time_series_for("sales/1", daily_rollup.get_time_series_name("rawSales"))

                # Retrieve all the data from both time-series
                raw_data = raw_ts.get(datetime.min, datetime.max)
                rollup_data = daily_rollup_TS.get(datetime.min, datetime.max)
                # endregion
