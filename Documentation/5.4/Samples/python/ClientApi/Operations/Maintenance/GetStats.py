import datetime
from typing import List, Optional, Dict

from ravendb import GetCollectionStatisticsOperation
from ravendb import GetDetailedStatisticsOperation
from ravendb.tools.utils import Size

from examples_base import ExampleBase
from ravendb.documents.operations.statistics import (
    GetDetailedCollectionStatisticsOperation,
    GetStatisticsOperation,
    IndexInformation,
)


class GetStats(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_get_stats(self):
        with self.embedded_server.get_document_store("GetStats") as store:
            with store.open_session() as session:
                # region stats_1
                # Pass an instance of class 'GetCollectionStatisticsOperation' to the store
                stats = store.maintenance.send(GetCollectionStatisticsOperation())
                # endregion

            with store.open_session() as session:
                # region stats_2
                # Pass an instance of class 'GetDetailedCollectionStatisticsOperation' to the store
                stats = store.maintenance.send(GetDetailedCollectionStatisticsOperation())
                # endregion

            with store.open_session() as session:
                # region stats_3
                # Pass an instance of class 'GetStatisticsOperation' to the store
                stats = store.maintenance.send(GetStatisticsOperation())
                # endregion

            with store.open_session() as session:
                # region stats_4
                # Pass an instance of class 'GetDetailedStatisticsOperation' to the store
                stats = store.maintenance.send(GetDetailedStatisticsOperation())
                # endregion

            with store.open_session() as session:
                # region stats_5
                # Get stats for 'AnotherDatabase'
                stats = store.maintenance.for_database("AnotherDatabase").send(GetStatisticsOperation())
                # endregion


class Foo:
    # region stats_1_results
    class CollectionStatistics:
        def __init__(
            self,
            count_of_documents: Optional[int] = None,
            count_of_conflicts: Optional[int] = None,
            collections: Optional[Dict[str, int]] = None,
        ): ...

    # endregion
    # region stats_2_results
    class Size:
        def __init__(self, size_in_bytes: int = None, human_size: str = None): ...

    class CollectionDetails:
        def __init__(
            self,
            name: str = None,
            count_of_documents: int = None,
            size: Size = None,
            documents_size: Size = None,
            tombstones_size: Size = None,
            revisions_size: Size = None,
        ): ...

    class DetailedCollectionStatistics:
        def __init__(
            self,
            count_of_documents: int = None,
            count_of_conflicts: int = None,
            collections: Dict[str, CollectionDetails] = None,
        ) -> None: ...

    # endregion

    # region stats_3_results
    class DatabaseStatistics:
        def __init__(
            self,
            last_doc_etag: int = None,
            last_database_etag: int = None,
            count_of_indexes: int = None,
            count_of_documents: int = None,
            count_of_revision_documents: int = None,
            count_of_documents_conflicts: int = None,
            count_of_tombstones: int = None,
            count_of_conflicts: int = None,
            count_of_attachments: int = None,
            count_of_unique_attachments: int = None,
            count_of_counter_entries: int = None,
            count_of_time_series_segments: int = None,
            indexes: List[IndexInformation] = None,
            database_change_vector: str = None,
            database_id: str = None,
            is_64_bit: bool = None,
            pager: str = None,
            last_indexing_time: datetime.datetime = None,
            size_on_disk: Size = None,
            temp_buffers_size_on_disk: Size = None,
            number_of_transaction_merger_queue_operations: int = None,
        ): ...

    # endregion

    # region stats_4_results
    class DetailedDatabaseStatistics(DatabaseStatistics):
        def __init__(
            self,
            last_doc_etag: int = None,
            last_database_etag: int = None,
            count_of_indexes: int = None,
            count_of_documents: int = None,
            count_of_revision_documents: int = None,
            count_of_documents_conflicts: int = None,
            count_of_tombstones: int = None,
            count_of_conflicts: int = None,
            count_of_attachments: int = None,
            count_of_unique_attachments: int = None,
            count_of_counter_entries: int = None,
            count_of_time_series_segments: int = None,
            indexes: List[IndexInformation] = None,
            database_change_vector: str = None,
            database_id: str = None,
            is_64_bit: bool = None,
            pager: str = None,
            last_indexing_time: datetime.datetime = None,
            size_on_disk: Size = None,
            temp_buffers_size_on_disk: Size = None,
            number_of_transaction_merger_queue_operations: int = None,
            count_of_identities: int = None,  # Total # of identities in database
            count_of_compare_exchange: int = None,  # Total # of compare-exchange items in database
            count_of_compare_exchange_tombstones: int = None,  # Total # of cmpXchg tombstones in database
        ): ...

    # endregion
