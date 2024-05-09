import time
from datetime import datetime, timedelta
from typing import Dict, Any

from ravendb import (
    PutCompareExchangeValueOperation,
    CmpXchg,
    DocumentStore,
    DeleteCompareExchangeValueOperation,
    SessionOptions,
    TransactionMode,
)

from examples_base import ExampleBase, Company, Contact


class User:
    def __init__(self, email: str = None, Id: str = None, age: int = None):
        self.email = email
        self.Id = Id
        self.age = age

    @classmethod
    def from_json(cls, json_dict: Dict[str, Any]):
        return cls(json_dict["Email"], json_dict["Id"], json_dict["Age"])

    def to_json(self) -> Dict[str, Any]:
        return {"Email": self.email, "Id": self.Id, "Age": self.age}


class CompareExchange(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_compare_exchange(self):
        with self.embedded_server.get_document_store("ServerCompareExchange") as store:
            # region email
            email = "user@example.com"

            user = User(email=email)

            with store.open_session() as sesion:
                sesion.store(user)
                # At this point, the user document has an Id assigned

                # Try to reserve a new user email
                # Note: This operation takes place outside the session transaction,
                # It is a cluster-wide reservation
                cmp_xchg_result = store.operations.send(PutCompareExchangeValueOperation(f"emails/{email}", user.Id, 0))

                if cmp_xchg_result.successful is False:
                    raise RuntimeError("Email is already in use")

                # At this point we managed to reserve/save the user mail -
                # The document can be saved in save_changes
                sesion.save_changes()

                # endregion

                # region query_cmpxchg
                query = sesion.query(object_type=User).where_equals("Id", CmpXchg.value("emails/ayende@ayende.com"))
                # endregion

        # region shared_resource
        class SharedResource:
            def __init__(self, reserved_until: datetime = None):
                self.reserved_until = reserved_until

        def print_work() -> None:
            # Try to get hold of the printer resource
            reservation_index = lock_resource(store, "Printer/First-Floor", timedelta(minutes=20))

            try:
                ...
                # Do some work for the duration that was set
                # Don't exceed the duration, otherwise resource is available for someone else
            finally:
                release_resource(store, "Printer/First-Floor", reservation_index)

        def lock_resource(document_store: DocumentStore, resource_name: str, duration: timedelta):
            while True:
                now = datetime.utcnow()

                resource = SharedResource(reserved_until=now + duration)
                save_result = document_store.operations.send(
                    PutCompareExchangeValueOperation(resource_name, resource, 0)
                )

                if save_result.successful:
                    # resource_name wasn't present - we managed to reserve
                    return save_result.index

                # At this point, Put operation failed - someone else owns the lock or lock time expired
                if save_result.value.reserved_until < now:
                    # Time expired - Update the existing key with new value
                    take_lock_with_timeout_result = document_store.operations.send(
                        PutCompareExchangeValueOperation(resource_name, resource, save_result.index)
                    )

                    if take_lock_with_timeout_result.successful:
                        return take_lock_with_timeout_result.index

                # Wait a little bit and retry
                time.sleep(0.02)

        def release_resource(store: DocumentStore, resource_name: str, index: int) -> None:
            delete_result = store.operations.send(DeleteCompareExchangeValueOperation(resource_name, index))

            # We have 2 options here:
            # delete_result.successful is True - we managed to release resource
            # delete_result.successful is False - someone else took the lock due to timeout

        # endregion

        # region create_uniqueness_control_documents
        # When you create documents that must contain a unique value such as a phone or email, etc.,
        # you can create reference documents that will have that unique value in their IDs.
        # To know if a value already exists, all you need to do is check whether a reference document with such ID exists.

        # The reference document class
        class UniquePhoneReference:
            class PhoneReference:
                def __init__(self, Id: str = None, company_id: str = None):
                    self.Id = Id
                    self.company_id = company_id

            def main(self):
                # A company document class that must be created with a unique 'Phone' field
                new_company = Company(
                    name="companyName", phone="phoneNumber", contact=Contact(name="contactName", title="contactTitle")
                )

                def create_company_with_unique_phone(new_company: Company) -> None:
                    # Open a cluster-wide session in your document store
                    with store.open_session(
                        session_options=SessionOptions(transaction_mode=TransactionMode.CLUSTER_WIDE)
                    ) as session:
                        # Check whether the new company phone already exists
                        # by checking if there is already a reference document that has the new phone in its ID.
                        phone_ref_document = session.load(f"phones/{new_company.phone}")
                        if phone_ref_document is not None:
                            msg = f"Phone '{new_company.phone}' already exists, store the new entity"
                            raise RuntimeError(msg)

                        # If the new phone number doesn't already exist, store the new entity
                        session.store(new_company)
                        # Store a new reference document with the new phone value in its ID for future checks
                        session.store(
                            UniquePhoneReference.PhoneReference(company_id=new_company.Id),
                            f"phones/{new_company.phone}",
                        )

                        # May fail if called concurrently with the same phone number
                        session.save_changes()

                # endregion
