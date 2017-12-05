# What are attachments?

In RavenDB v4.0 attachments are binary stream which can be attached to an exiting document. 
Each attachment has a name and you can specify the content type ("image/png" or "application/pdf" for example).

Each attachment is attached to an exiting document and in order to get an document you'll need to specify the document ID and the attachment name. 
What's great in this approach that you can specify the attachment's metadata in the document itself, and this document can be queries as any other document.

## Example 1:

In order to store album of pictures in RavenDB 4.0 you can create the folloinwg "albums/1" document:

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

You can store an "users/1" document and attach to it a profile picutre.
When requesting the document from the server the results would be:

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

You can note from this that the document has a HasAttachments flag and a @attachments array with the attachments info.
You can see the attachments name, content type, hash and size.

Note that we would store the attachment streams by the hash, so if many attachments has the same hash their streams would be store just once.

## Transaction support

In RavenDB v4.0 attachment and documents can be stored in one transaction, a real one, which means you either get all of them saved to disk or none.

## Reivions and attachments

When revisions is on, each attachment addition to a document (or deletion from a document) would create a new revision to the document, 
as there would be a change to the document's metadata, as shown in example #2. 

## Client API

The follwing client API are related to attachments:

- Get attachment names using a document ID
- Get attachment using a document ID and attachment name.
- Checking if attachment exists.
- Get attachment of a revision document using document ID, attachment name and the revision change vector.
- Store attachment.
- Delete attachment.
