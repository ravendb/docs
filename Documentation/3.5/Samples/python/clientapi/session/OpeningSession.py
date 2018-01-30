from pyravendb.store.document_store import documentstore


class OpeningSession(object):
    @staticmethod
    def open_session():
        store = documentstore()
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

        # Open session for a specific database with api_key
        with store.open_session(database="Your database", api_key="API_KEY") as session:
            session.load("doc/3")
            # code here

        # endregion
