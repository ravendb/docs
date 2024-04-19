from typing import Optional

from ravendb import SessionOptions, TransactionMode

from examples_base import ExampleBase, Employee, Product


class HowToLazy(ExampleBase):
    def test_lazy(self):
        with self.embedded_server.get_document_store("Lazy") as store:
            with store.open_session() as session:
                # region lazy_Load
                lazy_employee = (
                    session
                    # Add a call to lazily
                    .advanced.lazily.
                    # Document will not be loaded from the database here, no server call in made
                    load("employees/1-A", Employee)
                )

                employee = lazy_employee.value  # 'load' operation is executed here
                # The employee entity is now loaded & tracked by the session
                # endregion

            with store.open_session() as session:
                # region lazy_LoadWithInclude
                lazy_product = (
                    session
                    # Add a call to lazily
                    .advanced.lazily
                    # Request to include the related Supplier document
                    # Documents will Not be loaded from the database here, no server call is made
                    .include("SupplierId").load("products/1-A")
                )

                # 'Load with include' operation will be executed here
                # Both documents will be retrieved from the database
                product = lazy_product.value
                # The product entity is now loaded & tracked by the session

                # Access the related document, no additional server call is made
                supplier = session.load(product.SupplierId)
                # The supplier entity is now also loaded & tracked by the session
                # endregion

            with store.open_session() as session:
                # region lazy_LoadStartingWith
                lazy_employees = (
                    session
                    # Add a call to lazily
                    .advanced.lazily
                    # Request to load entities whose ID starts with 'employees/'
                    # Documents will Not be loaded from the database here, no server call is made
                    .load_starting_with("employees/")
                )

                employees = lazy_employees.value  # 'load' operation is executed here
                # The employee entities is now also loaded & tracked by the session
                # endregion

            # region lazy_ConditionalLoad
            # Create document and get is change-vector:
            change_vector: Optional[str] = None
            with store.open_session() as session1:
                employee = Employee()
                session1.store(employee, "employees/1-A")
                session1.save_changes()

                # Get the tracked entity change-vector
                change_vector = session1.advanced.get_change_vector_for(employee)

            # Conditionally lazy-load the document
            with store.open_session() as session2:
                lazy_employee = (
                    session2
                    # Add a call to lazily
                    .advanced.lazily
                    # Document will Not be loaded from the database here, no server call is made
                    .conditional_load("employees/1-A", change_vector)
                )

                loaded_item = lazy_employee.value  # 'conditional_load' operation is executed here
                employee = loaded_item.entity

                #  If conditional_load has actually fetched the document from the server (logic described above)
                #  then the employee entity is now loaded & tracked by the session
            # endregion

            with store.open_session() as session:
                # region lazy_Query
                # Define a lazy query:
                lazy_employees = (
                    session.query(object_type=Employee).where_equals("FirstName", "John")
                    # Add a call to lazily, the query will not be executed here
                    .lazily()
                )

                employees = lazy_employees.value  # query is executed here
                # Note: Since query results are not projected,
                # then the resulting employee entities will be tracked by the session.
                # endregion

            with store.open_session() as session:
                # region lazy_Revisions
                lazy_revisions = (
                    session.
                    # Add a call to lazily
                    advanced.revisions.lazily
                    # Revisions will Not be fetched here, no sever call is made
                    .get_for("employees/1-A", Employee)
                )

                # Usage is the same for the other get revisions methods:
                # .get()
                # .get_metadata_for()

                revisions = lazy_revisions.value  # Getting revisions is executed here
                # endregion

            # region lazy_CompareExchange
            with store.open_session(
                session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
            ) as session:
                # Create compare-exchange value:
                session.advanced.cluster_transaction.create_compare_exchange_value("someKey", "someValue")
                session.save_changes()

                # Get the compare-exchange value lazily:
                lazy_cmp_xchg = (
                    session
                    # Adda a call to lazily
                    .advanced.cluster_transaction.lazily
                    # Compare-exchange values will Not be fetched here, no server call is made
                    .get_compare_exchange_value("someKey")
                )

                # Usage is the same for the other method:
                # .get_compare_exchange_values()

                cmp_xchg_value = lazy_cmp_xchg.value  # Getting compare-exchange value is executed here
            # endregion

            with store.open_session() as session:
                # region lazy_ExecuteAll_Implicit
                # Define multiple lazy requests
                lazy_user_1 = session.advanced.lazily.load("users/1-A")
                lazy_user_2 = session.advanced.lazily.load("users/2-A")

                lazy_employees = session.query(object_type=Employee).lazily()
                lazy_products = session.query(object_type=Product).search("Name", "Ch*").lazily()

                # Accessing the value of ANY of the lazy instances will trigger
                # the execution of ALL pending lazy requests held up by the session
                # This is done in a SINGLE server call
                user1 = lazy_user_1.value

                # ALL the other values are now also available
                # No additional server calls are made when accessing these values
                user2 = lazy_user_2.value
                employees = lazy_employees.value
                products = lazy_products.value
                # endregion

            with store.open_session() as session:
                # region lazy_ExecuteAll_Explicit
                # Define multiple lazy requests
                lazy_user_1 = session.advanced.lazily.load("users/1-A")
                lazy_user_2 = session.advanced.lazily.load("users/2-A")

                lazy_employees = session.query(object_type=Employee).lazily()
                lazy_products = session.query(object_type=Product).search("Name", "Ch*").lazily()

                # Explicitly call 'execute_all_pending_lazy_operations'
                # ALL pending lazy requests held up by the session will be executed in a SINGLE server call
                session.advanced.eagerly.execute_all_pending_lazy_operations()

                # ALL values are now available
                # No additional server calls are made when accessing the values
                user1 = lazy_user_1.value
                user2 = lazy_user_2.value
                employees = lazy_employees.value
                products = lazy_products.value
                # endregion

    # region lazy_productClass
    class Product:
        def __init__(self, Id: str = None, Name: str = None, SupplierId: str = None):
            self.Id = Id
            self.Name = Name
            self.SupplierId = SupplierId  # The related document ID

    # endregion
