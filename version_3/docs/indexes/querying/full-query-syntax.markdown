#Full RavenDB query syntax

A full query syntax is a combination of a [Lucene syntax](http://lucene.apache.org/core/old_versioned_docs/versions/2_9_4/queryparsersyntax.html) and RavenDB-specific extensions. This article describes Raven flavored options which a user can use to
construct index queries. The syntax that is presented here can by used to query by using either HTTP API or Client API ([Query](../client-api/commands/querying/how-to-query-a-database) and [DocumentQuery]() methods).

##Tokenized values

If you want to execute a query by using a field that was marked as *NotAnalyzed* in the index definition and your intention is to do an *exact match*, you have to tokenize it by using the following syntax:
{CODE-START: csharp /}
FieldName:[[Value]]
{CODE-END /}
It will cause that on the server side such field will be treated as not analyzed by a Lucene analyzer and will look for exact match of the query.

###Example I

To get all documents from Users collection by using the Raven/DocumentsByEntityName index use the query:

{CODE-START: csharp /}
Tag:[[Users]]
{CODE-END /}

There are also two reserved tokenized values `[[NULL_VALUE]]` and `[[EMPTY_STRING]]` that denotes respectively *null* and *string.Empty* values.

###Example II
In order to get users that have a null or empty name specify the query:

{CODE-START: csharp /}
Name:[[NULL_VALUE]] OR Name:[[EMPTY_STRING]]
{CODE-END /}

##Querying nested properties

If a document stored in a database contains an another object you need to use a *. (dot)* operator to query by using a nested value.

###Example III
The sample document:

{CODE-START: json /}
{
    "FullName" : {
        "FirstName" : "John",
        "LastName" : "Smith"
    }
}
{CODE-END /}

The query:

{CODE-START: csharp /}
FullName.FirstName:John
{CODE-END /}

##Quering collections

For querying into collections use a *, (comma)* operator.

###Example IV
The sample document:

{CODE-START: json /}
{
    "Tags" : [
        {
            "Name" : "sport"
        },
        {
            "Name" : "tv"
        }
    ]
}
{CODE-END /}

The query:

{CODE-START: csharp /}
Tags,Name:sport
{CODE-END /}


##Numeric values

If an indexed value is numeric then RavenDB will create two fields in the Lucene index. In the first one the numeric value will be stored as a not analyzed string while
the second one will have a <em>_Range</em> suffix and have a numeric form in order to allow to perform a range query. 

###Example V

If you want to query by the exact value just create the query as usual:

{CODE-START: csharp /}
Age:20
{CODE-END /}

The query above will return all users that are 20 years old.

###Example VI

If you need specify a range of the value then add the mentioned prefix to the property name that you are interested in. 
For example to ask for users whose age is greater than 20 we need to create the following query:

{CODE-START: csharp /}
Age_Range:{20 TO NULL}
{CODE-END /}


{INFO The syntax for range queries is the same like in Lucene. Inclusive range queries are denoted by square brackets. Exclusive range queries are denoted by curly brackets. /}

##ISO dates parsing

RavenDB supports parsing dates in ISO standard. 

###Example VII

In order to get users that was born between 1/1/1980 and 12/31/1999 use the following query:

{CODE-START: csharp /}
DateOfBirth:[1980-01-01 TO 1999-12-31T00:00:00.0000000]
{CODE-END /}

Note that we can use date only (*1980-01-01*) as well as pass it together with time (*1999-12-31T00:00:00.0000000*).

##Suggestions over multiple words

By default RavenDB support suggestions by using single search term. However if you need to find suggestions by using multiple words you have to use extended suggestion query syntax:

{CODE-START: csharp /}
<<word1 word2>>
{CODE-END /}

or 
{CODE-START: csharp /}
(word1 word2)
{CODE-END /}

You can provide any number of the words, the expected term separators are: '&nbsp;&nbsp;' (space), '\t', '\r', '\n'.

##Query methods

###`@in`

In order to specify multiple values in `Where` clause an operator `@in` was introduced. Its syntax looks as follow:

{CODE-START: csharp /}
@in<FieldName>:(value1, value2)
{CODE-END /}

You can specify any number of parameters that you are looking for. The *, (comma)* character is a separator of the values.

###Example VIII

{CODE-START: csharp /}
@in<Age>:(20, 25)
{CODE-END /}

The result of the query above will be users that are 20 or 25 years old.

###Example IX

In order to specify a phrase (i.e. when search term contains whitespaces) wrap it by using \\". 
If you need to escape a comma character, wrap it by using grave accent (\`) character. For example apply the following query to Users/ByVisitedCountries index:

{CODE-START: csharp /}
@in<VisitedCountries>:(\"Australia`,` Canada\", Israel)
{CODE-END /}

This will cause the exact search for string values *"Australia, Canada"* or *"Israel"*.

{INFO The operator `@in` is used internally by `WhereIn` method. If the specified list of values is empty then instead of `@in` the following expression is created `@emptyIn<FieldName>:(no-results)` to handle such cases./}

