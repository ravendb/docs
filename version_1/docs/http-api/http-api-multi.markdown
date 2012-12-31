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
    [
       {
                 "name":"ayende",
                "@metadata":{
                    "Content-Type":"application/x-www-form-urlencoded",
                    "@id":"users/ayende",
                    "@etag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"
                }
       },
       {
                "name":"oren",
                "@metadata":{
                    "Content-Type":"application/x-www-form-urlencoded",
                    "@id":"users/oren",
                    "@etag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"
                 }
       }
    ]
{CODE-END /}

More formally, we POST a request to '/queries', with the content of a JSON array of document ids. The response for such a request is a JSON array of documents, include the document metadata.

Important: If you request a non existing key, the request is ignored. In other words, the output of this request and the previous ones are identical.Â

{CODE-START:json /}
    > curl -X POST http://localhost:8080/queries -d "['users/ayende','does not exists', 'users/oren']"
    [
          {
                "name":"ayende",
                "@metadata":{
                       "Content-Type":"application/x-www-form-urlencoded",
                       "@id":"users/ayende",
                       "@etag":"7f9cd674-4c6f-11df-8ec2-001fd08ec235"
                }
          },
          {
                "name":"oren",
                "@metadata":{
                       "Content-Type":"application/x-www-form-urlencoded",
                       "@id":"users/oren",
                       "@etag":"7f9cd675-4c6f-11df-8ec2-001fd08ec235"
               }
          }
    ]
{CODE-END /}

In other words, you cannot rely on the size of the returned array to be the same as the array of ids requested.
Aside from missing documents, which are ignored, the order of the documents in the returned array match the order of ids in the requested array.

## Set based operations

Typically, document databases don't support set based operations. Raven does for deletes and updates, for inserts, you can POST to the [bulk_docs](http://ravendb.net/docs/http-api/multi/http-api-multi-batching?version=1.0) endpoint (this is how the client API behaves).

Set based operations are based on very simple idea, you pass a query to a Raven index, and Raven will delete all the documents matching the query. All operations that are supported with an [index query](http://ravendb.net/docs/http-api/http-api-indexes-querying?version=1.0) are supported for set based operations. You need to specify the index that you intend to operate on, the actual query, the [optional cut off point](http://ravendb.net/docs/theory/indexes/docs-http-indexes?version=1.0) and whatever to allow this operation over a stale index.

Note that Raven indexes are allowed to be stale. If the index for the set based operation is stale, Raven will fail the operation. You can control this behavior using the following options:

* cutOff - determines what is the cut off point for considering the index stale.
* allowStale - determines if the operation is allowed to proceed on a stale index (default: false)

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

Set based updates work very similarly to set based deletes. They require an index to operate on an a query for this index. But they use the [PATCH format](http://ravendb.net/docs/http-api/singledocumentoperations/http-api-patch?version=1.0) as their payload. For example, if we wanted to mark all the users who haven't logged on recently as inactive, we could define the following index:

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