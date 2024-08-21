import abc
import math
import random
from datetime import datetime, timedelta
from typing import Dict, Tuple, Optional, List, TypeVar, Union, Callable, Type, Any

from ravendb import (
    PatchCommandData,
    PatchRequest,
    PatchOperation,
    PatchByQueryOperation,
    IndexQuery,
    IncludeBuilderBase,
    IncludeBuilder,
    RawDocumentQuery,
    CommandData,
    PatchResult,
    QueryOperationOptions,
    DocumentQuery,
)
from ravendb.documents.indexes.time_series import AbstractTimeSeriesIndexCreationTask
from ravendb.documents.operations.definitions import VoidOperation, IOperation, OperationIdResult
from ravendb.documents.operations.time_series import (
    GetTimeSeriesOperation,
    GetMultipleTimeSeriesOperation,
    TimeSeriesOperation,
    TimeSeriesBatchOperation,
    TimeSeriesRangeResult,
    TimeSeriesDetails,
    ConfigureTimeSeriesValueNamesOperation,
)
from ravendb.documents.queries.index_query import Parameters
from ravendb.documents.queries.time_series import TimeSeriesAggregationResult, TimeSeriesRawResult
from ravendb.documents.session.loaders.include import TimeSeriesIncludeBuilder
from ravendb.documents.session.time_series import (
    ITimeSeriesValuesBindable,
    TimeSeriesRange,
    TimeSeriesEntry,
    TypedTimeSeriesEntry,
    AbstractTimeSeriesRange,
)
from ravendb.primitives.constants import int_max

from examples_base import ExampleBase, User, Company, Address, Employee

_T_TS_Values_Bindable = TypeVar("_T_TS_Values_Bindable")
_T = TypeVar("_T")
_T_Collection = TypeVar("_T_Collection")


