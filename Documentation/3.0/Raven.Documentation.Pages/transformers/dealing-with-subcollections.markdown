# Dealing with subcollections

By default, each found document in index is passed once to the transformer, in most cases this is a valid assumption, but there are cases when this behavior needs to be changed.

The best example is when one document produces multiple index entries which are stored and we want to extract them from index and pass to transformer. Default behavior would filter-out duplicates (by duplicates we mean index entries that originate from same document) so only one result would be transformed. To address this situation the `SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer` configuration option has been introduced.

## Example

Let's us assume that we want to get all `Product` names for specific `Order`. Our index would look like:

{CODE transformers_1@Transformers/Subcollections.cs /}

and transformer would look like:

{CODE transformers_2@Transformers/Subcollections.cs /}

Default behavior would return us 6 out of 12 results:

{CODE transformers_3@Transformers/Subcollections.cs /}

But, when `SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer` is used, we get all results that we want:

{CODE transformers_4@Transformers/Subcollections.cs /}

## Related articles

- [[Client API] SetAllowMultipleIndexEntriesForSameDocumentToResultTransformer](../client-api/session/querying/how-to-customize-query#setallowmultipleindexentriesforsamedocumenttoresulttransformer)