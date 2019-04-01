# Session: How to customize collection assignment for entities?

Entities are grouped into [collections](../../faq/what-is-a-collection) on the server side. In order to determine the collection name that an entity belongs to
there are special conventions which return the collection name based on the type of an entity: [`FindTypeTagName` and `FindDynamicTagName`](../../configuration/conventions/identifier-generation/global#findtypetagname-and-finddynamictagname).

##Example

By default a collection name is pluralized form of a name of an entity type. For example objects which type is `Category` will belong to `Categories` collection. However if your intention
is to classify them as `ProductGroups` use the following code:

{CODE custom_collection_name@ClientApi\Session\Configuration\CustomizeCollectionAssignmentForEntities.cs /}


## Related articles

- [What is a collection?](../../faq/what-is-a-collection)  
