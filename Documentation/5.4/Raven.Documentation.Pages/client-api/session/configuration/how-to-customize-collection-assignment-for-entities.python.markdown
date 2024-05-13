# Session: How to Customize Collection Assignment for Entities

Entities are grouped into [collections](../../faq/what-is-a-collection) on the server side.  
The name of the collection that an entity belongs to can be retrieved using the `find_collection_name` convention.  

## Example

By default, a collection name is the pluralized form of a name of an entity type.  
E.g., objects of type `Category` will belong to the `Categories` collection.  
If you mean to classify them as `ProductGroups`, however, use the following code:

{CODE:python custom_collection_name@ClientApi\Session\Configuration\CustomizeCollectionAssignmentForEntities.py /}

This can become very useful when there is a need to deal with [polymorphic data](../../../indexes/indexing-polymorphic-data).

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize the Identity Property Lookup for Entities](../../../client-api/session/configuration/how-to-customize-identity-property-lookup-for-entities)

### FAQ

- [What is a Collection?](../../../client-api/faq/what-is-a-collection)  
