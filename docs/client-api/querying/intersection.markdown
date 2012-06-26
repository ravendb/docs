#Intersection Queries

##Setup
{CODE intersectPOCO@Consumer\IntersectionQueries.cs /}


##Index
{CODE intersectIndex@Consumer\IntersectionQueries.cs /}

##Sample Data
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


![Figure 1: Polymorphic indexes](images/intersect_search_lucene_doc_internals.png)

##Querying
{CODE intersectQuery@Consumer\IntersectionQueries.cs /}

__Note__ because of the way Intersection Queries work, any ordering you specify is applied to the first *Where* clause in your query. This ordering is then shared by the other *Where* clauses