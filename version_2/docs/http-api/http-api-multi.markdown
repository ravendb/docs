# Commands on multiple documents

## GET

We can address documents directly by using their key (in the following example, the key is 'users/ayende'):

{CODE-START:json /}
    > curl http://localhost:8080/docs/users/ayende

    {
      "name": "ayende"
    }
{CODE-END /}

But while that is useful, there are often scenarios where we want to get more than a single document. In order to avoid the common SELECT N+1 issues, RavenDB supports the ability to get multiple documents in a single remote call.
 
We load the database with the following two documents:

{CODE-START:json /}
     > curl -X PUT http://localhost:8080/docs/users/ayende -d "{ name: 'ayende'}"
    {"Key":"users/ayende","ETag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"}
    > curl -X PUT http://localhost:8080/docs/users/oren -d "{ name: 'oren'}"
    {"Key":"users/oren","ETag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"}
{CODE-END /}

And now, in order to get them both in a single query, we use:

{CODE-START:json /}
> curl -X POST http://localhost:8080/queries -d "['users/ayende','users/oren']"
{
"Results":
	[
		{				
			"name":"ayende",
			"@metadata":
			{	
				"@id":"users/ayende",
				"Last-Modified":"2012-11-16T13:29:08.5880000",
				"@etag":"00000000-0000-2500-0000-000000000002",
				"Non-Authoritative-Information":false
			}
		},
		{		
			"name":"oren",
			"@metadata":
			{
				"@id":"users/oren",
				"Last-Modified":"2012-11-16T13:29:17.9650000",
				"@etag":"00000000-0000-2500-0000-000000000003",
				"Non-Authoritative-Information":false
			}
		}
	],
"Includes":[]
}
{CODE-END /}

More formally, we POST a request to '/queries', with the content of a JSON array of document ids. The response for such a request is a JSON
object that consists of two arrays of documents - *Results* and *Includes*. Each document also includes the metadata.

The *Results* array has documents that we asked for, while the *Includes* array contains optionally referenced documents. To determine what a reference you want to retrieve together with a document add the *include* parameter to the query string.

For example, let's modify `users/ayende` by adding `location` property which is a reference to `locations/1` document:

{CODE-START:json /}
> curl -X PUT http://localhost:8080/docs/locations/1 -d "{ country: 'Israel', city: 'Hadera' }"
   
> curl -X PUT http://localhost:8080/docs/users/ayende -d "{ name: 'ayende', location: 'locations/1' }"
{CODE-END /}

Now let's perform the following request:

{CODE-START:json /}
curl -X POST http://localhost:8080/queries?include=location -d "['users/ayende','users/oren']"

{
"Results":
	[
		{
			"name":"ayende",
			"location":"locations/1",
			"@metadata":
			{
				"@id":"users/ayende",
				"Last-Modified":"2012-11-19T07:42:58.9170000",
				"@etag":"00000000-0000-2600-0000-000000000003",
				"Non-Authoritative-Information":false
			}
		},
		{
			"name":"oren",
			"@metadata":
			{
				"@id":"users/oren",
				"Last-Modified":"2012-11-16T13:29:17.9650000",
				"@etag":"00000000-0000-2500-0000-000000000003",
				"Non-Authoritative-Information":false
			}
		}
	],
"Includes":
	[
		{
			"country":"Israel",
			"city":"Hadera",
			"@metadata":
			{
				"@id":"locations/1",
				"Last-Modified":"2012-11-19T07:45:22.1400000",
				"@etag":"00000000-0000-2600-0000-000000000004",
				"Non-Authoritative-Information":false
			}
		}
	]
}
{CODE-END /}

Note that now the *Includes* array is not empty and contains the `locations/1` document.

{INFO You can add *include* parameter to the query string multiple times to get all referenced documents, e.g. /queries?include=location&include=projects /}
 
Important: If you request a non existing key, the request is ignored. In other words, the *Results* of this request and the previous ones are identical.

{CODE-START:json /}
> curl -X POST http://localhost:8080/queries -d "['users/ayende','does not exists', 'users/oren']"
{
"Results":
	[
		{
			"name":"ayende",
			"location":"locations/1",
			"@metadata":
			{
				"@id":"users/ayende",
				"Last-Modified":"2012-11-19T07:42:58.9170000",
				"@etag":"00000000-0000-2600-0000-000000000003",
				"Non-Authoritative-Information":false
			}
		},
		{
			"name":"oren",
			"@metadata":
			{
				"@id":"users/oren",
				"Last-Modified":"2012-11-16T13:29:17.9650000",
				"@etag":"00000000-0000-2500-0000-000000000003",
				"Non-Authoritative-Information":false
			}
		}
	],
"Includes":[]
}
{CODE-END /}

In other words, you cannot rely on the size of the returned array to be the same as the array of ids requested.
Aside from missing documents, which are ignored, the order of the documents in the returned array match the order of ids in the requested array.

