
# What's new in 2.5?

##Features

* [Dynamic Aggregations](../client-api/querying/dynamic-aggregation)
* [SSL support for a windows service](../server/deployment/enabling-ssl)
* [Result Transformers](../client-api/querying/results-transformation/result-transformers)
* [Scripted Index Results](../server/extending/bundles/scripted-index-results)
* [Unbounded Results API](../client-api/advanced/unbounded-results)
* [New Functions for Scripted Patching API](../client-api/partial-document-updates#performing-complex-updates) (creating new doc, debugging)
* [More notifications in Changes API](../client-api/changes-api) (replication conflicts, bulk inserts, index priorities)
* [Spatial Enhancements](../client-api/querying/static-indexes/spatial-search)
* [NoTracking and NoCaching query customizations](../client-api/querying/query-customizations)
* [Excel integration](../http-api/excel-integration) - CSV endpoint
* [MSI Installer](../server/deployment/installer)
* [Write Assurance](../server/scaling-out/replication/write-assurance)
* [Usage Northwind for sample data](https://ayende.com/blog/162338/ravendb-sample-data-hello-northwind)

##Indexing

* [Index locking](../server/administration/index-administration#index-locking)
* [Index priorities](../server/administration/index-administration#index-prioritization)
* [Index directly to memory](https://ayende.com/blog/161282/robs-sprint-faster-index-creation)
* [Query optimizer can expand indexes](https://ayende.com/blog/161283/robs-sprint-query-optimizer-jumped-a-grade)

##Improvements

* [Better Aggressive Cache with notifications usage](../client-api/advanced/aggressive-caching)
* [Better pre fetching algorithms for reducing IO](https://ayende.com/blog/160291/ravendb-indexing-optimizations-step-iii-skipping-the-disk-altogether)
* [More robust and better pefromance of SQL Replication](../server/extending/bundler/sql-replication)
* DTC performance improvement
* [Better adherence to the DTC protocol](../client-api/advanced/transaction-support)
* Will refresh the index writer every now and then, avoiding high memory costs for active indexes
* Faster error reporting by bulk inserts
* Removed db write lock and enabled completely concurrent writes

##Studio

* [Dynamic Reporting](../studio/dynamic-reporting)
* Consolidated Documents & Collections screen
* Easier settings management
* Streamlined the UI
* Validate replication information from the studio
* Can now look at a sample document while editing an index
