#HTTP API - Single Document Operations

##GET

Perform a GET request to read a JSON document from its URL: 

{CODE-START:plain /}
    > curl -X GET http://localhost:8080/docs/bobs_address
{CODE-END /}

Assuming there is a document with an id of "bobs_address", RavenDB will respond with the contents of that document and an HTTP 200 OK response code: 

{CODE-START:json /}
    HTTP/1.1 200 OK 
 
    {  
      "FirstName": "Bob",  
      "LastName": "Smith",  
      "Address": "5 Elm St."  
    }
{CODE-END /}

If the URL specified does not point to a valid document, RavenDB follows HTTP conventions and responds with: 

    HTTP/1.1 404 Not Found

##PUT
Perform a PUT request to /docs/{document_id} to create the specified document with the given document id: 

{CODE-START:json /}
    > curl -X PUT http://localhost:8080/docs/bobs_address -d "{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }"
{CODE-END /}

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code: 

{CODE-START:json /}
    HTTP/1.1 201 Created
  
    {"Key":"bobs_address","ETag":"179048f3-4c71-11df-8ec2-001fd08ec235"}
{CODE-END /}

It is important to note that a PUT in RavenDB will always create the specified document at the request URL, if necessary overwriting what was there before.

A PUT request to /docs without specifying the document id in the URL is an invalid request and RavenDB will return a HTTP 400 Bad Request response code. 

##POST
Perform a POST request to the /docs area to create the specified document and allow RavenDB to assign a unique id to it: 

{CODE-START:json /}
    > curl -X POST http://localhost:8080/docs -d "{ FirstName: 'Bob', LastName: 'Smith', Address: '5 Elm St' }"
{CODE-END /}

For a successful request, RavenDB will respond with the id it generated and an HTTP 201 Created response code: 

{CODE-START:json /}
    HTTP/1.1 201 Created  

    {"Key":"5ecec911-4c71-11df-8ec2-001fd08ec235","ETag":"5ecec912-4c71-11df-8ec2-001fd08ec235"}
{CODE-END /}

It is important to note that a repeated POST request for the same document will create that document in a new place, with a new id each time.

A POST to a document URL is an invalid request and RavenDB will return a HTTP 400 Bad Request response code.

##PATCH

Any single document within RavenDB can be updated without replacing the entire document with a PUT. You can do it either by using [Client API](../client-api/partial-document-updates) or by creating a PATCH request. The PATCH command allows Raven to implement field level concurrency.

All PATCH requests are made to the URL of the document, and follow the general format of: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/{document_id} -d 
      "[{ Type: |patch_operation|, Name: |document_property|, Position: |position in array|,   
          Value: |new_value|, PrevVal: |old property value| }, {... more patch operations ...}]"
{CODE-END /}

RavenDB will return a short JSON acknowledgment and HTTP 200 OK response code for a successful patch:   

{CODE-START:json /}
    HTTP 1.1/200 OK  

    { "patched":true }
{CODE-END /}

Note that the PATCH command accepts an array of commands, so you can issue multiple modifications for the same document. The command keys are case sensitive, so they need to be Type, Name, Position, Value, PrevVal and Nested, with the correct camel casing.
 
For the purposes of these examples, suppose we start with this document: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1 -X PUT -d "{ title: 'A Blog Post', body: 'html markup here', 
      comments: [ {author: 'ayende', text: 'good post'} ] }"
{CODE-END /}

###Patch Operations
Raven supports the following patch operations:

* Set - Set a property to a new value (optionally creating the property).
* Inc - Increment a property value by a given value (optionally creating the property).
* Unset - Remove a property.
* Add - Add a value to an existing array.
* Insert - Insert a value to an existing array at the specified position.
* Remove - Remove a value from an existing array at a specified position.
* Modify - Apply nested patch operation to an existing property value.
* Copy - Copy a property value to a new property (creating or overwriting the property).
* Rename - Rename a property (overwriting the new property if it is needed).

The name property in the patch format refer to the target of the patch operation and can accept both a simple property name or a JPath. The following are all valid values for the name property:

* blog_id
* comments[0].author
* author.name

