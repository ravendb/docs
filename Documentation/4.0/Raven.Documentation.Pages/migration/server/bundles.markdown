# Bundles

RavenDB 4.0 doesn't have the notion of bundles. All features are built-in the RavenDB server. 

## Replication

When running in a cluster the replication is turned on between databases belonging to the same database group. In addition to that you can define External Replication task.

## Periodic export

Please define backup task and choose what kind of backup you want to create periodically.

## Scripted Index Results

Creating documents based on map-reduce indexing results is possible using `OutputReduceToCollection` option defined in the index definition. This way recursive map-reduce indexes are possible.

## SQL Replication

It's defined as SQL ETL task

