# Revisions

The revisions feature will create a revision (snapshot) for every document, upon every update received, or when it is deleted.
It is useful when you need to track the history of the documents or when you need a full audit trail.

## Configuration

You can configure the revisions feature using the studio:

![Configuring revisions feature on the database](images/configure-revisions.png)

By default, the reivions feature will track history for all documents and never purge old revisions. 
You can configure this for all collections (using the default configuration) and you can have a configuration for a specific collection.

Configuration optoins:

| Configuration option | Description |
| **Purge on delete** | Configure whether to delete the reivions upon document delete, or create a delete marker instead. |
| **Limit # of revisions** | Configure how much revisions to keep. Default: unlimited. |
| **Limit # of revisions by age** | Configure a minimum retention time before the revisions can be expired. Default: None. |
| **Disable** | If true, disalbe the reivions feature for this configuration (default or specific collection). Defalut: false. |

You can also configure the revisions feature using the client:

{CODE configuration@Server\Revisions.cs /}

It is possible to have a default configuration telling the revisions feature to revision all documents (`Disabled=false` on the defulat configuration) and only keep up to 5 revisions, purging older ones (`MinimumRevisionsToKeep=5`).
Than we override the behavior of the reivions feature by specifing a configuration specifically to a collection. For example, let's say that we don't want to revision users.

Conversely, we can disable the default configuration (`Disalbed = true`) but configure a collection to enable reivions.

## How it works

With the Revisions feature enabled, let us execute this code:

{CODE store@Server\Revisions.cs /}

If we inspect the document we will see that the following revision were created:

![Figure 1: Revisions](images\revisoins1.png)

This is a revision of the document (you can navigate to the document by clicking on `See the current document`) which is stored on the revisions storage.
Now, let's modify the original document. This would create another revision:

![Figure 2: Revisions, Modified](images\revisoins2.png)

As you can see, we have full audit record of all the changes that were made to the document.

You can access the revisions of a specific document by the document's ID ("users/1").
Or you can access a specific revision by its change vector.

{CODE get_revisions@Server\Revisions.cs /}

Now, let's delete the document. 
The document would be removed the document but a revision will be created, so you aren't going to lose the audit trail if the document is deleted. 
The new revision can be accessed using the studio under the Revisions Bin:

![Figure 3: Revisions, Deleted](images\revisoins3.png)

Clicking on the revision we can also navigate to the other revisions of this document:

![Figure 4: Revisions, Deleted - other revisions](images\revisoins4.png)

As you can see, the revisions feature attempts to make things as simple as possible, and once it is enabled, you'll automatically get the appropriate audit trail.
