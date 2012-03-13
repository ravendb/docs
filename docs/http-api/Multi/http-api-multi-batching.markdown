#Batching requests
RavenDB supports batching multiple operations into a single request, reducing the number of remote calls and allowing several operations to share the same transactions.
Request batching in RavenDB is handled using the '/bulk_docs' endpoint, which accepts an array of operations to execute. The format for the operations is:

* method - PUT, PATCH or DELETE.
* key - the document key that this operation pertains to.
* etag - optional - the etag to check against the current document etag.
* document - the JSON document to PUT.
* @metadata - the metadata associated with the document to PUT.

Below you can see an example of the the operation format:

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
    
This can be executed using curl with the following syntax:

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

##Concurrency
If an etag is specified in the command, that etag is compared to the current etag on the document on the server. If the etags do no match, a 409 Conlict status code is returned. In such a case, the entire operation fails and non of the updates that were tried will succeed.

##Transactions
All the operations in the batch will succeed or fail as a transaction. Other users will not be able to see any of the changes until the entire batch completes.
