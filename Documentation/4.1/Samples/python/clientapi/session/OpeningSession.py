from pyravendb.store.document_store import DocumentStore


class OpeningSession(object):
    @staticmethod
    def open_session():
        store = DocumentStore()
        store.initialize()

        # region open_session_1
        # Open session for a 'default' database configured in 'documentstore'
        with store.open_session() as session:
            session.load("doc/1")
            # code here

        # Open session for a specific database
        with store.open_session(database="Your database") as session:
            session.load("doc/2")
            # code here

        # Open session with request_executor
        request_executor = RequestsExecutor(database_name="Your database", certificate=None)
        with DocumentStore.open_session(request_executor=request_executor) as session:
            session.load("doc/3")
            # code here

        # endregion
