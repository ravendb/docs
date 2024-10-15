# Session: How to Customize the Identity Property Lookup for Entities

The client must know which property of your entity is considered as an identity. By default, it always looks for the `Id` property (case-sensitive). This behavior can be changed by overwriting one of our conventions called `FindIdentityProperty`.

## Syntax

{CODE identity_1@ClientApi\Session\Configuration\FindIdentityProperty.cs /}

`MemberInfo` will represent the member of a stored entity, and return value (bool) will indicate if given member is an identity property.

## Example

The simplest example would be to check if the property name is equal to 'Identifier'.

{CODE identity_2@ClientApi\Session\Configuration\FindIdentityProperty.cs /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize Collection Assignment for Entities](../../../client-api/session/configuration/how-to-customize-collection-assignment-for-entities)

### Knowledge Base

- [Document Identifier Generation](../../../server/kb/document-identifier-generation)
