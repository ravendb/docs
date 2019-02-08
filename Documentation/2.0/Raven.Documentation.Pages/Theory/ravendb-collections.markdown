#RavenDB Collections
RavenDB's collections are similar to [MongoDB's collections](https://docs.mongodb.com/manual/reference/glossary/#term-collection), they serve as a simple way to group related documents together. The expected usage pattern is that collections are used to group documents with similar structure, although that is not required. From the database standpoint, a collection is just a group of documents that share the same entity name.

##Raven-Entity-Name
Collections are defined in the document metadata, more specifically, the collection a document belongs to is specified as the value of doc["@metadata"]["Raven-Entity-Name"]. Using the HTTP API, we can define the following document in the "Users" collection.

    PUT http://localhost:8080/docs/users/ayende HTTP/1.1  
    Raven-Entity-Name: Users  
    Host: localhost:8080  
    Content-Length: 23
    
    { "Name": "Ayende" }

As you can see, the HTTP header Raven-Entity-Name specifies that this document belongs to the Users collection.

##Using collections in indexes
Probably the most important role for collections is their use in indexes. RavenDB allows the use of the collection name when filtering the target audience for documents.
For example, this index definition:

{CODE-START:json /}
{
	'Map' : 'from post in docs.Posts select new { post.Title, post.PostedAt};'
}
{CODE-END /}
    

More formally, Raven filters the selected documents based on the name of the collection from the overall docs.
The index definition above is equivalent to the following index definition:

{CODE-START:json /}
{
	'Map' : 'from doc in docs
			where doc["@metadata"]["Raven-Entity-Name"] == "Posts"
			let post = doc
			select new { post.Title, post.PostedAt };'
}
{CODE-END /}

Using collections in this manner significantly simplifies the index definition and is strongly recommended.

##Raven/DocumentsByEntityName
By default, RavenDB defines the index 'Raven/DocumentsByEntityName' as follows:

{CODE-START:json /}
{
	'Map' : 'from doc in docs
				let Tag = doc["@metadata"]["Raven-Entity-Name"]
				select new { Tag, LastModified = (DateTime)doc["@metadata"]["Last-Modified"] };' 
}
{CODE-END /}

This allows querying for documents based on their entity name using:

{CODE-START:json /}
http://localhost:8080/indexes/Raven/DocumentsByEntityName?query=Tag:Users
&nbsp;
{
	"Results":[
		{
			"Name":"Ayende",
			"@metadata":{
			"Raven-Entity-Name":"Users",
			"@id":"users/ayende",
			"@etag":"ecdb775b-4c96-11df-8ec2-001fd08ec235"
			}
		}
	],
	"IsStale":false,
	"TotalResults":1
}
{CODE-END /}