class TimeSeries(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_time_series_testing(self):
        with self.embedded_server.get_document_store("TimeSeriesTesting") as store:
            base_line = datetime.utcnow()

            # Open a session
            with store.open_session() as session:
                # Use the session to create a document
                session.store(User(name="John"), "users/john")

                # Create an instance of TimeSeriesFor
                # Pass an explicit document ID to the TimeSeriesFor constructor
                # Append a HeartRate of 70 at the first-minute timestamp
                session.time_series_for("users/john", "HeartRates").append_single(
                    base_line + timedelta(minutes=1), 70.0, "watches/fitbit"
                )
                session.save_changes()

            # Get time series names
            # region timeseries_region_Retrieve-TimeSeries-Names
            # Open a session
            with store.open_session() as session:
                # Load a document entity to the session
                user = session.load("users/john")

                # Call GetTimeSeriesFor, pass the entity
                ts_names = session.advanced.get_time_series_for(user)

                # Results will include the names of all time series associated with document 'users/john'
            # endregion

            with store.open_session() as session:
                # Use the session to load a document
                user = session.load("users/john")

                # Pass the document object returned from session.load as a param
                # Retrieve a single value from the "HeartRates" time series
                val = session.time_series_for_entity(user, "HeartRates").get(datetime.min, datetime.max)

            # region timeseries_region_Delete-TimeSeriesFor-Single-Time-Point
            # Delete a single entry
            with store.open_session() as session:
                session.time_series_for("users/john", "HeartRates").delete(base_line + timedelta(minutes=1))
                session.save_changes()
            # endregion

            store.time_series.register_type(User, HeartRate)
            store.time_series.register_type(User, StockPrice)
            # region timeseries_region_Named-Values-Register
            store.time_series.register_type(User, RoutePoint)
            # endregion

            with store.open_session() as session:
                # region timeseries_region_Append-Named-Values-1
                # Append coordinates
                session.typed_time_series_for(RoutePoint, "users/john").append_single(
                    base_line + timedelta(hours=1), RoutePoint(40.712776, -74.005974), "devices/Navigator"
                )
                # endregion

                session.typed_time_series_for(RoutePoint, "users/john").append_single(
                    base_line + timedelta(hours=2),
                    RoutePoint(latitude=40.712781, longitude=-74.005979),
                    "devices/Navigator",
                )
                session.typed_time_series_for(RoutePoint, "users/john").append_single(
                    base_line + timedelta(hours=3),
                    RoutePoint(latitude=40.712789, longitude=-74.005979),
                    "devices/Navigator",
                )
                session.typed_time_series_for(RoutePoint, "users/john").append_single(
                    base_line + timedelta(hours=4),
                    RoutePoint(latitude=40.712792, longitude=-74.005979),
                    "devices/Navigator",
                )
                session.save_changes()

            # Get entries
            with store.open_session() as session:
                # Use the session to load a document
                user = session.load("users/john")

                # Pass the document object returned from session.Load as a param
                # Retrieve a single value from the "HeartRates" time series
                results = session.typed_time_series_for(RoutePoint, "users/john").get()

            # append entries
            with store.open_session() as session:
                session.store(User(name="John"), key="users/john")

                # Append a HeartRate entry
                session.time_series_for("users/john", "HeartRates").append_single(
                    base_line + timedelta(minutes=1), 70.0, "watches/fitbit"
                )

                session.save_changes()

            # append entries using a registered time series type
            with store.open_session() as session:
                session.store(User(name="John"), "users/john")

                session.typed_time_series_for(HeartRate, "users/john").append_single(
                    base_line, HeartRate(80), "watches/anotherFirm"
                )
                session.save_changes()

            # append multi-value entries by name
            # region timeseries_region_Append-Named-Values-2
            with store.open_session() as session:
                session.store(User(name="John"), "users/john")

                session.typed_time_series_for(StockPrice, "users/john").append_single(
                    base_line + timedelta(days=1), StockPrice(52, 54, 63.5, 51.4, 9824), "companies/kitchenAppliances"
                )

                session.typed_time_series_for(StockPrice, "users/john").append_single(
                    base_line + timedelta(days=2), StockPrice(54, 55, 61.5, 49.4, 8400), "companies/kitchenAppliances"
                )

                session.typed_time_series_for(StockPrice, "users/john").append_single(
                    base_line + timedelta(days=3), StockPrice(55, 57, 65.5, 50, 9020), "companies/kitchenAppliances"
                )

                session.save_changes()
            # endregion

            # region timeseries_region_Append-Unnamed-Values-2
            with store.open_session() as session:
                session.store(User(name="John"), "users/john")

                session.time_series_for("users/john", "StockPrices").append(
                    base_line + timedelta(days=1), [52, 54, 63.5, 51.4, 9824], "companies/kitchenAppliances"
                )

                session.time_series_for("users/john", "StockPrices").append(
                    base_line + timedelta(days=2), [54, 55, 61.5, 49.4, 8400], "companies/kitchenAppliances"
                )

                session.time_series_for("users/john", "StockPrices").append(
                    base_line + timedelta(days=3), [55, 57, 65.5, 50, 9020], "companies/kitchenAppliances"
                )

                session.save_changes()
            # endregion

            # append multi-value entries using a registered time series type
            with store.open_session() as session:
                session.store(
                    Company(name="kitchenAppliances", address=Address(city="New York")), "companies/kitchenAppliances"
                )

                session.typed_time_series_for(StockPrice, "companies/kitchenAppliances").append(
                    base_line + timedelta(days=1), [52, 54, 63.5, 51.4, 9824], "companies/kitchenAppliances"
                )
                session.typed_time_series_for(StockPrice, "companies/kitchenAppliances").append(
                    base_line + timedelta(days=2), [54, 55, 61.5, 49.4, 7400], "companies/kitchenAppliances"
                )
                session.typed_time_series_for(StockPrice, "companies/kitchenAppliances").append(
                    base_line + timedelta(days=3), [55, 57, 65.5, 50, 9020], "companies/kitchenAppliances"
                )

                session.save_changes()

            # get entries
            with store.open_session() as session:
                # Use the session to load a document
                user = session.load("users/john")

                # Pass the document object returned from session.Load as a param
                # Retrieve a single value from the "HeartRates" time series
                val = session.time_series_for_entity(user, "HeartRates").get(datetime.min, datetime.max)

            # region timeseries_region_Get-NO-Names-Values
            # Use Get without a named type
            # Is the stock's closing-price rising?
            going_up = False

            with store.open_session() as session:
                val = session.time_series_for("users/john", "StockPrices").get()

                close_price_day_1 = val[0].values[1]
                close_price_day_2 = val[1].values[1]
                close_price_day_3 = val[2].values[1]

                if close_price_day_2 > close_price_day_1 and close_price_day_3 > close_price_day_2:
                    going_up = True
            # endregion

            # region timeseries_region_Get-Named-Values
            going_up = False

            # Use Get with a Named type
            with store.open_session() as session:
                val = session.typed_time_series_for(StockPrice, "users/john").get()

                close_price_day_1 = val[0].value.close
                close_price_day_2 = val[1].value.close
                close_price_day_3 = val[2].value.close
                if close_price_day_2 > close_price_day_1 and close_price_day_3 > close_price_day_2:
                    going_up = True
            # endregion

            # remove entries using a registered time series type
            with store.open_session() as session:
                session.time_series_for("users/john", "HeartRates").delete(
                    base_line + timedelta(days=1), base_line + timedelta(days=2)
                )
                session.save_changes()

            # query
            # Query for a document with the Name property "John" and append it a time point
            with store.open_session() as session:
                query = session.query(object_type=User).where_equals("Name", "John")

                result = list(query)

                session.time_series_for_entity(result[0], "HeartRates").append_single(
                    base_line + timedelta(minutes=1), 72, "watches/fitbit"
                )

                session.save_changes()

            # region timeseries_region_Pass-TimeSeriesFor-Get-Query-Results
            # Query for a document with the Name property "John"
            # and get its HeartRates time-series values
            with store.open_session() as session:
                base_line = datetime.utcnow()

                query = session.query(object_type=User).where_equals("Name", "John")

                result = list(query)

                doc_id = session.advanced.get_document_id(result[0])

                val = session.time_series_for(doc_id, "HeartRates").get(datetime.min, datetime.max)

                session.save_changes()
            # endregion

            # Query for a document with the Name property "John" and append it a time point
            with store.open_session() as session:
                base_line = datetime.utcnow()
                query = session.query(object_type=User).where_equals("Name", "John")
                result = list(query)
                document_id = session.advanced.get_document_id(result[0])
                for cnt in range(10):
                    session.time_series_for(document_id, "HeartRates").append_single(
                        base_line + timedelta(minutes=cnt), 72, "watches/fitbit"
                    )

                session.save_changes()

            # region timeseries_region_Load-Document-And-Include-TimeSeries
            with store.open_session() as session:
                base_line = datetime.utcnow()

                # Load a document
                user = session.load(
                    "users/john",
                    User,
                    lambda include_builder:
                    # Call 'include_time_series' to include time series entries, pass:
                    # * The time series name
                    # * Start and end timestamps indicating the range of entries to include
                    include_builder.include_time_series(
                        "HeartRates", base_line + timedelta(minutes=3), base_line + timedelta(minutes=8)
                    ),
                )

                # The following call to 'get' will not trigger a server request,
                # the entries will be retrieved from the session's cache.
                entries = session.time_series_for("users/john", "HeartRates").get()
            # endregion

            # region timeseries_region_Query-Document-And-Include-TimeSeries
            with store.open_session() as session:
                # Query for a document and include a whole time-series
                user = (
                    session.query(object_type=User)
                    .where_equals("Name", "John")
                    .include(
                        lambda builder: builder
                        # Call 'IncludeTimeSeries' to include time series entries, pass:
                        # * The time series name
                        # * Start and end timestamps indicating the range of entries to include
                        .include_time_series(
                            "HeartRates", base_line + timedelta(minutes=3), base_line + timedelta(minutes=8)
                        )
                    )
                )
            # endregion

            # region timeseries_region_Raw-Query-Document-And-Include-TimeSeries
            with store.open_session() as session:
                base_time = datetime.utcnow()
                from_time = base_time
                to_time = base_time + timedelta(minutes=5)

                # Define the Raw Query:
                query = (
                    session.advanced.raw_query("from Users include timeseries('HeartRates', $from, $to)", User)
                    .add_parameter("from", from_time)
                    .add_parameter("to", to_time)
                )

                # Execute the query:
                # For each document in the query results,
                # the time series entries will be 'loaded' to the session along with the document
                users = list(query)

                # The following call to 'Get' will Not trigger a server request,
                # the entries will be retrieved from the session's cache.
                entries = session.time_series_for_entity(users[0], "HeartRates").get(from_time, to_time)
            # endregion

            # Open a session
            with store.open_session() as session:
                # Use the session to create a document
                session.store(User(name="John"), "users/john")

                session.time_series_for("users/john", "HeartRates").append(
                    base_line + timedelta(minutes=1), [65.0, 52.0, 72.0], "watches/fitbit"
                )
                session.save_changes()

            with store.open_session() as session:
                # Use the session to load a document
                user = session.load("users/john")

                # Pass the document object returned from session.load as a param
                # Retrieve a single value from the "HeartRates" time series
                val = session.time_series_for_entity(user, "HeartRates").get(datetime.min, datetime.max)

            # Get time series HeartRates' time points data
            with store.open_session() as session:
                # region timeseries_region_Get-All-Entries-Using-Document-ID
                # Get all time series entries
                val = session.time_series_for("users/john", "HeartRates").get(datetime.min, datetime.max)
                # endregion

            # Get time series HeartRate's time points data
            with store.open_session() as session:
                # region IncludeParentAndTaggedDocuments
                # Get all time series entries
                entries = session.time_series_for("users/john", "HeartRates").get_with_include(
                    datetime.min,
                    datetime.max,
                    lambda builder: builder
                    # Include documents referred-to by entry tags
                    .include_tags()
                    # Include Parent Document
                    .include_document(),
                )
                # endregion

            # region timeseries_region_TimeSeriesFor-Append-TimeSeries-Range
            base_line = datetime.utcnow()

            # Append 10 HeartRate values
            with store.open_session() as session:
                session.store(User(name="John"), "users/john")

                tsf = session.time_series_for("users/john", "HeartRates")

                for i in range(10):
                    tsf.append_single(base_line + timedelta(seconds=i), 67.0, "watches/fitbit")

                session.save_changes()

            # endregion

            # region timeseries_region_TimeSeriesFor-Delete-Time-Points_Range
            # Delete a range of entries from the time series
            with store.open_session() as session:
                session.time_series_for("users/john", "HeartRates").delete(base_line, base_line + timedelta(seconds=9))
                session.save_changes()
            # endregion

            # Use GetTimeSeriesOperation and GetMultipleTimeSeriesOperation
            # Create a document
            with store.open_session() as session:
                employee_1 = Employee(first_name="John")
                session.store(employee_1)

                employee_2 = Employee(first_name="Mia")
                session.store(employee_2)

                employee_3 = Employee(first_name="Emil")
                session.store(employee_3)

                session.save_changes()

            # get employees Id list
            with store.open_session() as session:
                employee_id_list = [
                    session.advanced.get_document_id(employee) for employee in list(session.query(object_type=Employee))
                ]

                # Append each employee a week (168 hours) of random exercise HeartRate values and a week (168 hours) of random rest HeartRate values
                base_time = datetime(year=2020, month=5, day=17)
                random_values = random.Random()

                with store.open_session() as session:
                    for emp_id in employee_id_list:
                        for tse in range(168):
                            session.time_series_for(emp_id, "ExerciseHeartRate").append_single(
                                base_time + timedelta(hours=tse),
                                68 + round(19 * random_values.uniform(0.0, 3.0)),
                                "watches/fitbit",
                            )
                            session.time_series_for(emp_id, "Rest").append_single(
                                base_time + timedelta(hours=tse),
                                52 + round(19 * random_values.uniform(0.0, 3.0)),
                                "watches/fitbit",
                            )

                    session.save_changes()

                document_id = "employees/1-A"

                # region timeseries_region_Get-Single-Time-Series
                # Get all values of a single time-series
                single_time_series_details = store.operations.send(
                    GetTimeSeriesOperation(
                        document_id,
                        "HeartRates",
                        datetime.min,
                        datetime.max,
                    )
                )
                # endregion

                # region timeseries_region_Get-Multiple-Time-Series
                multiple_time_series_details = store.operations.send(
                    GetMultipleTimeSeriesOperation(
                        document_id,
                        [
                            TimeSeriesRange(
                                "ExerciseHeartRate", base_time + timedelta(hours=1), base_time + timedelta(hours=10)
                            ),
                            TimeSeriesRange(
                                "RestHeartRate", base_time + timedelta(hours=11), base_time + timedelta(hours=20)
                            ),
                        ],
                    )
                )
                # endregion

                # region timeseries_region_Append-Using-TimeSeriesBatchOperation
                base_time = datetime.utcnow()

                # Define the Append operations:
                # =============================
                append_op_1 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=1), [79.0], "watches/fitbit"
                )
                append_op_2 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=2), [82.0], "watches/fitbit"
                )
                append_op_3 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=3), [80.0], "watches/fitbit"
                )
                append_op_4 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=4), [78.0], "watches/fitbit"
                )

                # Define 'TimeSeriesOperation' and add the Append operations:
                # ===========================================================
                time_series_op = TimeSeriesOperation(name="HeartRates")

                time_series_op.append(append_op_1)
                time_series_op.append(append_op_2)
                time_series_op.append(append_op_3)
                time_series_op.append(append_op_4)

                # Define 'TimeSeriesBatchOperation' and execute:
                # ==============================================
                time_series_batch_op = TimeSeriesBatchOperation("users/john", time_series_op)
                store.operations.send(time_series_batch_op)
                # endregion

                # region timeseries_region_Delete-Range-Using-TimeSeriesBatchOperation
                base_time = datetime.utcnow()

                delete_op = TimeSeriesOperation.DeleteOperation(
                    datetime_from=base_time + timedelta(minutes=2), datetime_to=base_time + timedelta(minutes=3)
                )

                time_series_op = TimeSeriesOperation("HeartRates")
                time_series_op.delete(delete_op)

                time_series_batch_op = TimeSeriesBatchOperation("users/john", time_series_op)

                store.operations.send(time_series_batch_op)
                # endregion

                # region timeseries_region-Append-and-Delete-TimeSeriesBatchOperation
                base_time = datetime.utcnow()

                # Define some Append operations:
                append_op_1 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=1), [79.0], "watches/fitbit"
                )
                append_op_2 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=2), [82.0], "watches/fitbit"
                )
                append_op_3 = TimeSeriesOperation.AppendOperation(
                    base_time + timedelta(minutes=3), [80.0], "watches/fitbit"
                )

                # Define a Delete operation:
                delete_op = TimeSeriesOperation.DeleteOperation(
                    base_time + timedelta(minutes=2), base_time + timedelta(minutes=3)
                )

                time_series_op = TimeSeriesOperation("HeartRates")

                # Add the Append & Delete operations to the list of actions
                # Note: the Delete action will be executed BEFORE all the Append actions
                #       even though it is added last

                time_series_op.append(append_op_1)
                time_series_op.append(append_op_2)
                time_series_op.append(append_op_3)
                time_series_op.delete(delete_op)

                time_series_batch_op = TimeSeriesBatchOperation("users/john", time_series_op)

                store.operations.send(time_series_batch_op)

                # Results:
                # All 3 entries that were appended will exist and are not deleted.
                # This is because the Delete action occurs first, before all Append actions.
                # endregion

            # Query for document with the Name property "John" and append it a time point
            with store.open_session() as session:
                query = session.query(object_type=User).where_equals("Name", "John")
                result = list(query)
                document_id = session.advanced.get_document_id(result[0])

            # region timeseries_region_Use-BulkInsert-To-Append-2-Entries
            # Use BulkInsert to append 2 time-series entries
            with store.bulk_insert() as bulk_insert:
                with bulk_insert.time_series_for(document_id, "HeartRates") as time_series_bulk_insert:
                    time_series_bulk_insert.append_single(base_line + timedelta(minutes=2), 61, "watches/fitbit")
                    time_series_bulk_insert.append_single(base_line + timedelta(minutes=3), 62, "watches/apple-watch")
            # endregion

            # region timeseries_region_Use-BulkInsert-To-Append-100-Entries
            # Use BulkInsert to append 100 time-series entries
            with store.bulk_insert() as bulk_insert:
                with bulk_insert.time_series_for(document_id, "HeartRates") as time_series_bulk_insert:
                    for minute in range(0, 100):
                        time_series_bulk_insert.append(base_line + timedelta(minutes=minute), [80], "watches/fitbit")
            # endregion

            # Query for a document with the Name property "John" and append it a time point
            with store.open_session() as session:
                query = session.query(object_type=User).where_equals("Name", "John")
                result = list(query)

                document_id = session.advanced.get_document_id(result[0])

            # region BulkInsert-overload-2-Two-HeartRate-Sets
            # Use BulkInsert to append 2 sets of time series entries
            with store.bulk_insert() as bulk_insert:
                exercise_heart_rate = [89.0, 82.0, 85.0]
                resting_heart_rate = [59.0, 63.0, 61.0, 64.0, 65.0]

                with bulk_insert.time_series_for(document_id, "HeartRates") as time_series_bulk_insert:
                    time_series_bulk_insert.append(
                        base_line + timedelta(minutes=2), exercise_heart_rate, "watches/fitbit"
                    )
                    time_series_bulk_insert.append(
                        base_line + timedelta(minutes=3), resting_heart_rate, "watches/apple-watch"
                    )
            # endregion

            values = [59.0, 63.0, 69.0, 64, 65.0]

            # Use BulkInsert to append 100 multi-values time-series entries
            with store.bulk_insert() as bulk_insert:
                with bulk_insert.time_series_for(document_id, "HeartRates") as time_series_bulk_insert:
                    for minute in range(100):
                        time_series_bulk_insert.append(base_line + timedelta(minutes=minute), values, "watches/fitbit")

            # Patch a time-series to a document whose Name property is "John"
            with store.open_session() as session:
                query = session.query(object_type=User).where_equals("Name", "John")
                result = list(query)
                document_id = session.advanced.get_document_id(result[0])
                change_vector = session.advanced.get_change_vector_for(result[0])
                values = [59.0]
                tag = "watches/fitbit"
                timeseries = "HeartRates"
                # session.advanced.defer(
                #     PatchCommandData(
                #         document_id,
                #         change_vector,
                #         PatchRequest(
                #             (
                #                 "timeseries(this, $timeseries)"
                #                 ".append("
                #                 "    $timestamp,"
                #                 "    $values,"
                #                 "    $tag"
                #                 ");"
                #             ),  # 'tag' should appear last
                #             {
                #                 "timeseries": timeseries,
                #                 "timestamp": base_line + timedelta(minutes=1),
                #                 "values": values,
                #                 "tag": tag,
                #             },
                #         ),
                #         None,
                #     )
                # )
                #
                # session.save_changes()

            # region TS_region-Operation_Patch-Append-Single-TS-Entry
            base_line = datetime.utcnow()

            store.operations.send(
                PatchOperation(
                    "users/1-A",
                    None,
                    PatchRequest(
                        "timeseries(this, $timeseries).append($timestamp, $values, $tag);",
                        {
                            "timeseries": "HeartRates",
                            "timestamp": base_line + timedelta(minutes=1),
                            "values": 59.0,
                            "tag": "watches/fitbit",
                        },
                    ),
                )
            )
            # endregion
            # Patching: Append and Remove multiple time-series entries
            # Using session.Advanced.Defer
            with store.open_session() as session:
                # region TS_region-Session_Patch-Append-100-Random-TS-Entries
                base_line = datetime.utcnow()

                # Create arrays of timestamps and random values to patch
                values = []
                time_stamps = []

                for i in range(100):
                    values.append(68 + round(19 * random.uniform(0.0, 1.0)))
                    time_stamps.append(base_line + timedelta(seconds=i))

                session.advanced.defer(
                    PatchCommandData(
                        "users/1-A",
                        None,
                        PatchRequest(
                            script=(
                                "var i = 0;"
                                "for(i = 0; i < $values.length; i++)"
                                "{"
                                "    timeseries(id(this), $timeseries)"
                                "    .append ("
                                "      new Date($time_stamps[i]),"
                                "      $values[i],"
                                "      $tag);"
                                "}"
                            ),
                            values={
                                "timeseries": "HeartRates",
                                "time_stamps": time_stamps,
                                "values": values,
                                "tag": "watches/fitbit",
                            },
                        ),
                        None,
                    )
                )

                session.save_changes()
                # endregion

                # region TS_region-Session_Patch-Delete-50-TS-Entries
                # Delete time-series entries
                session.advanced.defer(
                    PatchCommandData(
                        "users/1-A",
                        None,
                        PatchRequest(
                            script=("timeseries(this, $timeseries)" ".delete(" "  $from," "  $to" ");"),
                            values={
                                "timeseries": "HeartRates",
                                "from": base_line,
                                "to": base_line + timedelta(seconds=49),
                            },
                        ),
                        None,
                    )
                )
                # endregion
                # region TS_region-Session_Patch-Append-TS-Entries

                session.advanced.defer(
                    PatchCommandData(
                        "users/1-A",
                        None,
                        PatchRequest(
                            (
                                "var i = 0;"
                                "for(i = 0; i < $values.length; i++)"
                                "{"
                                "    timeseries(id(this), $timeseries)"
                                "    .append ("
                                "      new Date($time_stamps[i]),"
                                "      $values[i],"
                                "      $tag);"
                                "}"
                            ),
                            {
                                "timeseries": "HeartRates",
                                "time_stamps": time_stamps,
                                "values": values,
                                "tag": "watches/fitbit",
                            },
                        ),
                        None,
                    )
                )

                session.save_changes()
                # endregion

            # Patch a document 100 time-series entries
            with store.open_session() as session:
                # region TS_region-Operation_patch-Append-100-TS-Entries
                base_line = datetime.utcnow()

                # Create arrays of timestamps and random values to patch
                values = []
                time_stamps = []

                for cnt in range(100):
                    values.append(68 + round(19 * random.uniform(0.0, 1.0)))
                    time_stamps.append(base_line + timedelta(seconds=cnt))

                store.operations.send(
                    PatchOperation(
                        "users/1-A",
                        None,
                        PatchRequest(
                            "var i = 0;"
                            "for (i = 0; i < $values.length; i++)"
                            "{timeseries(id(this), $timeseries)"
                            ".append ("
                            "           new Date($time_stamps[i]),"
                            "           $values[i],"
                            "           $tag);"
                            "}",
                            {
                                "timeseries": "HeartRates",
                                "time_stamps": time_stamps,
                                "values": values,
                                "tag": "watches/fitbit",
                            },
                        ),
                    )
                )
                # endregion

            # Query for a document with the Name property "John" and append it a time point
            with store.open_session() as session:
                query = session.query(object_type=User).where_equals("Name", "John")
                result = list(query)

                for cnt in range(120):
                    document_id = session.advanced.get_document_id(result[0])
                    session.time_series_for(document_id, "HeartRates").append_single(
                        base_line + timedelta(days=cnt), 72.0, "watches/fitbit"
                    )

                session.save_changes()

            # todo reeb: skipped the region here (ts_region_LINQ-6-Aggregation) because I lack .Select() and ts aggregations like this

            # Raw Query
            with store.open_session() as session:
                base_line = datetime.utcnow()

                start = base_line
                end = base_line + timedelta(hours=1)

                query = (
                    session.advanced.raw_query(
                        object_type=User, query="from Users include timeseries('HeartRates', $start, $end)"
                    )
                    .add_parameter("start", start)
                    .add_parameter("end", end)
                )

                # Raw Query with aggregation
                aggregated_raw_query = session.advanced.raw_query(
                    object_type=TimeSeriesAggregationResult,
                    query="from Users as u where Age < 30 "
                    "select timeseries("
                    "    from HeartRates between"
                    "        '2020-05-27T00:00:00.0000000Z'"
                    "            and '2020-06-23T00:00.0000000Z'"
                    "    group by '7 days'"
                    "    select min(), max())",
                )

                aggregated_raw_query_result = list(aggregated_raw_query)

                # region ts_region_Raw-RQL-Select-Syntax-Aggregation-and-Projections-StockPrice
                aggregated_raw_query = session.advanced.raw_query(
                    "from Companies as c where c.Address.Country = 'USA' "
                    "select timeseries ("
                    "    from StockPrices"
                    "    where Values[4] > 500000"
                    "        group by '7 day'"
                    "        select max(), min()"
                    ")",
                    TimeSeriesAggregationResult,
                )
                aggregated_raw_query_result = list(aggregated_raw_query)
                # endregion

                # region ts_region_Raw-RQL-Declare-Syntax-Aggregation-and-Projections-StockPrice
                aggregated_raw_query = session.advanced.raw_query(
                    object_type=TimeSeriesAggregationResult,
                    query="""
                declare timeseries SP(c) {
                    from c.StockPrices
                    where Values[4] > 500000
                    group by '7 day'
                    select max(), min()
                }
                from Companies as c
                where c.Address.Country = 'USA'
                select c.Name, SP(c)
                """,
                )

                aggregated_raw_query_result = list(aggregated_raw_query)
                # endregion
                # region ts_region_Raw-Query-Non-Aggregated-Declare-Syntax
                base_time = datetime(2020, 5, 17, 00, 00, 00)  # May 17 2020, 00:00:00

                # Raw query with no aggregation - Declare syntax
                query = (
                    session.advanced.raw_query(
                        object_type=TimeSeriesRawResult,
                        query="""
                            declare timeseries getHeartRates(user) 
                            {
                                from user.HeartRates 
                                    between $start and $end
                                    offset '02:00'
                            }
                            from Users as u where Age < 30
                            select getHeartRates(u)
                            """,
                    )
                    .add_parameter("start", base_time)
                    .add_parameter("end", base_time + timedelta(hours=24))
                )

                results = list(query)
                # endregion

            with store.open_session() as session:
                # region ts_region_Raw-Query-Aggregated
                base_line = datetime(2020, 5, 17, 00, 00, 00)

                # Raw Query with aggregation
                query = (
                    session.advanced.raw_query(
                        object_type=TimeSeriesAggregationResult,
                        query="""
                from Users as u
                select timeseries(
                    from HeartRates 
                        between $start and $end
                    group by '1 day'
                    select min(), max()
                    offset '03:00')
                """,
                    )
                    .add_parameter("start", base_line)
                    .add_parameter("end", base_line + timedelta(days=7))
                )

                result = list(query)
                # endregion

            with store.open_session() as session:
                # region ts_region_LINQ-2-RQL-Equivalent
                # Raw query with no aggregation - Select syntax
                query = session.advanced.raw_query(
                    object_type=TimeSeriesRawResult,
                    query="""
                from Users where Age < 30
                select timeseries (
                    from HeartRates
                )
                """,
                )

                results = list(query)
                # endregion

                # region ts_region_Filter-By-load-Tag-Raw-RQL
                non_aggregated_raw_query = session.advanced.raw_query(
                    object_type=TimeSeriesRawResult,
                    query="""
                from Companies as c where c.Address.Country = 'USA'
                select timeseries(
                    from StockPrices
                       load Tag as emp
                       where emp.Title == 'Sales Representative'
                )
                """,
                )

                non_aggregated_raw_query_result = list(non_aggregated_raw_query)
                # endregion

                # region TS_DocQuery_1
                # Define the query:
                query = (
                    session.advanced.document_query(object_type=User)
                    .select_time_series(HeartRate, lambda builder: builder.raw("from HeartRates"))
                    .of_type(TimeSeriesRawResult)
                )

                # Execute the query
                results = list(query)
                # endregion

            with store.open_session() as session:
                # region TS_DocQuery_2
                query = (
                    session.advanced.document_query(object_type=User)
                    .select_time_series(
                        HeartRate,
                        lambda builder: builder.raw("from HeartRates between $from and $to"),
                    )
                    .add_parameter("from", datetime.utcnow())
                    .add_parameter("to", datetime.utcnow() + timedelta(days=1))
                ).of_type(TimeSeriesRawResult)

                results = list(query)
                # endregion

                # region TS_region-PatchByQueryOperation-Append-To-Multiple-Docs
                query = IndexQuery(
                    query="""
                    from Users as u update
                    {
                        timeseries(u, $name).append($time, $values, $tag)
                    }
                    """
                )
                query.query_parameters = Parameters(
                    {
                        "name": "RestHeartRate",
                        "time": base_line + timedelta(minutes=1),
                        "values": [59.0],
                        "tag": "watches/fitbit1",
                    }
                )

                append_rest_heart_rate_operation = PatchByQueryOperation(query)

                store.operations.send(append_rest_heart_rate_operation)
                # endregion

                # region TS_region-PatchByQueryOperation-Delete-From-Multiple-Docs
                # Delete time-series from all users
                index_query = IndexQuery("from Users as u update { timeseries(u, $name).delete($from, $to) }")
                index_query.query_parameters = Parameters(
                    {"name": "HeartRates", "from": datetime.min, "to": datetime.max}
                )
                delete_operation = PatchByQueryOperation(index_query)
                store.operations.send(delete_operation)
                # endregion

                session.store(User(name="shaya"))
                session.save_changes()

            # region TS_region-PatchByQueryOperation-Get
            index_query = IndexQuery(
                query="""
                declare function foo(doc){
                    var entries = timeseries(doc, $name).get($from, $to);
                    var differentTags = [];
                    for (var i = 0; i < entries.length; i++)
                    {
                        var e = entries[i];
                        if (e.Tag !== null)
                        {
                            if (!differentTags.includes(e.Tag))
                            {
                                differentTags.push(e.Tag);
                            }
                        }
                    }
                    doc.NumberOfUniqueTagsInTS = differentTags.length;
                    return doc;
                }

                from Users as u
                update
                {
                    put(id(u), foo(u))
                }
                """
            )
            index_query.query_parameters = Parameters(
                {"name": "ExerciseHeartRate", "from": datetime.min, "to": datetime.max}
            )

            patch_num_of_unique_tags = PatchByQueryOperation(index_query)

            result = store.operations.send_async(patch_num_of_unique_tags).wait_for_completion()
            # endregion

            # De


