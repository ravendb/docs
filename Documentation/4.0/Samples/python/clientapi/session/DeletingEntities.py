from pyravendb.store.document_store import DocumentStore


class OpeningSession(object):
    @staticmethod
    def open_session():
        store = DocumentStore()
        store.initialize()


        # region deleting_1
        def delete(self, key_or_entity):

        def delete_by_entity(self, entity):
        # endregion

        with store.open_session() as session:
            # region deleting_2
            employee = session.load("employees/1")
            session.delete(employee)
            session.save_changes()
            # endregion

        with store.open_session() as session:
            # region deleting_3
            session.delete("employees/1")
            # endregion


        with store.open_session() as session:
            # region deleting_4
            employee = session.load("employees/1")
            session.delete_by_entity(employee)
            # endregion
