# Bundle: Versioning

The versioning bundle will create snapshots for every document, upon every update received, or when it is deleted. It is useful when you need to track the history of the documents or when you need a full audit trail.

## Installation

To activate versioning server-wide, simply add `Versioning` to `Raven/ActiveBundles` configuration in the global configuration file, or setup a new database with the versioning bundle turned on using API or the Studio.

Learn how to create a database with versioning enabled using the Studio [here](../../studio/overview/settings/versioning).

## Configuration

By default, the Versioning bundle will track history for all documents and never purge old revisions. This may be easily configurable through creating or changing configuration documents as follows:

{CODE versioning1@Server\Bundles\Versioning.cs /}

It is an example of a global configuration document, telling the versioning bundle to version all documents (`Exclude=false`) and only keep up to 5 revisions, purging older ones (`MaxRevisions=5`).

It is possible to override the behavior of the Versioning Bundle for a set of documents by creating a document specifically for an entity name. For example, let's say that we don't want to version users. In this case, we can tell the Versioning Bundle to avoid doing so by adding a configuration document that is specific for the `Users` collection:

{CODE versioning2@Server\Bundles\Versioning.cs /}

The naming convention used is `"Raven/Versioning/" + entityName`, where the entity name is the value of the Raven-Entity-Name property on the document.

Conversely, we can also set the default configuration to not version (`Exclude = true`) and configure versioning for each set of the documents individually, by adding a document for each collection that we do want to version.

## How it works

With the Versioning Bundle installed, let us execute this code:

{CODE versioning3@Server\Bundles\Versioning.cs /}

If we inspect the server, we will see that the following documents were created:

![Figure 1: Versioned Documents](images\versioned_docs.png)

The first document is the actual document that we just saved, yet we can see that it has more additional metadata properties than we are used to:

* Raven-Document-Revision - The document revision (starts at 1).
* Raven-Document-Revision-Status - Whatever it is the Current revision or a Historical snapshot.

Now, let's modify the original document. This will give us:

![Figure 2: Versioned Documents, Modified](images\versioned_docs_2.png)

As you can see, we have full audit record of all the changes that were made to the document.

You can access each of the revisions simply using its id "users/1/revisions1". However, modifications / deletions of revisions are not allowed, and will result in an error if attempted.

Now, let's delete the original document:

![Figure 3: Versioned Documents, Deleted](images\versioned_docs_3.png)

That removed the current document, but the snapshots will remain in place, so you aren't going to lose the audit trail if the document is deleted. 

As you can see, the Versioning Bundle attempts to make things as simple as possible, and once it is installed, you'll automatically get the appropriate audit trail. Working with revisions is identical to working with standard documents (except that you can't modify / delete them). Revision documents cannnot be indexed like standard documents.

Limitations

* Versioning will fail on documents with keys larger than 230 characters.
* Versioning will not attempt to version internal Raven document (documents whose keys start with "Raven/")

## Client integration

The Versioning bundle also has a client side part, which you can access by adding a reference to Raven.Client.Versioning assembly.

Then, you can access past revisions of a document using the following code:

{CODE versioning_4@Server\Bundles\Versioning.cs /}