class HeartRate(ITimeSeriesValuesBindable):
    def __init__(self, heart_rate_measure: float = None):
        self.heart_rate_measure = heart_rate_measure

    def get_time_series_mapping(self) -> Dict[int, Tuple[str, Optional[str]]]:
        return {0: ("heart_rate_measure", None)}


# region Custom-Data-Type-1
class StockPrice(ITimeSeriesValuesBindable):
    def __init__(
        self,
        open: float = None,
        close: float = None,
        high: float = None,
        low: float = None,
        volume: float = None,
    ):
        self.open = open
        self.close = close
        self.high = high
        self.low = low
        self.volume = volume

    def get_time_series_mapping(self) -> Dict[int, Tuple[str, Optional[str]]]:
        return {
            0: ("open", None),  # { index : (name, tag), ... }
            1: ("close", None),
            2: ("high", None),
            3: ("low", None),
            4: ("volume", None),
        }


# endregion


# region Custom-Data-Type-2
class RoutePoint(ITimeSeriesValuesBindable):  # Inherit 'ITimeSeriesValuesBindable'
    def __init__(self, latitude: float = None, longitude: float = None):
        self.latitude = latitude
        self.longitude = longitude

    def get_time_series_mapping(self) -> Dict[int, Tuple[str, Optional[str]]]:
        return {
            0: ("latitude", None),  # { index : (name, tag), ... }
            1: ("longitude", None),
        }


