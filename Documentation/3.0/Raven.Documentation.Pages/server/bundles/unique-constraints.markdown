# Bundle : Unique Constraints

{PANEL}

This bundle allows the user to impose unique constraints on the objects (useful for properties such as email or social security number).

{PANEL/}

{PANEL:**How it works?**}

The bundle works both on the server (using a PutTrigger and a DeleteTrigger) and on the client (using the `DocumentStoreListener` and providing extension methods on `IDocumentSession`).

When a document is stored in a database, it generates "dummy" documents with following key format: "UniqueConstraints/" + entityName + "/" + propertyName + "/" + propertyValue. This way you don't need indexes (which would have to be **not** stale to check for uniqueness).

### UniqueConstraintsStoreListener

The listener works by using reflection when the document is stored in the database and generating metadata regarding its unique constraints. The reflection result is cached in the `ConcurrentDictionary` to help with performance.

<hr />

### UniqueConstraintSessionExtensions

The bundle provides two extension methods for `IDocumentSession`.

#### LoadByUniqueConstraint

Allows loading a document by its UniqueConstraint, returning null if the document doesn't exists.

#### CheckForUniqueConstraints

Checks a document to see if its constraints are available on the server. It returns a `UniqueConstraintCheckResult` containing the loaded docs and properties they are responsible for.

<hr />

### UniqueConstraintsPutTrigger

The put trigger acts whenever it finds a document being inserted with metadata constraints. It checks for existing documents in the constraints. If any existing document is found, it returns a VetoResult.Deny, informing the conflicting fields. This has to be checked on the client-side, using a try block for the OperationVetoedException.

If a document is being updated, the trigger updates the generated constraint document.

<hr />

### UniqueConstraintsDeleteTrigger

The delete trigger acts whenever it finds a document being deleted with constraints metadata . It deletes the referenced constraint documents.

{PANEL/}

{PANEL:Usage}

### Server side

Drop the `Raven.Bundles.UniqueContraints` assembly in the Plugins directory.

<hr />

### Client side

To use the Unique Constraint features on Client side you need to reference `Raven.Client.UniqueConstraints` assembly.

{CODE using@Server\Bundles\UniqueConstraints.cs /}

When creating the DocumentStore, you'll need to register the `UniqueConstraintsStoreListener` in the store, as follows:

{CODE unique_constraints_1@Server\Bundles\UniqueConstraints.cs /}

To define a unique constraint on a property use the `UniqueConstraint` attribute as shown below:

{CODE unique_constraints_4@Server\Bundles\UniqueConstraints.cs /}

#### Extension methods

To check if a value is available for use:

{CODE unique_constraints_2@Server\Bundles\UniqueConstraints.cs /}

To check a document against the database use:

{CODE unique_constraints_3@Server\Bundles\UniqueConstraints.cs /}

{PANEL/}

## Remarks

{INFO Thanks to Felipe Leusin for contributing the code and Matt Warren for helping in setting up and testing. /}

## Related articles

TODO