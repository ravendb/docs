import logging
from concurrent.futures import Future
from datetime import timedelta, datetime
from typing import Optional, Type, TypeVar, Callable, Any, Dict, List

from ravendb import DocumentStore
from ravendb.documents.session.loaders.include import SubscriptionIncludeBuilder
from ravendb.documents.subscriptions.options import (
    SubscriptionCreationOptions,
    SubscriptionUpdateOptions,
    SubscriptionWorkerOptions,
    SubscriptionOpeningStrategy,
)
from ravendb.documents.subscriptions.state import SubscriptionState
from ravendb.documents.subscriptions.worker import SubscriptionWorker, SubscriptionBatch
from ravendb.exceptions.exceptions import (
    DatabaseDoesNotExistException,
    SubscriptionDoesNotExistException,
    SubscriptionInvalidStateException,
    AuthorizationException,
    SubscriptionClosedException,
    SubscriberErrorException,
    SubscriptionInUseException,
)
from ravendb.serverwide.operations.common import DeleteDatabaseOperation

from examples_base import Order, ExampleBase, Company

_T = TypeVar("_T")


class OrderRevenues:
    def __init__(self, previous_revenue: int = None, current_revenue: int = None):
        self.previous_revenue = previous_revenue
        self.current_revenue = current_revenue


class Foo:
    # region subscriptionCreationOverloads
    def create_for_options(self, options: SubscriptionCreationOptions, database: Optional[str] = None) -> str: ...

    def create_for_class(
        self,
        object_type: Type[_T],
        options: Optional[SubscriptionCreationOptions] = None,
        database: Optional[str] = None,
    ) -> str: ...

    # endregion
    # region updating_subscription
    def update(self, options: SubscriptionUpdateOptions, database: Optional[str] = None) -> str: ...

    # endregion

    # region sub_create_options
    class SubscriptionCreationOptions:
        def __init__(
            self,
            name: Optional[str] = None,
            query: Optional[str] = None,
            includes: Optional[Callable[[SubscriptionIncludeBuilder], None]] = None,
            change_vector: Optional[str] = None,
            mentor_node: Optional[str] = None,
        ):
            self.name = name
            self.query = query
            self.includes = includes
            self.change_vector = change_vector
            self.mentor_node = mentor_node

    # endregion

    # region sub_update_options

    class SubscriptionUpdateOptions(SubscriptionCreationOptions):
        def __init__(
            self,
            name: Optional[str] = None,
            query: Optional[str] = None,
            includes: Optional[Callable[[SubscriptionIncludeBuilder], None]] = None,
            change_vector: Optional[str] = None,
            mentor_node: Optional[str] = None,
            key: Optional[int] = None,
            create_new: Optional[bool] = None,
        ): ...

    # endregion
    # region subscriptionWorkerGeneration
    def get_subscription_worker(
        self, options: SubscriptionWorkerOptions, object_type: Optional[Type[_T]] = None, database: Optional[str] = None
    ) -> SubscriptionWorker[_T]: ...

    def get_subscription_worker_by_name(
        self,
        subscription_name: Optional[str] = None,
        object_type: Optional[Type[_T]] = None,
        database: Optional[str] = None,
    ) -> SubscriptionWorker[_T]: ...

    # endregion

    # region subscriptionWorkerRunning
    def run(self, process_documents: Optional[Callable[[SubscriptionBatch[_T]], Any]]) -> Future: ...

    # endregion

    # region subscriptions_example
    def worker(self, store: DocumentStore) -> Future:
        subscription_name = store.subscriptions.create_for_class(
            Order, SubscriptionCreationOptions(query="from Orders where Company = " "")
        )
        subscription = store.subscriptions.get_subscription_worker_by_name(subscription_name)

        def __subscription_callback(subscription_batch: SubscriptionBatch[Order]):
            for item in subscription_batch.items:
                print(f"Order #{item.result.key}")

        subscription_task = subscription.run(__subscription_callback)

        return subscription_task

    # endregion

    # region creation_api
    # endregion


