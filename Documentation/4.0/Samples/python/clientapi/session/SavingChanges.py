from pyravendb.store.document_store import DocumentStore


class Employee(object):
    def __init__(self, first_name, last_name):
        self.first_name = first_name
        self.last_name = last_name


class SavingChanges(object):
    @staticmethod
    def saving_changes():
        store = documentstore()
        store.initialize()

        # region saving_changes_1
        def save_changes(self):
            # endregion
            pass

        with store.open_session() as session:
            # region saving_changes_2
            # storing new entity
            session.store(Employee("first_name", "second_name"))

            session.save_changes()
            # endregion