**Add a Property**  
Perform a PATCH request to the document URL with a type of "Set" to add a property: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Set', Name: 'blog_id', Value: 1}]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1
    {
      "title": "A Blog Post",
      "body": "html markup here",
      "comments": [
        {
          "author": "ayende",
          "text": "good post"
        }
      ],
      "blog_id": 1
    }
{CODE-END /}

**Increment a Property**  
Perform a PATCH request to the document URL with a type of "Inc" to increment a property: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Inc', Name: 'blog_id', Value: 1}]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ],  
      "blog_id": 2  
    }
{CODE-END /}

You can also use negative values for decrementing:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Inc', Name: 'blog_id', Value: -1}]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ],  
      "blog_id": 1  
    }
{CODE-END /}

**Update a Property**  
Perform a PATCH request to the document URL with a type of "Set" to update an existing property: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Set', Name: 'title', Value: 'A Better Blog Post'}]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ],  
      "blog_id": 1  
    }
{CODE-END /}

**Set a Property to NULL**  
Perform a PATCH request to the document URL with a type of "Set" and the reserved value of null to set property to be null: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Set', Name: 'blog_id', Value: null }]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ],  
      "blog_id": null  
    }
{CODE-END /}

**Copy a property**  
Perform a PATCH request to the document URL with a type of "Copy" with the value set to the new property name: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Copy', Name: 'title', Value: 'title_copy' }]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "title_copy": "A Better Blog Post",   
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ]  
    }
{CODE-END /}

**Rename a property**  
Perform a PATCH request to the document URL with a type of "Rename" with the value set to the new property name: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'Rename', Name: 'title', Value: 'title_copy' }]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title_copy": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ]  
    }
{CODE-END /}

**Removing a Property**  
Perform a PATCH request to the document URL with a type of "unset" to remove a property from a document: 

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'unset', Name: 'blog_id'}]"
{CODE-END /}

A subsequent GET of this document would show the patched result: 

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post"  
        }  
      ]  
    }
{CODE-END /}

If the property specified does not exist or has already been removed from the document, RavenDB will just respond with a 200 OK and do nothing.

**Operating on Arrays - Adding**  
Perform a PATCH request to the document URL with a type of "add", to add a value to an array:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1   
      -d "[{ Type: 'add', Name: 'comments', Value: {'author': 'oren', 'text': 'another good post', 'author_id': 2 } }]"
{CODE-END /}

A subsequent GET of this document would show the patched result:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post",  
          "author_id": 1  
        },  
        {  
          "author": "oren",  
          "text": "another good post",  
          "author_id": 2  
        }  
      ]
    }
{CODE-END /}

**Operating on Arrays - Inserting**  
Perform a PATCH request to the document URL with a type of "insert", to insert a value to an array at the specified position:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1   
      -d "[{ Type: 'insert', Name: 'comments', Position: 1, Value: {'author': 'alon', 'text': 'first comment', 'author_id': 3 } }]"
{CODE-END /}

A subsequent GET of this document would show the patched result:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post",  
          "author_id": 1  
        },  
        {  
          "author": "alon",  
          "text": "first comment",  
          "author_id": 3  
        },  
        {  
          "author": "oren",  
          "text": "another good post",  
          "author_id": 2  
        }  
      ]  
    }
{CODE-END /}

**Operating on Arrays - Removing**  
Perform a PATCH request to the document URL with a type of "remove", to remove a value from an array by position:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'remove', Name: 'comments', Position: 0}]"
{CODE-END /}

An error will be returned if the position is out of bounds for the array.
The same can also be done by Value:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'remove', Name: 'comments', Value: {'author': 'oren', 'text': 'another good post', 'author_id': 2 } }]"
{CODE-END /}

In this case, if the value doesn't exist in the array no error will be thrown.
A subsequent GET of this document would show the patched result:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "alon",  
          "text": "first comment",  
          "author_id": 3  
        },  
        {  
          "author": "oren",  
          "text": "another good post",  
          "author_id": 2  
        }  
      ] 
    }
{CODE-END /}

The same thing could have been done by Value:

**Operating on Nested Properties**  
Perform a PATCH request to the document URL with a type of "modify", and an optional "position" to manipulate a nested property, in this case, the nested property is the element in the array:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1   
      -d "[{ Type: 'modify', Name: 'comments', Position: 0, Nested: [ { Type: 'set', Name: 'author_id', Value: 1 } ]}]"
{CODE-END /}

A subsequent GET of this document would show the patched result:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "A Better Blog Post",  
      "body": "html markup here",  
      "comments": [  
        {  
          "author": "ayende",  
          "text": "good post",  
          "author_id": 1  
        }  
      ]  
    }
{CODE-END /}

**Several operations in a single request**  
The PATCH command accepts an array of commands to work with, so we can issue several commands in a single request:

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1   
      -d "[{ Type: 'set', Name: 'comments', Value: []}, { Type: 'unset', Name: 'body'},   
           { Type: 'set', Name: 'title', Value: 'Best Blog Post'}]"
{CODE-END /}

A subsequent GET of this document would show the patched result:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "Best Blog Post",  
      "comments": []  
    }
{CODE-END /}

**Document level concurrency**  
The PATCH command support ETag based concurrency, so the following command will work the first time, but fail the second time, when the etag fails to match

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'set', Name: 'blog_id', Value: 1}]" --header "If-Match:0514e6de-4c76-11df-8ec2-001fd08ec235"  
    {"patched":true}  
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'set', Name: 'blog_id', Value: 2}]" --header "If-Match:0514e6de-4c76-11df-8ec2-001fd08ec235"  
    HTTP 1.1/409 Conflict  

    {"url":"/docs/post_1","actualETag":"a4c5d756-4c76-11df-8ec2-001fd08ec235","expectedETag":"0514e6de-4c76-11df-8ec2-001fd08ec235","error":"Could not patch document 'post_1' because non current etag was used"}
{CODE-END /}

A subsequent GET of this document would show that the first patch command succeeded, and that the second one had no results:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "Best Blog Post",  
      "comments": [],  
      "blog_id": 1  
    }
{CODE-END /}

**Field level concurrency**  
Beyond document level concurrency, the PATCH command support field level concurrency, applying changes only if the field is set to a particular value. This is done by omitting the If-Modified header and using the prevVal field in the patch command, like so.

{CODE-START:json /}
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'set', Name: 'blog_id', Value: 2, PrevVal: 1}]"  
    {"patched":true}  
    > curl -X PATCH http://localhost:8080/docs/post_1 -d "[{ Type: 'set', Name: 'blog_id', Value: 3, PrevVal: 1}]"  
    HTTP 1.1/409 Conflict 
 
    {"url":"/docs/post_1","actualETag":"00000000-0000-0000-0000-000000000000","expectedETag":"00000000-0000-0000-0000-000000000000","error":"Exception of type 'Raven.Database.Exceptions.ConcurrencyException' was thrown."}
{CODE-END /}

A subsequent GET of this document would show that the first operation worked, but the second had no affect:

{CODE-START:json /}
    > curl http://localhost:8080/docs/post_1  
    {  
      "title": "Best Blog Post",  
      "comments": [],  
      "blog_id": 2  
    }
{CODE-END /}

Using this approach, you can make an atomic change to the a particular part of a document safely.

## DELETE
Perform a DELETE request to delete the JSON document specified by the URL: 

{CODE-START:plain /}
    > curl -X DELETE http://localhost:8080/docs/bobs_address
{CODE-END /}

For a successful delete, RavenDB will respond with an HTTP response code 204 No Content: 

    "HTTP/1.1 204 No Content"

The only way a delete can fail is if [the etag doesn't match](http://ravendb.net/docs/http-api/http-api-concurrency?version=2.0). If the document doesn't exist, a delete will still respond with a successful status code. 

**Hard vs. Soft Deletes**  
Deleting a document through the HTTP API is not reversible. In database terms, it is a "hard" delete.

An alternative approach is to mark a document with a deleted flag and then ignore documents like this in your business logic.

This approach, a "soft" delete, preserves the data intact in RavenDB and can be useful for auditing or undoing a user's actions.

The right approach for you will depend on the problem space that you are modeling. 