class SubscriptionExamples(ExampleBase):
    def setUp(self):
        super().setUp()

    def tearDown(self):
        with self.embedded_server.get_document_store("Manager") as store:
            parameters = DeleteDatabaseOperation.Parameters(["SubscriptionsExamples"], True)
            store.maintenance.server.send(DeleteDatabaseOperation(parameters=parameters))

    def test_subscriptions(self):
        with self.embedded_server.get_document_store("SubscriptionsExamples") as store:
            # region create_whole_collection_generic_with_name
            name = store.subscriptions.create_for_class(
                Order, SubscriptionCreationOptions(name="OrdersProcessingSubscription")
            )
            # endregion

            # region create_whole_collection_generic_with_mentor_node
            name = store.subscriptions.create_for_class(Order, SubscriptionCreationOptions(mentor_node="D"))
            # endregion

            # region create_whole_collection_generic1
            # With the following subscription definition, the server will send ALL documents
            # from the 'Orders' collection to a client that connects to this subscription.
            name = store.subscriptions.create_for_class(Order)
            # endregion

            # region create_whole_collection_RQL
            name = store.subscriptions.create_for_options(SubscriptionCreationOptions(query="From Orders"))
            # endregion
            all_name = name

            # region create_filter_only_RQL
            name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query=(
                        "declare function getOrderLinesSum(doc) {"
                        "    var sum = 0;"
                        "    for (var i in doc.Lines) {"
                        "        sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;"
                        "    }"
                        "    return sum;"
                        "}"
                        "From Orders as o "
                        "Where getOrderLinesSum(o) > 100 "
                    )
                ),
            )
            # endregion

            # region create_filter_and_projection_RQL
            name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query="""
                    declare function getOrderLinesSum(doc) {
                        var sum = 0;
                        for (var i in doc.Lines) {
                            sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                        }
                        return sum;
                    }
                    
                    declare function projectOrder(doc) {
                        return {
                            Id: doc.Id,
                            Total: getOrderLinesSum(doc)
                        };
                    }
                    
                    From Orders as o
                    Where getOrderLinesSum(o) > 100
                    Select projectOrder(o)
                    """
                )
            )
            # endregion

            # region create_filter_and_load_document_RQL
            name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query="""
                    declare function getOrderLinesSum(doc) {
                        var sum = 0;
                        for (var i in doc.Lines) {
                            sum += doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity;
                        }
                        return sum;
                    }
                   
                    declare function projectOrder(doc) {
                        var employee = load(doc.Employee);
                        return {
                            Id: doc.Id,
                            Total: getOrderLinesSum(doc),
                            ShipTo: doc.ShipTo,
                            EmployeeName: employee.FirstName + ' ' + employee.LastName
                        };
                    }
                   
                    From Orders as o
                    Where getOrderLinesSum(o) > 100
                    Select projectOrder(o)
                    """
                )
            )
            # endregion

            # region create_filter_and_load_document_RQL
            name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query="""
                    declare function getOrderLinesSum(doc){
                        var sum = 0;
                        for (var i in doc.Lines) { sum += doc.Lines[i];}
                        return sum;
                    }

                    declare function projectOrder(doc){
                        var employee = load(doc.Employee);
                        return {
                            Id: order.Id,
                            Total: getOrderLinesSum(order),
                            ShipTo: order.ShipTo,
                            EmployeeName: employee.FirstName + ' ' + employee.LastName

                        };
                    }

                    From Orders as o 
                    Where getOrderLinesSum(o) > 100
                    Select projectOrder(o)
                    """
                )
            )
            # endregion

            # region create_projected_revisions_subscription_RQL
            name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query="""
                    declare function getOrderLinesSum(doc){
                        var sum = 0;
                        for (var i in doc.Lines) { sum += doc.Lines[i];}
                        return sum;
                    }

                    From Orders (Revisions = true) as o
                    Where getOrderLinesSum(o.Current)  > getOrderLinesSum(o.Previous)
                    Select 
                    {
                        previous_revenue: getOrderLinesSum(o.Previous),
                        current_revenue: getOrderLinesSum(o.Current)                            
                    }
                    """
                )
            )
            # endregion

            # region consume_revisions_subscription_generic
            revenues_comparison_worker = store.subscriptions.get_subscription_worker_by_name(name, OrderRevenues)

            def _revenues_callback(batch: SubscriptionBatch[OrderRevenues]):
                for item in batch.items:
                    print(
                        f"Revenue for order with Id: {item.key} grown from {item.result.previous_revenue} to {item.result.current_revenue}"
                    )

            revenues_comparison_worker.run(_revenues_callback)
            # endregion
            """
            # region consumption_0
            subscription_name = store.subscriptions.create_for_class(
                Order, SubscriptionCreationOptions(query='From Orders Where Company = "companies/11"')
            )

            subscription = store.subscriptions.get_subscription_worker_by_name(subscription_name, Order)

            def _orders_callback(batch: SubscriptionBatch[Order]):
                for item in batch.items:
                    print(f"Order {item.result.key} will be shipped via: {item.result.ship_via}")

            subscription_task = subscription.run(_orders_callback)

            subscription_task.result()  # Optionally wrap it in an asyncio.Future
            # endregion
            """
            # region open_1
            subscription = store.subscriptions.get_subscription_worker_by_name(name, Order)
            # endregion

            # region open_2
            subscription = store.subscriptions.get_subscription_worker(
                SubscriptionWorkerOptions(name, strategy=SubscriptionOpeningStrategy.WAIT_FOR_FREE)
            )
            # endregion

            # region open_3
            subscription = store.subscriptions.get_subscription_worker(
                SubscriptionWorkerOptions(
                    name,
                    strategy=SubscriptionOpeningStrategy.WAIT_FOR_FREE,
                    max_docs_per_batch=500,
                    ignore_subscriber_errors=True,
                )
            )
            # endregion

            # region create_subscription_with_includes_strongly_typed
            store.subscriptions.create_for_class(
                Order,
                SubscriptionCreationOptions(includes=lambda builder: builder.include_documents("Lines[].Product")),
            )
            # endregion

            # region create_subscription_with_includes_rql_path
            store.subscriptions.create_for_options(
                SubscriptionCreationOptions(query="from Orders include Lines[].Product")
            )
            # endregion

            # region create_subscription_with_includes_rql_javascript
            store.subscriptions.create_for_options(
                SubscriptionCreationOptions(
                    query="""
                    declare function includeProducts(doc) {
                        let includedFields = 0;
                        let linesCount = doc.Lines.length;

                        for (let i = 0; i < linesCount; i++) {
                            includedFields++;
                            include(doc.Lines[i].Product);
                        }

                        return doc;
                    }

                    from Orders as o select includeProducts(o)
                    """
                )
            )
            # endregion

            # region include_builder_counter_methods
            def include_counter(self, name: str) -> SubscriptionIncludeBuilder: ...

            def include_counters(self, *names: str) -> SubscriptionIncludeBuilder: ...

            def include_all_counters(self) -> SubscriptionIncludeBuilder: ...

            # endregion

            """
            # region create_subscription_include_counters_builder
            store.subscriptions.create_for_class(
                Order,
                SubscriptionCreationOptions(
                    includes=lambda builder: builder
                    .include_counter("Likes")
                    .include_counters("Pros", "Cons")
                ),
            )
            # endregion
            
            
            # region update_subscription_example_0
            store.subscriptions.update(SubscriptionUpdateOptions(
                name="My subscription", query="from Products where PricePerUnit > 50"))
            # endregion
            
            
            # region update_subscription_example_1
            my_subscription = store.subscriptions.get_subscription_state("my subscription")

            subscription_id = my_subscription.subscription_id

            store.subscriptions.update(SubscriptionUpdateOptions(key=subscription_id, name="new name"))

            # endregion
            """
            # region interface_subscription_deletion
            def delete(self, name: str, database: Optional[str] = None) -> None: ...

            # endregion

            # region interface_connection_dropping
            def drop_connection(self, name: str, database: Optional[str] = None) -> None: ...

            # endregion

            # region interface_subscription_enabling
            def enable(self, name: str, database: Optional[str] = None) -> None: ...

            # endregion

            # region interface_subscription_disabling
            def disable(self, name: str, database: Optional[str] = None) -> None: ...

            # endregion

            # region interface_subscription_state
            def get_subscription_state(
                self, subscription_name: str, database: Optional[str] = None
            ) -> SubscriptionState: ...

            # endregion

            subscription_name = ""
            """
            # region subscription_enabling
            store.subscriptions.enable(subscription_name)
            # endregion

            # region subscription_disabling
            store.subscriptions.disable(subscription_name)
            # endregion
            # region subscription_deletion
            store.subscriptions.delete(subscription_name)
            # endregion

            # region connection_dropping
            store.subscriptions.drop_connection(subscription_name)
            # endregion

            # region subscription_state
            store.subscriptions.get_subscription_state(subscription_name)
            # endregion
            

            # region subscription_open_simple
            subscription_worker = store.subscriptions.get_subscription_worker_by_name(subscription_name, Order)
            # endregion

            # region subscription_run_simple
            subscription_runtime_task = subscription_worker.run(
                process_documents=lambda batch: ...
            )  # Pass your method that takes SubscriptionBatch[_T] as an argument, with your logic in it
            # endregion

            # region subscription_worker_with_batch_size
            worker_w_batch = store.subscriptions.get_subscription_worker(
                SubscriptionWorkerOptions(subscription_name, max_docs_per_batch=20), Order
            )

            _ = worker_w_batch.run(
                process_documents=lambda batch: ...
            )  # Pass your method that takes SubscriptionBatch[_T] as an argument, with your logic in it

            # endregion

            # region throw_during_user_logic
            def _throw_exception(batch: SubscriptionBatch):
                raise Exception()

            _ = worker_w_batch.run(_throw_exception)
            # endregion
            
            logger = logging.Logger("my_logger")

            class UnsupportedCompanyException(Exception): ...

            process_order = lambda x: ...
            subscription_name = all_name
            # region reconnecting_client
            while True:
                options = SubscriptionWorkerOptions(subscription_name)

                # here we configure that we allow a down time of up to 2 hours, and will wait for 2 minutes for reconnecting
                options.max_erroneous_period = timedelta(hours=2)
                options.time_to_wait_before_connection_retry = timedelta(minutes=2)

                subscription_worker = store.subscriptions.get_subscription_worker(options, Order)

                try:
                    # here we are able to be informed of any exceptions that happens during processing
                    subscription_worker.add_on_subscription_connection_retry(
                        lambda exception: logger.error(
                            f"Error during subscription processing: {subscription_name}", exc_info=exception
                        )
                    )

                    def _process_documents_callback(batch: SubscriptionBatch[Order]):
                        for item in batch.items:
                            # we want to force close the subscription processing in that case
                            # and let the external code decide what to do with that
                            if item.result.company == "companies/2-A":
                                raise UnsupportedCompanyException(
                                    "Company Id can't be 'companies/2-A', you must fix this"
                                )
                            process_order(item.result)

                        # Run will complete normally if you have disposed the subscription
                        return

                    # Pass the callback to worker.run()
                    subscription_worker.run(_process_documents_callback)

                except Exception as e:
                    logger.error(f"Failure in subscription: {subscription_name}", exc_info=e)
                    exception_type = type(e)
                    if (
                        exception_type is DatabaseDoesNotExistException
                        or exception_type is SubscriptionDoesNotExistException
                        or exception_type is SubscriptionInvalidStateException
                        or exception_type is AuthorizationException
                    ):
                        raise  # not recoverable

                    if exception_type is SubscriptionClosedException:
                        # closed explicitely by admin, probably
                        return

                    if exception_type is SubscriberErrorException:
                        # for UnsupportedCompanyException type, we want to throw an exception, otherwise
                        # we continue processing
                        if e.args[1] is not None and type(e.args[1]) is UnsupportedCompanyException:
                            raise

                        continue

                    # handle this depending on subscription
                    # open strategy (discussed later)
                    if e is SubscriptionInUseException:
                        continue

                    return
                finally:
                    subscription_worker.close(False)
            # endregion
            
            # region worker_timeout
            options = SubscriptionWorkerOptions(subscription_name)

            subscription_worker = store.subscriptions.get_subscription_worker(options, Order)

            try:
                subscription_worker.add_on_subscription_connection_retry(
                    lambda exception: logger.error(
                        f"Error during subscription processing: {subscription_name}", exc_info=exception
                    )
                )

                subscription_worker.run(lambda batch: [... for item in batch.items])

                # Run will complete normally if you have disposed the subscription
                return
            except Exception as e:
                logger.error(f"Error during subscription process: {subscription_name}", exc_info=e)
            finally:
                subscription_worker.__exit__()
            # endregion
            """
            subs_id = store.subscriptions.create_for_class(
                Order,
                SubscriptionCreationOptions(
                    query="""
                    declare function getOrderLinesTotal(doc){
                        var total = 0;
                        for (var i in doc.Lines) { 
                            total += (doc.Lines[i].PricePerUnit * doc.Lines[i].Quantity);
                        }
                        return total;
                    }

                    From Orders as o
                    Where getOrderLinesTotal(o) > 10000
                    Select 
                    {
                        order_id: id(o),
                        company: this.LoadDocument(o.Company, "Companies")                            
                    }
                    """
                ),
            )

            class OrderAndCompany:
                def __init__(self, order_id: str = None, company: Company = None):
                    self.order_id = order_id
                    self.company = company

                @classmethod
                def from_json(cls, json_dict: Dict[str, Any]):
                    return cls(json_dict["order_id"], Company.from_json(json_dict["company"]))

            send_thank_you_note_to_employee = lambda x: ...
            """
            # region single_run
            high_value_orders_worker = store.subscriptions.get_subscription_worker(
                SubscriptionWorkerOptions(
                    subs_id,
                    # Here we ask the worker to stop when there are no documents left to send.
                    # Will throw SubscriptionClosedException when it finishes its job
                    close_when_no_docs_left=True,
                ),
                OrderAndCompany,
            )

            try:

                def _subscription_batch_callback(batch: SubscriptionBatch[OrderAndCompany]):
                    for item in batch.items:
                        send_thank_you_note_to_employee(item.result)

                high_value_orders_worker.run(_subscription_batch_callback)
            except SubscriptionClosedException:
                # that's expected
                ...
            # endregion
            """
            raise_notification = lambda x: ...
            # region dynamic_worker
            subscription_name = "My dynamic subscription"
            store.subscriptions.create_for_class(
                Order,
                SubscriptionCreationOptions(
                    subscription_name,
                    query="""
                    From Orders as o
                    Select 
                    {
                        dynamic_field_1: "Company: " + o.Company + " Employee: " + o.Employee,
                    }
                    """,
                ),
            )

            subscription_worker = store.subscriptions.get_subscription_worker_by_name(subscription_name)

            def _raise_notification_callback(batch: SubscriptionBatch[Order]):
                for item in batch.items:
                    raise_notification(item.result.dynamic_field_1)

            _ = subscription_worker.run(_raise_notification_callback)

            # endregion
            transfer_order_to_shipment_company = lambda x: ...
            # region subscription_with_open_session_usage
            subscription_name = store.subscriptions.create_for_options(
                SubscriptionCreationOptions(query="from Orders as o where o.ShippedAt = null")
            )

            subscription_worker = store.subscriptions.get_subscription_worker_by_name(subscription_name, Order)

            def _transfer_order_callback(batch: SubscriptionBatch[Order]):
                with batch.open_session() as session:
                    for order in (item.result for item in batch.items):
                        transfer_order_to_shipment_company(order)
                        order.shipped_at = datetime.utcnow()

                    # we know that we have at least one order to ship,
                    # because the subscription query above has that in it's WHERE clause
                    session.save_changes()

            _ = subscription_worker.run(_transfer_order_callback)
            # endregion

            # region waitforfree
            worker = store.subscriptions.get_subscription_worker(
                SubscriptionWorkerOptions(subscription_name, strategy=SubscriptionOpeningStrategy.WAIT_FOR_FREE), Order
            )
            # endregion
            """
            # region waiting_subscription_1
            primary_worker = store.subscriptions.get_subscription_worker(SubscriptionWorkerOptions(subscription_name, strategy=SubscriptionOpeningStrategy.TAKE_OVER), Order)
            
            while True:
                try:
                    run_future = primary_worker.run(lambda batch: ...) # your logic
                except Exception:
                    ... # retry
            # endregion
            
            # region waiting_subscription_2
            secondary_worker = store.subscriptions.get_subscription_worker(SubscriptionWorkerOptions(subscription_name), strategy=SubscriptionOpeningStrategy.WAIT_FOR_FREE)
            
            while True:
                try:
                    run_future = secondary_worker.run(lambda batch: ...) # your logic
                except Exception:
                    ... # retry
            # endregion
            """

            # region Item_definition
            class Item(Generic[_T_Item]):
                """
                Represents a single item in a subscription batch results.
                This class should be used only inside the subscription's run delegate,
                using it outside this scope might cause unexpected behavior.
                """
            # endregion

            # region number_of_items_in_batch_definition
            def number_of_items_in_batch(self) -> int:
                return 0 if self.items is None else len(self.items)
            # endregion

            # region SubscriptionBatch_definition
            class SubscriptionBatch(Generic[_T]):

            def __init__(self):
                self._result: Optional[_T_Item] = None
                self._exception_message: Optional[str] = None
                self._key: Optional[str] = None
                self._change_vector: Optional[str] = None
                self._projection: Optional[bool] = None
                self._revision: Optional[bool] = None
                self.raw_result: Optional[Dict] = None
                self.raw_metadata: Optional[Dict] = None
                self._metadata: Optional[MetadataAsDictionary] = None
            # endregion

            # region get_subscriptions_1
            def get_subscriptions(
                self, start: int, take: int, database: Optional[str] = None
            ) -> List[SubscriptionState]: ...

            # endregion
            # region delete_1
            def delete(self, name: str, database: Optional[str] = None) -> None: ...

            # endregion

            # region events
            self._after_acknowledgment: List[Callable[[SubscriptionBatch[_T]], None]] = []
            # endregion
