from pyravendb.store.document_store import DocumentStore


class Employee(object):
    def __init__(self, first_name, last_name):
        self.first_name = first_name
        self.last_name = last_name


class StoringEntities(object):
    @staticmethod
    def open_session():
        store = DocumentStore()
        store.initialize()

        # region store_entities_1
        def store(self, entity, key=None, change_vector=None):
            # endregion
            pass

        with store.open_session() as session:
            entity = Employee("first_name", "second_name")
            change_vector = ""
            # region store_entities_2
            session.store(entity)
            # endregion

            # region store_entities_3
            session.store(entity, change_vector=change_vector)
            # endregion

            # region store_entities_4
            session.store(entity, key="doc/1")
            # endregion

            # region store_entities_5
            session.store(entity, key="doc/1", change_vector=change_vector)
            # endregion

        with store.open_session() as session:
            # region store_entities_6
            # generate Id automatically
            # when we have a new and empty database and conventions are not changed: 'employees/1-A'
            employee = Employee("John", "Doe")
            session.store(employee)
            session.save_changes()
            # endregion
