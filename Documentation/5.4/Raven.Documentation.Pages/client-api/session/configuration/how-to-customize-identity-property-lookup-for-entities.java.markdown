# Session: How to Customize the Identity Property Lookup for Entities

The client must know which property of your entity is considered as an identity. By default, it always looks for the `id` field (case-sensitive). This behavior can be changed by overwriting one of our conventions called `findIdentityProperty`.

## Syntax

{CODE:java identity_1@ClientApi\Session\Configuration\FindIdentityProperty.java /}

`PropertyDescriptor` will represent the property of a stored entity, and return value (bool) will indicate if given member is an identity property.

## Example

The simplest example would be to check if the property name is equal to 'Identifier'.

{CODE:java identity_2@ClientApi\Session\Configuration\FindIdentityProperty.java /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize Collection Assignment for Entities](../../../client-api/session/configuration/how-to-customize-collection-assignment-for-entities)

### Knowledge Base

- [Document Identifier Generation](../../../server/kb/document-identifier-generation)
