from pyravendb.store.document_store import documentstore


class Employee(object):
    def __init__(self, first_name, last_name):
        self.first_name = first_name
        self.last_name = last_name


class StoringEntities(object):
    @staticmethod
    def open_session():
        store = documentstore()
        store.initialize()

        # region store_entities_1
        def store(self, entity, key=None, etag=None, force_concurrency_check=False):
            # endregion
            pass

        with store.open_session() as session:
            entity = Employee("first_name", "second_name")
            etag = ""
            # region store_entities_2
            session.store(entity)
            # endregion

            # region store_entities_3
            session.store(entity, etag=etag)
            # endregion

            # region store_entities_4
            session.store(entity, key="doc/1")
            # endregion

            # region store_entities_5
            session.store(entity, key="doc/1", etag=etag)
            # endregion

        with store.open_session() as session:
            # region store_entities_6
            # generate Id automatically
            # # when we have a new and empty database and conventions are not changed: 'employees/1'
            employee = Employee("John", "Doe")
            session.store(employee)
            session.save_changes()
            # endregion
