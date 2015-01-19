﻿#Miscellaneous conventions

###DisableProfiling

Disable all [profiling support](../../how-to/enable-profiling) which has been enabled by the `DocumentStore.InitializeProfiling()` call.

{CODE disable_profiling@ClientApi\Configuration\Conventions\Misc.cs /}

###EnlistInDistributedTransactions

It determines whether RavenDB client should automatically enlist in distributed transactions or not. Default: `true`.

{CODE enlist_in_dist_tx@ClientApi\Configuration\Conventions\Misc.cs /}

###MaxNumberOfRequestsPerSession

The max number of requests per session. See the [related article](../../session/configuration/how-to-change-maximum-number-of-requests-per-session) for details.

{CODE max_number_of_requests_per_session@ClientApi\Configuration\Conventions\Misc.cs /}

###SaveEnumsAsIntegers

It determines if C# `enum` types should be saved as integers or strings and instruct the Linq provider to query enums as integer values. Default: `false`.

{CODE save_enums_as_integers@ClientApi\Configuration\Conventions\Misc.cs /}

###DefaultUseOptimisticConcurrency

This convention allows to enable optimistic concurrency for all opened sessions. More about optimistic concurrency you will find [here](../../session/configuration/how-to-enable-optimistic-concurrency).
By default concurrency checks are turned off:

{CODE use_optimistic_concurrency_by_default@ClientApi\Configuration\Conventions\Misc.cs /}

###PrettifyGeneratedLinqExpressions

It determines if it should attempt to prettify the generated Linq expressions in definitions of indexes and transformers (used by the following classes: `AbstractIndexCreationTask`, `AbstractMultiMapIndexCreationTask`, `AbstractTransformerCreationTask` and `IndexDefinitionBuilder`).

{CODE prettify_generated_linq_expressions@ClientApi\Configuration\Conventions\Misc.cs /}

###IndexAndTransformerReplicationMode

This conventions determines if index and transformer definitions should be replicated to destination servers when indexes and transformers are deployed using the `AbstractIndexCreationTask` and `AbstractTransformerCreationTask`. 

Possible values are:

- `None`,
- `Indexes`,
- `Transformers`

with default set to `Indexes | Transformers`.

{CODE index_and_transformer_replication_mode@ClientApi\Configuration\Conventions\Misc.cs /}
