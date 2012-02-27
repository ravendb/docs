# UniqueConstraints Bundle

## Premise

This bundle aims to allow the user to implement unique constraints in the objects (usefull for properties like email or social security number).

## How it works

The bundle works both in the server (using a PutTrigger and a DeleteTrigger) and in the client (using a DocumentStoreListener).

When a document is stored in the database it generates "dummy" documents with the fields as "UniqueConstraints/" + entityName + "/" + propertyName + "/" + propertyValue. This way you don't require indexes (which would need to be stale to check for uniqueness).

### UniqueConstraintsStoreListener

The listener works by using reflection when the document is stored on the database and generating metadata regarding it's unique constraints. The reflection result is cached in a ConcurrentDictionary to help with performance.

### UniqueConstraintsPutTrigger

The put trigger acts whenever it finds a document being inserted with constraints metadata it checks for existing documents in for the constraints and if any existing document is found it returns a VetoResult.Deny informing the confliction fields. This would need to be checked on the client-side using a try block for the OperationVetoedException.

If a document is being updated the trigger updates the generated constraint document.

### UniqueConstraintsDeleteTrigger

The delete trigger acts whener it find a document being delete with constraints metadata and deletes the referenced constraint documents.

### Thanks to Felipe Leusin (for contirubting the code) and Matt Warren (for helping in setting up and testing).


## Usage

### Server side

Drop the Raven.BUndles.UniqueContraints assembly in the Plugins directory.

### Client side

When creating the DocumentStore, you'll need to register the UniqueConstraintsStoreListener in the store, like this:

    store.RegisterListener(new UniqueConstraintsStoreListener());