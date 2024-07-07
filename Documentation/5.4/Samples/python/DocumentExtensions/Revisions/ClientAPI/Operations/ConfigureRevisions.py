from datetime import timedelta
from typing import Dict

from ravendb import RevisionsCollectionConfiguration, RevisionsConfiguration, GetDatabaseRecordOperation
from ravendb.documents.operations.definitions import MaintenanceOperation
from ravendb.documents.operations.revisions import ConfigureRevisionsOperation, ConfigureRevisionsOperationResult

from examples_base import ExampleBase


class ConfigRevisions(ExampleBase):
    def setUp(self):
        super().setUp()

    def test_configure_revisions(self):
        with self.embedded_server.get_document_store("ConfigurationRevisions") as store:
            with store.open_session() as session:
                # region replace_configuration
                # ==============================================================================
                # Define default settings that will apply to ALL collections
                # Note: this is optional
                default_rev_config = RevisionsCollectionConfiguration(
                    minimum_revisions_to_keep=100,
                    minimum_revisions_age_to_keep=timedelta(days=7),
                    maximum_revisions_to_delete_upon_document_creation=15,
                    purge_on_delete=False,
                    disabled=False,
                    # With this configuration:
                    # ------------------------
                    # * A revision will be created anytime a document is modified or deleted.
                    # * Revisions of a deleted document can be accessed in the Revisions Bin view.
                    # * At least 100 of the latest revisions will be kept.
                    # * Older revisions will be removed if they exceed 7 days on next revision creation.
                    # * A maximum of 15 revisions will be deleted each time a document is updated,
                    #   until the defined '# of revisions to keep' limit is reached.
                )

                employees_rev_config = RevisionsCollectionConfiguration(
                    minimum_revisions_to_keep=50,
                    minimum_revisions_age_to_keep=timedelta(hours=12),
                    purge_on_delete=True,
                    disabled=False,
                    # With this configuration:
                    # ------------------------
                    # * A revision will be created anytime an Employee document is modified.
                    # * When a document is deleted all its revisions will be removed.
                    # * At least 50 of the latest revisions will be kept.
                    # * Older revisions will be removed if they exceed 12 hours on next revision creation.
                )

                # ==============================================================================
                # Define a specific configuration for the EMPLOYEES collection
                # This will override the default settings
                products_rev_config = RevisionsCollectionConfiguration(
                    disabled=True
                    # No revisions will be created for the Products collection,
                    # even though default configuration is enabled
                )

                # ==============================================================================
                # Combine all configurations in the RevisionsConfiguration object
                revisions_config = RevisionsConfiguration(
                    default_config=default_rev_config,
                    collections={"Employees": employees_rev_config, "Products": products_rev_config},
                )

                # ==============================================================================
                # Define the configure revisions operation, pass the configuration
                configure_revisions_op = ConfigureRevisionsOperation(revisions_config)

                # Execute the operation by passing it to Maintenance.Send
                # Any existing configuration will be replaced with the new configuration passed
                store.maintenance.send(configure_revisions_op)
                # endregion

                # MODIFY
                default_rev_config = RevisionsCollectionConfiguration(
                    minimum_revisions_to_keep=100,
                    minimum_revisions_age_to_keep=timedelta(days=7),
                    maximum_revisions_to_delete_upon_document_creation=15,
                    purge_on_delete=False,
                    disabled=False,
                )

                employees_rev_config = RevisionsCollectionConfiguration(
                    minimum_revisions_to_keep=50,
                    minimum_revisions_age_to_keep=timedelta(hours=12),
                    purge_on_delete=True,
                )

                products_rev_config = RevisionsCollectionConfiguration(disabled=True)

                # region modify_configuration
                # ==============================================================================
                # Define the get database record operation:
                get_database_record_op = GetDatabaseRecordOperation(store.database)
                # Get the current revisions configuration from the database record:
                revisions_config = store.maintenance.server.send(get_database_record_op).revisions

                # ==============================================================================
                # If no revisions configuration exists, then create a new configuration
                if revisions_config is None:
                    revisions_config = RevisionsConfiguration(
                        default_config=default_rev_config,
                        collections={"Employees": employees_rev_config, "Products": products_rev_config},
                    )

                # ==============================================================================
                # If a revisions configuration already exists, then modify it
                else:
                    revisions_config.default_config = default_rev_config
                    revisions_config.collections["Employees"] = employees_rev_config
                    revisions_config.collections["Products"] = products_rev_config

                # ==============================================================================
                # Define the configure revisions operation, pass the configuration
                configure_revisions_op = ConfigureRevisionsOperation(revisions_config)

                # Execute the operation by passing it to maintenance.send
                # The existing configuration will be updated
                store.maintenance.send(configure_revisions_op)

                # Execute the operation by passing it to maintenance.send
                # The existing configuration will be updated
                store.maintenance.send(configure_revisions_op)
                # endregion


class Foo:
    # region syntax_1
    class ConfigureRevisionsOperation(MaintenanceOperation[ConfigureRevisionsOperationResult]):
        def __init__(self, configuration: RevisionsConfiguration): ...

    # endregion

    # region syntax_2

    class RevisionsConfiguration:
        def __init__(
            self,
            default_config: RevisionsCollectionConfiguration = None,
            collections: Dict[str, RevisionsCollectionConfiguration] = None,
        ): ...

    # endregion
    # region syntax_3

    class RevisionsCollectionConfiguration:
        def __init__(
            self,
            minimum_revisions_to_keep: int = None,
            minimum_revisions_age_to_keep: timedelta = None,
            disabled: bool = False,
            purge_on_delete: bool = False,
            maximum_revisions_to_delete_upon_document_creation: int = None,
        ): ...

    # endregion