Besides requesting by using POST to get multiple documents you can also use HTTP GET method. Then create the request as follows:

{CODE-START:json /}
> curl -X GET http://localhost:8080/queries?id=users/ayende"&"id=users/oren
{CODE-END /}

## Set based operations

Typically, document databases don't support set based operations. Raven does it for deletes and updates. For inserts, you can POST to the [bulk_docs](http-api-multi#batching-requests) endpoint (this is how the Client API behaves).

Set based operations are based on very simple idea, you pass a query to a Raven index, and Raven will delete all the documents matching the query. All operations that are supported with an [index query](http://ravendb.net/docs/http-api/http-api-indexes-querying?version=2.0) are supported for set based operations. You need to specify the index that you intend to operate on, the actual query, the [optional cut off point](http://ravendb.net/docs/theory/indexes/docs-http-indexes?version=2.0) and whatever to allow this operation over a stale index.

Note that Raven indexes are allowed to be stale. If the index for the set based operation is stale, Raven will fail the operation. You can control this behavior using the following options:

* cutOff - determines what is the cut off point for considering the index stale.
* allowStale - determines if the operation is allowed to proceed on a stale index (default: false)

The analogous Client API reference section you will find [here](../client-api/set-based-operations).

### Set based deletes

For example, let us say that we wanted to delete all the inactive users, we can define an index for the user activity status:

{CODE-START:plain /}
    from user in docs.Users
    select new{user.IsActive}
{CODE-END /}

And now we can issue the following command:

{CODE-START:plain /}
DELETE http://localhost:8080/bulk_docs/UsersByActivityStatus?query=IsActive:False
{CODE-END /}

This will remove all the documents from the UsersByActivityStatus where IsActive equals to false.

This is the equivalent for:

{CODE-START:plain /}
    DELETE FROM Users
    WHERE IsActive = 0
{CODE-END /}

###Set based updates

Set based updates work very similarly to set based deletes. They require an index to operate on an a query for this index. But they use the [PATCH format](../http-api/http-api-single#patch) as their payload. For example, if we wanted to mark all the users who haven't logged on recently as inactive, we could define the following index:

{CODE-START:plain /}
    from user in docs.Users
    select new { user.LastLoginDate }
{CODE-END /}

And then issue the following command:

{CODE-START:json /}
PATCH http://localhost:8080/bulk_docs/UsersByLastLoginDate?query=LastLoginDate:[NULL TO 20100527]

    [
       { "Type": "Set", "Name": "IsActive", "Value": false
    ]
{CODE-END /}

This is the equivalent for:

{CODE-START:plain /}
    UPDATE Users
    SET IsActive = 0
    WHERE LastLoginDate < '2010-05-27'
{CODE-END /}

##Batching requests
RavenDB supports batching multiple operations into a single request, reducing the number of remote calls and allowing several operations to share the same transactions.
Request batching in RavenDB is handled using the '/bulk_docs' endpoint, which accepts an array of operations to execute. The format for the operations is:

* method - PUT, PATCH or DELETE.
* key - the document key that this operation pertains to.
* etag - optional - the etag to check against the current document etag.
* document - the JSON document to PUT.
* @metadata - the metadata associated with the document to PUT.

Below you can see an example of the the operation format:

{CODE-START:json /}
    [
        {
            Method: "PUT",
            Document:
            {
                name: "BatchPut1_Name"
            },
            Metadata:
            {
                info: "BatchPut1_Info"
            },
            Key: "BatchPut1"
        },
        {
            Method: "PUT",
            Document:
           {
               name: "BatchPut2_Name"
           },
           Metadata:
           {
               info: "BatchPut2_Info"
           },
            Key: "BatchPut2"
        },
        {
            Method: "DELETE",
            Key: "BatchPut1"
        },
        {
            Method: "DELETE",
            Key: "NonExistent"
        }
    ]
{CODE-END /}
    
This can be executed using curl with the following syntax:

{CODE-START:json /}
    > curl http://localhost:8080/bulk_docs -X POST -d "[ { Method:'PUT', Document:{  name:'BatchPut1_Name' }, Metadata:{  info:'BatchPut1_Info' },Key:'BatchPut1' }, 
                                                       { Method:'PUT', Document:{  name:'BatchPut2_Name' }, Metadata:{  }, Key:'BatchPut2' } ]"
    [
     {
         "Etag":"4c06db4e-4c86-11df-8ec2-001fd08ec235",
         "Method":"PUT",
         "Key":"BatchPut1"
     },
     {
         "Etag":"4c06db4f-4c86-11df-8ec2-001fd08ec235",
         "Method":"PUT",
         "Key":"BatchPut2"
     }
    ]
{CODE-END /}

###Concurrency
If an etag is specified in the command, that etag is compared to the current etag on the document on the server. If the etags do no match, a 409 Conflict status code is returned. In such a case, the entire operation fails and non of the updates that were tried will succeed.

###Transactions
All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.