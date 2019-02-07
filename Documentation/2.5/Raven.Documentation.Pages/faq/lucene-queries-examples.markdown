# Lucene queries example

## Syntax

[http://lucene.apache.org/core/2_9_4/queryparsersyntax.html](http://lucene.apache.org/core/2_9_4/queryparsersyntax.html)

## Looking for words

In case we want to find all documents with property `Text` containing some word.

`
    Text:Word
`

## Looking for phrases

In case we want to find all documents with property `Text` containing specified sentence.

`
    Text:"several words"
`

## DateTime ranges

Take a look at these tests.

[http://github.com/ravendb/ravendb/commit/721c50ea51ff7721928cca76de957e9f7d9e3786](http://github.com/ravendb/ravendb/commit/721c50ea51ff7721928cca76de957e9f7d9e3786)

## How can I query for a null value?

Assuming when the document was saved the property's value was null; let us say we want to find all users that didn't provide their `Email`. This is the query that we will need to issue:

`
    Email:[[NULL_VALUE]]
`

The [[ ]] denotes a NotAnalyzed value, and Raven writes `NULL_VALUE` to the Lucene index when it encounters a null being indexed.

## How can I query for a non-null blank or empty string?

Assuming when the document was saved the property's value was a non-null blank/empty string; let us say we want to find all users that didn't provide their `LastName`. In order to query for a non-null blank or empty string we will need to issue:

`
    LastName: [[EMPTY_STRING]]
`

The [[ ]] denotes a NotAnalyzed value, and Raven writes `EMPTY_STRING` to the Lucene index when it encounters a non-null empty/blank string being indexed.
