# Session: How to Customize Collection Assignment for Entities

Entities are grouped on the server side into [collections](../../faq/what-is-a-collection).  
A collection's name is determined by the type of the entities in it.  
To find the name of an entity's collection, use [`findCollectionName`](../../configuration/identifier-generation/global#findcollectionname).  

## Example

By default, a collection name is the pluralized form of an entity's type.  
Entities of type `Category`, for example, will belong to the `Categories` collection.  
To modify this behavior, use `setFindCollectionName`.  

{CODE:php custom_collection_name@ClientApi\Session\Configuration\CustomizeCollectionAssignmentForEntities.php /}

This can become very useful when there is a need to deal with [polymorphic data](../../../indexes/indexing-polymorphic-data).

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize the Identity Property Lookup for Entities](../../../client-api/session/configuration/how-to-customize-identity-property-lookup-for-entities)

### FAQ

- [What is a Collection?](../../../client-api/faq/what-is-a-collection)  
