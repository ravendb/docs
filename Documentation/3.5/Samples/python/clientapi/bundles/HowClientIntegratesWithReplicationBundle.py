from pyravendb.data.document_convention import Failover
from pyravendb.data import database
from pyravendb.store.document_store import documentstore


class HowClientIntegratesWithReplicationBundle(object):
    def sample(self):
        try:
            document_store = documentstore()
            # region client_integration_1
            document_store.conventions.failover_behavior = Failover.fail_immediately
            # endregion

            # region client_integration_2
            with document_store.open_session("PyRavenDB") as session:
                replication_document = database.ReplicationDocument([database.ReplicationDestination(
                    url="http://localhost:8080", database="destination_database_name"),
                    database.ReplicationDestination(
                        url="http://localhost:8081", database="destination_database_name")])
                session.store(replication_document)
                session.save_changes()
                # endregion
        except Exception:
            raise
