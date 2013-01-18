#Full RavenDB query syntax

A full query syntax is a combination of a Lucene syntax and RavenDB-specific extensions. This section describes all options which a user can use to
construct index queries. The syntax that is presented here can by used to query by using either HTTP API or Client API 
(`IDatabaseCommands.Query` and `IDocumentSession.Advanced.LuceneQuery` methods).

##Tokenized values

If you want to execute a query by using a fields that was maked as *NotAnalyzed* in the index definition and your intention is to do an *exact match*, you have to tokenize it by using

	FieldName:[[Value]]

syntax. It will cause that on the server side such field will be treated as not analyzed by a Lucene analyzer and will look for exact match of the query.

{CODE tokenized_field@ClientApi/Advanced/FullQuerySyntax.cs /}

There are also two reserved tokenized values `[[NULL_VALUE]]` and `[[EMPTY_STRING]]` that denotes respectively *null* and *string.Empty* values.
In order to get users that have null or empty name perform the query:

{CODE name_null_or_empty@ClientApi/Advanced/FullQuerySyntax.cs /}

##Querying nested properties

If a document stored in database contains another nested object you need to use *.* (dot) operator to ask for nested value. The usage of this operator is shown below:

{CODE nested_properties@ClientApi/Advanced/FullQuerySyntax.cs /}

Note that we provided *dynamic* as an index name what tells to the RavenDB that a dynamic index should be used.

##Quering collections

For querying into collections, we use the *,* (comma) operator. This time let's use dynamic `LuceneQuery`:

{CODE users_by_tag_sportsman@ClientApi/Advanced/FullQuerySyntax.cs /}

##ISO dates parsing

RavenDB supports parsing dates in ISO standard. For example to get users that was born between 1/1/1980 and 12/31/1999 use the following query:

{CODE users_by_dob@ClientApi/Advanced/FullQuerySyntax.cs /}

Note that we can use date only (1980-01-01) as well as pass it together with time (1999-12-31T00:00:00.0000000).

##Suggestions over multiple words

By default RavenDB support suggestions by using single search term. However if you need to find suggestions by using multiple words you have use extending query syntax:

`<<word1 word2 etc>>` or `(word1 word2 etc)`

You can provide any number of words, the expected terms separators are: ' ' (space), '\t', '\r', '\n'.

