#Miscellaneous conventions

###DisableProfiling

Disable all [profiling support](../../how-to/enable-profiling) which has been enabled by the `DocumentStore.initializeProfiling()` call.

{CODE:java disable_profiling@ClientApi\Configuration\Conventions\Misc.java /}

###MaxNumberOfRequestsPerSession

The max number of requests per session. See the [related article](../../session/configuration/how-to-change-maximum-number-of-requests-per-session) for details.

{CODE:java max_number_of_requests_per_session@ClientApi\Configuration\Conventions\Misc.java /}

###SaveEnumsAsIntegers

It determines if java `enum` types should be saved as integers or strings and instruct the query provider to query enums as integer values. Default: `false`.

{CODE:java save_enums_as_integers@ClientApi\Configuration\Conventions\Misc.java /}

###DefaultUseOptimisticConcurrency

This convention allows to enable optimistic concurrency for all opened sessions. More about optimistic concurrency you will find [here](../../session/configuration/how-to-enable-optimistic-concurrency).
By default concurrency checks are turned off:

{CODE:java use_optimistic_concurrency_by_default@ClientApi\Configuration\Conventions\Misc.java /}

###PrettifyGeneratedLinqExpressions

It determines if it should attempt to prettify the generated Linq expressions in definitions of indexes and transformers (used by the following classes: `AbstractIndexCreationTask`, `AbstractMultiMapIndexCreationTask`, `AbstractTransformerCreationTask` and `IndexDefinitionBuilder`).

// TODO java-sample
