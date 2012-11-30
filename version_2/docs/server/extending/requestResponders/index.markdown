# Request Responders

Raven allow users to extend the type of requests it can handle. That is done by subclassing the RequestResponder class and dropping the resulting dll in the Plugins directory.
For example, let us assume that you have a burning desire to know what is the size of a document without retrieving it. You can do that by implementing the following responder:

{CODE index_1@Server\Extending\RequestResponders\Index.cs /}
 
The next step is compiling this and dropping the dll into the Plugins directory. With that, we can issue:

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/docsize/bobs_address
{CODE-END /}

Assuming there is a document with an id of "bobs_address", RavenDB will respond with the contents of that document and an HTTP 200 OK response code:

{CODE-START:json /}
    HTTP/1.1 200 OK
    
    {
      "Key": "bobs_address",
      "Size": "396"
    }
{CODE-END /}

## Expected usages
This feature is provided mostly for Raven's own need, more than to provide an extension point for users.

An expected usage scenario is to perform expensive server side operations, for example, evaluating shortest path between two linked documents.