# Expiration

The expiration feature serves a very simple purpose: it deletes the documents whose time has passed. 
Usage scenarios for the Expiration feature include storing user sessions in RavenDB or using RavenDB as a cache.

## Usage
You can set the expiration date for a document using the following code:

{CODE expiration1@Server\Expiration.cs /}

As you can see, all we need to do is to set the `@expires` property in the metadata for the appropriate date, and, at the specified time, the document will be automatically deleted.

{NOTE The date must be UTC, not local time. /}

## Configuration
By default the expriation is turned off. You can turned it on using the studio. 
The default delete frequency is 60 seconds, which is customizable.

![Configuring expiration feature on the server](images/expiration.png)

## Eventual consistency
Once the document is expired it can take up to the delete frequency interval (60 seconds, by default) until the expired documents would acutally be deleted. 
We do not filter expired documents on load/query/indexing time, so need to a aware that the document might be still there after it expires up to the delete frequency interval timefarme.

## More details
Internally we track each document with `@expires` property in the metadata, even if the expiration feature it turned off. 
This way once the expirtaion feature is turned on we can delete all expired documents easily.

{NOTE Metadata properties starting with `@` are internal for RavenDB use, you should not use `@expires` property in metadata for other purpose than the built in expiration feature. /}

You can turn on and off the revisions feature whlie the database is already live with data.
