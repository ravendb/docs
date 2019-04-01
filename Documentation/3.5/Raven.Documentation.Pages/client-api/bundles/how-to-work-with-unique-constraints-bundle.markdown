#Unique Constraints: How to work with unique constraints bundle?

In order to use this bundle first activate it on the server. The description can be found in [server section](../../server/bundles/unique-constraints).

To use the Unique Constraint features on the client side you need to reference `Raven.Client.UniqueConstraints` assembly.

{CODE using@ClientApi\Bundles\HowToWorkWithUniqueConstraintsBundle.cs /}

When creating the DocumentStore, you'll need to register the `UniqueConstraintsStoreListener` in the store, as follows:

{CODE unique_constraints_1@ClientApi\Bundles\HowToWorkWithUniqueConstraintsBundle.cs /}

To define a unique constraint on a property use the `UniqueConstraint` attribute as shown below:

{CODE unique_constraints_4@ClientApi\Bundles\HowToWorkWithUniqueConstraintsBundle.cs /}

##Extension methods

The bundle provides two extension methods for `IDocumentSession`.

#### LoadByUniqueConstraint

Allows loading a document by its UniqueConstraint, returning null if the document doesn't exists.

{CODE unique_constraints_2@ClientApi\Bundles\HowToWorkWithUniqueConstraintsBundle.cs /}

#### CheckForUniqueConstraints

Checks a document to see if its constraints are available on the server. It returns a `UniqueConstraintCheckResult` containing the loaded docs and properties they are responsible for.

{CODE unique_constraints_3@ClientApi\Bundles\HowToWorkWithUniqueConstraintsBundle.cs /}

## Related articles

* [Bundle : Unique Constraints](../../server/bundles/unique-constraints)
