# Session: How to Customize the Identity Property Lookup for Entities

* When a client handles an entity, it must know which property is the entity's identifier.  
  By default, the client always looks for the `Id` property (case-sensitive).  

* This behavior can be changed by overwriting the `FindIdentityProperty` convention.  
  To do so, use the `setFindIdentityProperty` method.  

## Syntax

{CODE:php identity_1@ClientApi\Session\Configuration\FindIdentityProperty.php /}

## Example

The simplest example would be to check if the property name is equal to 'Identifier'.

{CODE:php identity_2@ClientApi\Session\Configuration\FindIdentityProperty.php /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize Collection Assignment for Entities](../../../client-api/session/configuration/how-to-customize-collection-assignment-for-entities)

### Knowledge Base

- [Document Identifier Generation](../../../server/kb/document-identifier-generation)
