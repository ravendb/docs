# UniqueConstraints Bundle

## Premise

This bundle aims to allow the user to implement unique constraints in the objects (useful for properties like email or social security number).

## How it works

The bundle works both in the server (using a PutTrigger and a DeleteTrigger) and in the client (using a `DocumentStoreListener` and providing extension methods on `IDocumentSession`).

When a document is stored in the database it generates "dummy" documents with the fields as "UniqueConstraints/" + entityName + "/" + propertyName + "/" + propertyValue. This way you don't require indexes (which would need to be stale to check for uniqueness).

### UniqueConstraintsStoreListener

The listener works by using reflection when the document is stored on the database and generating metadata regarding it's unique constraints. The reflection result is cached in a `ConcurrentDictionary` to help with performance.

### UniqueConstraintSessionExtensions

The bundle provides two extension methods for `IDocumentSession`.

#### LoadByUniqueConstraint

Allows to load a document by it's UniqueConstraint, returning null if the document doesn't exists.

#### CheckForUniqueConstraints

Checks a document to see if it's constraints are available in the server. It returns a `UniqueConstraintCheckResult` containing the loaded docs and what property they are responsible for.


### UniqueConstraintsPutTrigger

The put trigger acts whenever it finds a document being inserted with constraints metadata. It checks for existing documents in the constraints. If any existing document is found, it returns a VetoResult.Deny informing the conflicting fields. This would need to be checked on the client-side using a try block for the OperationVetoedException.

If a document is being updated the trigger updates the generated constraint document.

### UniqueConstraintsDeleteTrigger

The delete trigger acts whenever it find a document being delete with constraints metadata and deletes the referenced constraint documents.

### Thanks to Felipe Leusin (for contributing the code) and Matt Warren (for helping in setting up and testing).


## Usage

### Server side

Drop the `Raven.Bundles.UniqueContraints` assembly in the Plugins directory.

### Client side

To use Unique Constraint features on Client side you need to reference `Raven.Client.UniqueConstraints` assembly.

{CODE using@Server\Bundles\UniqueConstraints.cs /}

When creating the DocumentStore, you'll need to register the `UniqueConstraintsStoreListener` in the store, like this:

{CODE unique_constraints_1@Server\Bundles\UniqueConstraints.cs /}

To define unique constraint on a property use `UniqueConstraint` attribute as shown below

{CODE unique_constraints_4@Server\Bundles\UniqueConstraints.cs /}

### Extension methods

To check if a value is available for use:

{CODE unique_constraints_2@Server\Bundles\UniqueConstraints.cs /}

To check document against the database use:

{CODE unique_constraints_3@Server\Bundles\UniqueConstraints.cs /}
