# Results Transformers

Results Transformers have been introduced in RavenDB 2.5 to give the user the ability to do a server side projections (with possibility to load data from other documents).

{NOTE Result Transformers are substituting the index [TransformResults](transform-results) feature, marking it as obsolete. /}

Main features of the Result Transformers are:   

I. Stand-alone, separated from Index.   

{CODE result_transformers_0@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}

{CODE result_transformers_1@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}

II. User can use them on index results on demand.

What does it mean? It means if you want load whole `Order` you can do so by not using a transformer in the query:    

{CODE result_transformers_3@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}

or if you want to get your transformed results then you can execute query as follows:    

{CODE result_transformers_2@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}

{CODE result_transformers_4@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}

III. Can be used with automatic indexes.   

{CODE result_transformers_5@ClientApi\Querying\ResultsTransformation\ResultTransformers.cs /}