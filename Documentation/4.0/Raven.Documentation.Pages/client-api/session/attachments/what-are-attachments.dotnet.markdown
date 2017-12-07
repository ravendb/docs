# What are attachments?

In RavenDB attachments are binary streams which can be bound to an existing document. 
Each attachment has a name and you can specify the content type ("image/png" or "application/pdf" for example).
A document can have any number of attachments.

Each attachment is bound to an existing document and in order to get an document you'll need to specify the document ID and the attachment name. 
What's great in this approach that you can specify the attachment's metadata in the document itself, and this document can be queried as any other document.

## Example 1:

In order to store album of pictures in RavenDB you can create the following "albums/1" document:

{
    "UserId": "users/1",
    "Name": "Holidays",
    "Description": "Holidays travel pictures of the all family",
    "Tags": ["Holidays Travel", "All Family"],
    "@metadata": {
        "@collection": "Albums"
    }
}

And this document can have the following attachments:

Name: 001.jpg, Content-Type: image/jpeg
Name: 002.jpg, Content-Type: image/jpeg
Name: 003.jpg, Content-Type: image/jpeg
Name: 004.mp4, Content-Type: video/mp4

## Example 2:

You can store an "users/1" document and attach to it a profile picture.
When requesting the document from the server the results would be:

{CODE-BLOCK:json}
{
  "Name": "Hibernating Rhinos",
  "@metadata": {
    "@attachments": [
      {
        "ContentType": "image/png",
        "Hash": "iFg0o6D38pUcWGVlP71ddDp8SCcoEal47kG3LtWx0+Y=",
        "Name": "profile.png",
        "Size": 33241
      }
    ],
    "@collection": "Users",
    "@change-vector": "A:1061-D11EJRPTVEGKpMaH2BUl9Q",
    "@flags": "HasAttachments",
    "@id": "users/1",
    "@last-modified": "2017-12-05T12:36:24.0504021Z"
  }
}
{CODE-BLOCK/}

You can note from this that the document has a HasAttachments flag and a @attachments array with the attachments info.
You can see the attachments name, content type, hash and size.

{NOTE Note that we would store the attachment streams by the hash, so if many attachments has the same hash their streams would be store just once. /}

## Transaction support

In RavenDB attachment and documents can be stored in one transaction, a real one, which means you either get all of them saved to disk or none.

## Revisions and attachments

When revisions feature is turned on in your database, each attachment addition to a document (or deletion from a document) would create a new revision of the document, 
as there would be a change to the document's metadata, as shown in example #2. 

## Client API

The following client API are related to attachments:

- [Get attachment names using a document ID](../../../client-api/commands/attachments/get) 
- [Get attachment using a document ID and attachment name](../../../client-api/session/attachments/get)
- [Checking if attachment exists](../../../client-api/commands/attachments/get) 
- [Get attachment of a revision document using document ID, attachment name and the revision change vector](../../../client-api/commands/attachments/get) 
- [Store attachment](../../../client-api/session/attachments/put)
- [Delete attachment](../../../client-api/session/attachments/delete)
