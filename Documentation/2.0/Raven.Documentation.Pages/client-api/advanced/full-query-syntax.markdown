#Full RavenDB query syntax

A full query syntax is a combination of a [Lucene syntax](http://lucene.apache.org/core/2_9_4/queryparsersyntax.html) and RavenDB-specific extensions. This section describes all Raven flavored options which a user can use to
construct index queries. The syntax that is presented here can by used to query by using either HTTP API or Client API 
(*IDatabaseCommands.Query* and *IDocumentSession.Advanced.LuceneQuery* methods).

##Tokenized values

If you want to execute a query by using a fields that was marked as *NotAnalyzed* in the index definition and your intention is to do an *exact match*, you have to tokenize it by using

	FieldName:[[Value]]

syntax. It will cause that on the server side such field will be treated as not analyzed by a Lucene analyzer and will look for exact match of the query.

{CODE tokenized_field@ClientApi/Advanced/FullQuerySyntax.cs /}

There are also two reserved tokenized values `[[NULL_VALUE]]` and `[[EMPTY_STRING]]` that denotes respectively *null* and *string.Empty* values.
In order to get users that have null or empty name perform the query:

{CODE name_null_or_empty@ClientApi/Advanced/FullQuerySyntax.cs /}

##Querying nested properties

If a document stored in database contains another nested object you need to use a *. (dot)* operator to ask for nested value. The usage of this operator is shown below:

{CODE nested_properties@ClientApi/Advanced/FullQuerySyntax.cs /}

Note that we provided a word *dynamic* as an index name what tells to the RavenDB that a dynamic index should be used.

##Quering collections

For querying into collections, we use a *, (comma)* operator. This time let's use dynamic `LuceneQuery`:

{CODE users_by_tag_sportsman@ClientApi/Advanced/FullQuerySyntax.cs /}

##Numeric values

If an indexed value is numeric then RavenDB will create two fields in the Lucene index. In the first one the numeric value will be stored as a not analyzed string while
the second one will have a <em>_Range</em> suffix and be in a numeric form that will allow to do range queries. 

If you want to query by the exact value just create the query as usual:

{CODE age_exact@ClientApi/Advanced/FullQuerySyntax.cs /}

The query above will return all users that are 20 years old.

If you need to look for numeric value range, add the mentioned prefix to the property name that you are interested in. 
For example to ask for users whose age is greater than 20 we need to create the following query:

{CODE age_range@ClientApi/Advanced/FullQuerySyntax.cs /}

{INFO The syntax for range queries is the same like in Lucene. Inclusive range queries are denoted by square brackets. Exclusive range queries are denoted by curly brackets. /}

##ISO dates parsing

RavenDB supports parsing dates in ISO standard. For example to get users that was born between 1/1/1980 and 12/31/1999 use the following query:

{CODE users_by_dob@ClientApi/Advanced/FullQuerySyntax.cs /}

Note that we can use date only (*1980-01-01*) as well as pass it together with time (*1999-12-31T00:00:00.0000000*).

##Suggestions over multiple words

By default RavenDB support suggestions by using single search term. However if you need to find suggestions by using multiple words you have to use extended suggestion query syntax:

`<<word1 word2>>` or `(word1 word2)`

You can provide any number of the words, the expected term separators are: '&nbsp;&nbsp;' (space), '\t', '\r', '\n'.

{CODE suggestion_syntax@ClientApi/Advanced/FullQuerySyntax.cs /}

##Query methods

###`@in`

In order to specify multiple values in `Where` clause an operator `@in` was introduced. Its syntax is the following:

`@in<FieldName>:(value1, value2)`

You can specify any number of paramters that you are looking for. The *, (comma)* character is a separator of the values.

{CODE in_method@ClientApi/Advanced/FullQuerySyntax.cs /}

The result of the query above will be users that are 20 or 25 years old.

In order to specify a phrase (i.e. when search term contains whitespaces) wrap it by using \\". 
If you need to escape a comma character, wrap it by using grave accent (\`) character.  e.g.:

{CODE in_method_comma@ClientApi/Advanced/FullQuerySyntax.cs /}

This will cause the exact search for string values *"Australia, Canada"* or *"Israel"*.

{INFO The operator `@in` is used internally by `WhereIn` method. If the specified list of values is empty then instead of `@in` the following syntax is created `@emptyIn<FieldName>:(no-results)` to handle such cases./}

