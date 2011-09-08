# Versioning bundle

The versioning bundle will create snapshots for every document upon every update made to it, or when it is deleted. It is useful when you need to track history of documents, or when you need a full audit trail.

As with all bundles, installation is as easy as just dropping Raven.Bundles.Versioning.dll into the plugins directory.

## Configuration

By default, the Versioning bundle will track history for all documents and never purge old revisions. This is easily configurable by creating or changing configuration documents like this:

{CODE versioning1@Server\Bundles.cs /}

In this example is a global configuration document, telling the versioning bundle to version all documents (`Exclude=false`), and to only keep up to 5 revisions - purging older ones (`MaxRevisions=5`).

It is possible to override the behavior of the Versioning Bundle for a set of documents by creating a document specifically for an entity name. For example, let us say that we don't want to version users, we can tell the Versioning Bundle to avoid doing so adding a configuration document that is specific to the `Users` collection:

{CODE versioning2@Server\Bundles.cs /}

The naming convention used is `"Raven/Versioning/" + entityName`, where the entity name is the value of the Raven-Entity-Name property on the document.

Conversely, we can also set the default configuration to not version (`Exclude = false`), and configure versioning for each set of documents individually, by adding a document for each collection that we do want to version.

## How it works

// TODO: http://ravendb.net/bundles/versioning

## Attribution

## Querying for revisions

## Client integration

// TODO: GetRevisionsFor<>