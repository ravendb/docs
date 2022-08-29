# What are Attachments

---

{NOTE: }

* In RavenDB, attachments are binary streams that can be bound to an existing document. 
  Each attachment has a name, and you can specify the content type (`image/png` or `application/pdf` for example).

* Each document can have multiple attachments of various types associated with it.

* The attachments' names (e.g. video.mp4), content type (e.g. image/png), and other info such as DateTime, hash, and size 
  [can be stored in the document metadata](../../document-extensions/attachments/what-are-attachments#example-ii---including-attachment-metadata-to-be-able-to-query-attachments).  
   * Referencing attachment info in the metadata allows you to [index and then query attachments](../../document-extensions/attachments/indexing) 
     like you would query documents by specifying the document ID and the attachment name.  

* Adding or removing an attachment changes the document metadata, thus triggering any tasks caused by document changes such as indexing, ETL, Revisions, etc.

* In this page:
   * [Example I - Document with an associated album of images](../../document-extensions/attachments/what-are-attachments#example-i---document-with-an-associated-album-of-images)
   * [Example II -Including attachment metadata to be able to query attachments](../../document-extensions/attachments/what-are-attachments#example-ii---including-attachment-metadata-to-be-able-to-query-attachments)
   * [Transaction Support](../../document-extensions/attachments/what-are-attachments#transaction-support)
   * [Revisions and Attachments](../../document-extensions/attachments/what-are-attachments#revisions-and-attachments)

{NOTE/}

{PANEL: Example I - Document with an associated album of images}

To store an album of pictures in RavenDB, you can create the following "albums/1" document:

{CODE-BLOCK:json}
{
    "UserId": "users/1",
    "Name": "Holidays",
    "Description": "Holidays travel pictures of the family",
    "Tags": ["Holidays Travel", "All Family"],
    "@metadata": {
        "@collection": "Albums"
    }
}
{CODE-BLOCK/}

This document can have the following attachments:

| Name | Content type |
| - | - |
| `001.jpg` | `image/jpeg` |
| `002.jpg` | `image/jpeg` |
| `003.jpg` | `image/jpeg` |
| `004.mp4` | `video/mp4`  |

{PANEL/}

{PANEL: Example II - Including attachment metadata to be able to query attachments }

You can store a `users/1` document and attach a profile picture to it.
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

Note that this document has an HasAttachments flag and an @attachments array with the attachment's info.

You can see the attachment's name, content type, hash, and size.

{NOTE We would store the attachment streams by the hash, so if many attachments have the same hash, their streams would be stored just once. /}

{PANEL/}

{PANEL: Transaction Support}

In RavenDB, attachments and documents are stored as [ACID transactions](../../client-api/session/what-is-a-session-and-how-does-it-work#batching): 
You either get all of them saved to the server or none.

{PANEL/}

{PANEL: Revisions and Attachments}

When the revisions feature is turned on in your database, each attachment addition to a document (or deletion from a document) will create a new revision of the document, 
as there will be a change to the document's metadata, as shown in example #2. 

{PANEL/}

## Related articles

### Attachments

- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
- [Copying, Moving, & Renaming](../../document-extensions/attachments/copying-moving-renaming)
- [Indexing](../../document-extensions/attachments/indexing)
- [Bulk Insert](../../document-extensions/attachments/bulk-insert)

### Studio

- [Studio - Attachments](../../studio/database/document-extensions/attachments)

---

### Code Walkthrough

- [Store Attachment](https://demo.ravendb.net/demos/csharp/attachments/store-attachment)
- [Load Attachment](https://demo.ravendb.net/demos/csharp/attachments/load-attachment)
- [Index Attachment Details](https://demo.ravendb.net/demos/csharp/attachments/index-attachment-details)
