How RavenDB uses Lucene
***********************

.. contents:: In this chapter...
  :depth: 3
  
  
Lucene
======
The RavenDB indexing mechanism is implemented using the open-source `Lucene.NET library 
<http://lucene.apache.org/lucene.net/>`_, a C# port of the `original Java library <http://lucene.apache.org/>`_. 

**Lucene is a full-text search library that makes it easy to add search functionality to an application.**
**It does so by adding content to a full-text index. It then searches this index and returns results ranked by either 
the relevance to the query or by an arbitrary field such as a document's last modified date.**

The best way of thinking about the indexes in RavenDB is to imagine them as a database's materialized view. 
Raven executes your indexes in the background, and the results are written to disk. This means that when we perform a
query, we have to do very little work. This is how RavenDB manages to achieve its near instantaneous replies for 
your queries, it doesn't have to think, all the processing has already been done.

Lucene comes with an advanced set of `query options <http://lucene.apache.org/java/2_4_0/queryparsersyntax.html>`_, 
that allow RavenDB to support the following (which is still just a partial list):

* full text search
* partial string matching
* range queries (date, integer, float etc)
* spatial searches
* auto-complete or spell-checking
* faceted searches


How indexes are stored
======================
Let's start by looking at a simple scenarion. Let's assume we have the type of document as shown in listing 4.1::

  // Listing 4.1 - A sample blog post   
  { // Document id: users/101
    "Name": "Matt Warren",
    "Age": "Age 30",        
  }

And the following index::

  // Listing 4.2 - A simple index
  var index = new IndexDefinition()
  {
    Map = "docs => from doc in docs select new { doc.Name }",		
  };
  db.DatabaseCommands.PutIndex("SimpleIndex", index);
	
.. note::
  In this chapter all code samples will be written using the Lucene syntax as we are looking at Lucene itself. 
  However the recommended way of using RavenDB is via the LINQ API, see :ref:`Chapter 3 <Chapter03>` 
  for more information about this.
  
Take a look at figure 4.3 to see how a simple index is stored

.. figure::  _static/LuceneDefaultNOTAnalysedIndex.png

  Figure 4.3 - A simple index
  
So by default RavenDB does the following when indexing a text field:

* Analyzes the fields using a *lower case* analyzer ("Matt Warren" -> "matt warren")
* Stores a the ID of the document that the terms comes from

The fields is converted to lower case so that case-sensitivity isn't an issue in basic queries. The ID of the document
is stored so that RavenDB can then pull the document out of the data store after is has performed the Lucene query. 
Remember RavenDB only uses Lucene to store the *indexed data*, not the actual documents themselves. This reduces the total
size of the index.

However things are slightly more complex when dealing with numbers. The rules that RavenDB follows here are:

* If the value is null, create a single field with the supplied name and the unanalyzed value 'NULL_VALUE'
* If the value is a string or was set to be not analyzed, create a single field with the supplied name and value
* If the value is a date, create a single field with millisecond precision and the supplied name
* If the value is numeric (int, long, double, decimal, or float) it will create two fields
  * using the field name, containing the numeric value as an unanalyzed string - useful for direct queries
  * using the field name +'_Range', containing the numeric value in a form that allows range queries
  
The last item is important. To enable RavenDB to perform range queries (i.e. Age > 4, Age < 40 etc) with Lucene, it 
needs to store the numerical data in a format that is suitable for this. But it also stores the value in its original 
format so that direct queries (such as matches) can be performed.

Take a look at figure 4.4 to see how a complex index is stored

.. figure::  _static/LuceneComplexIndex.png

  Figure 4.4 - A complex index
  
Advanced Lucene Options
=======================

RavenDB gives you full control on the indexing process, by exposing the low-level Lucene options as part of the 
index definition. You can use these like so::

    IndexDefinition indexAnalysed = new IndexDefinition()
        {    
            Map = "docs.Users.Select(doc => new {Name = doc.Name})",
            Analyzers = { {"Name", typeof(SimpleAnalyzer).FullName} },
            SortOptions = { { "Age", SortOptions.Double } }
            Stores = { { "Name", FieldStorage.Yes } }
        };

Analzying
^^^^^^^^^
By default RavenDB uses a *lower case* analyser, this converts a string into a lower case version. But this isn't
useful if you'd like to a full-text search on your documents. To achieve this you need to tokenise or analyse the 
fields you are indexing.

For instance given a field that contains the text "The quick brown fox jumped over the lazy dog, bob@hotmail.com 123432.", 


*Keyword Analyzer* keeps the entire stream as a single token.

[The quick brown fox jumped over the lazy dog, bob@hotmail.com 123432.]


*Whitespace Analyzer* tokenizes on white space only (note the punctuation at the end of "dog")

[The]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dog,]   [bob@hotmail.com]   [123432.]


*Stop Analyzer* strips out common English words (such as "and", "at" etc), tokenizes letters only and converts everything to lower case

[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob]   [hotmail]   [com]


*Simple Analyzer* only tokenizes letters and makes all tokens lower case

[the]   [quick]   [brown]   [fox]   [jumped]   [over]   [the]   [lazy]   [dog]   [bob]   [hotmail]   [com]


*Standard Analyzer* simple tokenizer that uses a stop list of common English works, also handles numbers and emails addresses correctly

[quick]   [brown]   [fox]   [jumped]   [over]   [lazy]   [dog]   [bob@hotmail.com]   [123432]


You would then perform the same analysis on the text you want to match. For instance "quick brown" -> [quick] [brown]
and Lucene would find all the documents with both of these terms in.


Sorting
^^^^^^^
When Lucene sorts values it performs this against a encoded version of the number (a binary representation). 
This means that is certain situations it can get the sort order wrong. For instance when sorting double and float
values or short/int/long values. To get round this issue you can explicitly set the sort option of the field.

Storage
^^^^^^^
For completeness RavenDB allows you to control whether or not a field is stored in the index. This could be useful 
if you wanted to pull back data directly from the Lucense index, but there are very few scenarious where this is
useful. It's far better to let RavenDB handle this for you, so specifying this option isn't really recommended.
Note that RavenDB allows to use projections directly from the document, without needing to store them in the index, 
that means that there usually aren't good reasons to store fields data.

Indexing
^^^^^^^^
Indexing allows you to control how you can search on an index. For the most part, you can just leave that to RavenDB's
defaults. This options, along with the storage option, are there for completion sake, more than anything else, and is 
only going to be useful for expert usage, if that.