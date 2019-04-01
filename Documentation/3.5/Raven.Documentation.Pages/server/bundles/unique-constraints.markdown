# Bundle: Unique Constraints

{PANEL}

This bundle allows the user to impose unique constraints on the objects (useful for properties such as email or social security number).

{PANEL/}

{PANEL:**How it works?**}

The bundle works both on the server (using a PutTrigger and a DeleteTrigger) and on the client (using the `DocumentStoreListener` and providing extension methods on `IDocumentSession`).

When a document is stored in a database, it generates "dummy" documents with following key format: "UniqueConstraints/" + entityName + "/" + propertyName + "/" + propertyValue. This way you don't need indexes (which would have to be **not** stale to check for uniqueness).

### UniqueConstraintsStoreListener

The listener works by using reflection when the document is stored in the database and generating metadata regarding its unique constraints. The reflection result is cached in the `ConcurrentDictionary` to help with performance.

<hr />

### UniqueConstraintsPutTrigger

The put trigger acts whenever it finds a document being inserted with metadata constraints. It checks for existing documents in the constraints. If any existing document is found, it returns a VetoResult.Deny, informing the conflicting fields. This has to be checked on the client-side, using a try block for the OperationVetoedException.

If a document is being updated, the trigger updates the generated constraint document.

<hr />

### UniqueConstraintsDeleteTrigger

The delete trigger acts whenever it finds a document being deleted with constraints metadata . It deletes the referenced constraint documents.

{PANEL/}

{PANEL:Installation}

Drop the `Raven.Bundles.UniqueContraints` assembly in the Plugins directory.

To activate unique constraints server-wide, simply add `Unique Constraints` to `Raven/ActiveBundles` configuration in the global configuration file, or setup a new database with the unique constraints bundle turned on using API or the Studio

{NOTE:Important}
Any bundle which is not added to ActiveBundles list, will not be active, even if the relevant assembly is in the `Plugins` directory.
{NOTE/}

{PANEL/}

## Remarks

{INFO Thanks to Felipe Leusin for contributing the code and Matt Warren for helping in setting up and testing. /}

## Related articles

* [Client API : How to work with unique constraints bundle?](../../client-api/bundles/how-to-work-with-unique-constraints-bundle)
