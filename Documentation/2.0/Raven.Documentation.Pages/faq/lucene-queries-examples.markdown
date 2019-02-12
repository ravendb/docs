# Lucene queries example

## Syntax

[http://lucene.apache.org/core/2_9_4/queryparsersyntax.html](http://lucene.apache.org/core/2_9_4/queryparsersyntax.html)

## Looking for words

In case we want to find all documents with property Text containing some word.

`
    Text:Word
`

## Looking for phrases

In case we want to find all documents with property Text containing specified sentence.

`
    Text:"several words"
`

## DateTime ranges

Take a look at these tests.

[https://github.com/ravendb/ravendb/commit/721c50ea51ff7721928cca76de957e9f7d9e3786](https://github.com/ravendb/ravendb/commit/721c50ea51ff7721928cca76de957e9f7d9e3786)

## How can I query for a null value?

Let us say that we want to find all users that didn't provide their email. This is the query that we will need to issue:

`
    Email:[[NULL_VALUE]]
`

The [[ ]] denotes and NotAnalyzed value, and Raven writes NULL_VALUE to the Lucene index when it encounters a null being indexed.
