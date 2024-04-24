# Session: How to Customize Collection Assignment for Entities

Entities are grouped into [collections](../../faq/what-is-a-collection) on the server side. In order to determine the collection name that an entity belongs to
there are special conventions which return the collection name based on the type of an entity: [`FindCollectionName` and `FindCollectionNameForDynamic`](../../configuration/identifier-generation/global#FindCollectionName-and-FindCollectionNameForDynamic).

## Example

By default a collection name is pluralized form of a name of an entity type. For example objects of type `Category` will belong to `Categories` collection. However if your intention
is to classify them as `ProductGroups` use the following code:

{CODE custom_collection_name@ClientApi\Session\Configuration\CustomizeCollectionAssignmentForEntities.cs /}

This can become very useful when there is a need to deal with [polymorphic data](../../../indexes/indexing-polymorphic-data).

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize the Identity Property Lookup for Entities](../../../client-api/session/configuration/how-to-customize-identity-property-lookup-for-entities)

### FAQ

- [What is a Collection?](../../../client-api/faq/what-is-a-collection)  
