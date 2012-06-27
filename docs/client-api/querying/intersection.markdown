#Query Intersection

##Motivation
Under-the-hood RavenDB uses [Lucene.NET](http://incubator.apache.org/lucene.net/) for indexing. One issue with this is Lucene will only index data in a *flat* format, so you loose any *relational* information. For instance given the following json docs:

{CODE-START:json /}
{
	"Id": "tshirt/1",
    "Name": "Wolf", 
    "Barcode": 10001, 
    "Types": [
        { "Colour": "Blue",   "Size": "Small" }, 
        { "Colour": "Black",  "Size": "Small" }, 
        { "Colour": "Black",  "Size": "Medium" }, 
        { "Colour": "Grey",   "Size": "Large" }
    ]
}
{CODE-END /}

{CODE-START:json /}
{
    "Id": "tshirt/2",
    "Name": "Wolf", 
    "Barcode": 10002, 
    "Types": [
        { "Colour": "Blue",   "Size": "Small" }, 
        { "Colour": "Black",  "Size": "Large" },         
        { "Colour": "Grey",   "Size": "Medium" }
    ]
}
{CODE-END /}

There are 2 ways you can store them in the index. You can either completely flatten out the *Types*, using an index like this:

{CODE-START:csharp /}
from shirt in docs.TShirts
from type in s.Types
	select new 
	{ 
		shirt.Name, 
		type.Colour, 
		type.Size, 		
	}
{CODE-END /}

In which case you end up with the following documents in the Lucene index:

![Figure 1: Polymorphic indexes](images/intersect_search_lucene_doc_internals_1.png)

**Remember** in RavenDB a "select new { .. }" statement ends up creating a single *Lucene* document in the index (Lucene documents are distinct from RavenDB documents). 

Alternatively you can keep the *Types* within their parent *TShirt* as demonstrated in the following index:

{CODE-START:csharp /}
from shirt in docs.TShirts
	select new 
	{ 
		shirt.Name, 
		shirt.Types.SelectMany(t => t.Colour)
		shirt.Types.SelectMany(t => t.Size)
	}
{CODE-END /}

However, you then lose the associatation between the individual *Types*, as seen below:

![Figure 2: Polymorphic indexes](images/intersect_search_lucene_doc_internals_2.png)

In the majority of cases you can perform all the queries you need to, using 1 of the approaches above. However, without intersection queries you are not able to say, *"Give me all the TShirts that have (Colour = Blue, Size = Small) AND (Colour = Gray, Size = Large)"*.

**Note:** you could do that query if you put an "OR" in there, but not "AND". Hopefully the diagrams above show why this is the case.

Hence the need for Query Intersection.

##Setting up Intersection Queries
So, given the following classes and index:

{CODE intersectPOCO@Consumer\IntersectionQueries.cs /}

{CODE intersectIndex@Consumer\IntersectionQueries.cs /}

##Querying
You can then write the following query, which will give you the expected results:

{CODE intersectQuery@Consumer\IntersectionQueries.cs /}

__Note__ because of the way Intersection Queries work, any ordering you specify is applied to the first *Where* clause in your query. This ordering is then shared by the other *Where* clauses

##More Information
For a more in-depth technical explanation of this feature, including some information on the internals and how it was implemeneted, see ????