# endregion


# region ts_region_Index-TS-Queries-6-IndexDefinition
class SimpleIndex(AbstractTimeSeriesIndexCreationTask):
    class Result:
        def __init__(self, heart_beat: float = None, date: datetime = None, user: str = None, tag: str = None):
            self.heart_beat = heart_beat
            self.date = date
            self.user = user
            self.tag = tag

    def __init__(self):
        super().__init__()
        self.map = (
            "from ts in timeSeries.Companies.HeartRate "
            "from entry in ts.Entries "
            "select new { "
            "    heart_beat = entry.Values[0],"
            "    date = entry.Timestamp.Date,"
            "    user = ts.DocumentId,"
            "    tag = entry.Tag"
            "}"
        )

    # endregion

    # region DefineCustomFunctions_ModifiedTimeSeriesEntry
    class ModifiedTimeSeriesEntry:
        def __init__(self, timestamp: datetime = None, value: float = None, tag: str = None):
            self.timestamp = timestamp
            self.value = value
            self.tag = tag

    # endregion

    # region TimeSeriesEntry-Definition
    class TimeSeriesEntry:
        def __init__(self, timestamp: datetime = None, tag: str = None, values: List[int] = None, rollup: bool = None):
            self.timestamp = timestamp
            self.tag = tag
            self.values = values
            self.rollup = rollup

        @property
        def value(self):
            if len(self.values) == 1:
                return self.values[0]
            raise ValueError("Entry has more than one value.")

        @value.setter
        def value(self, value: int):
            if len(self.values) == 1:
                self.values[0] = value
                return
            raise ValueError("Entry has more than one value")

    # endregion

    class Foo:
        # region TimeSeriesFor-Append-definition-double
        def append_single(self, timestamp: datetime, value: float, tag: Optional[str] = None) -> None: ...

        # endregion
        # region TimeSeriesFor-Append-definition-inum
        def append(self, timestamp: datetime, values: List[float], tag: Optional[str] = None) -> None: ...

        # endregion
        # region TimeSeriesFor-Delete-definition-single-timepoint
        def delete_at(self, at: datetime) -> None: ...

        # endregion
        # region TimeSeriesFor-Delete-definition-range-of-timepoints
        def delete(self, datetime_from: Optional[datetime] = None, datetime_to: Optional[datetime] = None): ...

        # endregion
        # region TimeSeriesFor-Get-definition
        def get(
            self,
            from_date: Optional[datetime] = None,
            to_date: Optional[datetime] = None,
            start: int = 0,
            page_size: int = int_max,
        ) -> Optional[List[TimeSeriesEntry]]: ...

        # endregion

        # region TimeSeriesFor-Get-Named-Values
        # The strongly-typed API is used, to address time series values by name.
        def get(
            self,
            from_date: Optional[datetime] = None,
            to_date: Optional[datetime] = None,
            start: int = 0,
            page_size: int = int_max,
        ) -> Optional[List[TypedTimeSeriesEntry[_T_TS_Values_Bindable]]]: ...

        # endregion
        # region TimeSeries-Bindable-Abstract-Class-Definition
        class ITimeSeriesValuesBindable(abc.ABC):
            @abc.abstractmethod
            def get_time_series_mapping(self) -> Dict[int, Tuple[str, Optional[str]]]:
                # return a dictionary {index of time series value - (name of 'float' field, label)}
                # e.g. return {0 : ('heart', 'Heartrate'), 1: ('bp', 'Blood Pressure')}
                # for some class that has 'heart' and 'bp' float fields
                raise NotImplementedError()

        # endregion

        # region Load-definition
        def load(
            self,
            key_or_keys: Union[List[str], str],
            object_type: Optional[Type[_T]] = None,
            includes: Callable[[IncludeBuilder], None] = None,
        ) -> Union[Dict[str, _T], _T]: ...

        # endregion

        # region IncludeTimeSeries-definition
        def include_time_series(
            self,
            name: str,
            from_date: Optional[datetime] = None,
            to_date: Optional[datetime] = None,
            alias: Optional[str] = "",
        ) -> IncludeBuilderBase: ...

        # endregion
        # region RawQuery-Definition
        def raw_query(self, query: str, object_type: Optional[Type[_T]] = None) -> RawDocumentQuery[_T]: ...

        # endregion

        # region PatchCommandData-definition
        class PatchCommandData(CommandData):
            def __init__(
                self,
                key: str,
                change_vector: Union[None, str],
                patch: PatchRequest,
                patch_if_missing: Optional[PatchRequest] = None,
            ): ...

        # endregion

        # region PatchRequest-definition
        class PatchRequest:
            def __init__(self, script: Optional[str] = "", values: Optional[Dict[str, object]] = None): ...

        # endregion

        # region TimeSeriesBatchOperation-definition
        class TimeSeriesBatchOperation(VoidOperation):
            def __init__(self, document_id: str, operation: TimeSeriesOperation): ...

        # endregion

        # region GetTimeSeriesOperation-Definition
        class GetTimeSeriesOperation(IOperation[TimeSeriesRangeResult]):
            def __init__(
                self,
                doc_id: str,
                time_series: str,
                from_date: datetime = None,
                to_date: datetime = None,
                start: int = 0,
                page_size: int = int_max,
                includes: Optional[Callable[[TimeSeriesIncludeBuilder], None]] = None,
            ): ...

        # endregion

        # region TimeSeriesRangeResult-class
        class TimeSeriesRangeResult:
            def __init__(
                self,
                from_date: datetime,
                to_date: datetime,
                entries: List[TimeSeriesEntry],
                total_results: int = None,
                includes: Optional[Dict[str, Any]] = None,
            ): ...

        # endregion

        # region GetMultipleTimeSeriesOperation
        class GetMultipleTimeSeriesOperation(IOperation[TimeSeriesDetails]):
            def __init__(
                self,
                doc_id: str,
                ranges: List[AbstractTimeSeriesRange],
                start: Optional[int] = 0,
                page_size: Optional[int] = int_max,
                includes: Optional[Callable[[TimeSeriesIncludeBuilder], None]] = None,
            ): ...

        # endregion

        # region TimeSeriesRange-class
        class TimeSeriesRange(AbstractTimeSeriesRange):
            def __init__(self, name: str, from_date: Optional[datetime], to_date: Optional[datetime]): ...

        # endregion

        # region TimeSeriesDetails-class
        class TimeSeriesDetails:
            def __init__(self, key: str, values: Dict[str, List[TimeSeriesRangeResult]]): ...

        # endregion

        # region PatchOperation-Definition
        class PatchOperation(IOperation[PatchResult]):
            def __init__(
                self,
                key: str,
                change_vector: str,
                patch: PatchRequest,
                patch_if_missing: Optional[PatchRequest] = None,
                skip_patch_if_change_vector_mismatch: Optional[bool] = None,
            ): ...

        # endregion

        # region PatchByQueryOperation-Definition
        class PatchByQueryOperation(IOperation[OperationIdResult]):
            def __init__(
                self, query_to_update: Union[IndexQuery, str], options: Optional[QueryOperationOptions] = None
            ): ...

        # endregion

        class TimeSeriesBulkInsert:
            # region Append-Operation-Definition-1
            # Each appended entry has a single value.
            def append_single(self, timestamp: datetime, value: float, tag: Optional[str] = None) -> None: ...

            # endregion
            # region Append-Operation-Definition-2
            # Each appended entry has multiple values.
            def append(self, timestamp: datetime, values: List[float], tag: str = None) -> None: ...

            # endregion

        class TimeSeriesOperations:
            # region Register-Definition-1
            def register_type(
                self,
                collection_class: Type[_T_Collection],
                ts_bindable_object_type: Type[_T_TS_Values_Bindable],
                name: Optional[str] = None,
            ): ...

            # endregion
            # region Register-Definition-2
            def register(self, collection: str, name: str, value_names: List[str]) -> None: ...

            # endregion

        # region Query-definition

        def document_query(
            self,
            index_name: str = None,
            collection_name: str = None,
            object_type: Type[_T] = None,
            is_map_reduce: bool = False,
        ) -> DocumentQuery[_T]: ...

        # endregion

    # Watch class for TS Document Query documentation
    # region TS_DocQuery_class
    class Monitor:
        def __init__(self, accuracy: float = None):
            self.accuracy = accuracy

    # endregion
