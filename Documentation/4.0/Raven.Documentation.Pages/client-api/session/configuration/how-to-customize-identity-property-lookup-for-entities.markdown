# Session : How to customize identity property lookup for entities?

The client must know which property of your entity is considered as an identity. By default it always looks for `Id` property (case-sensitive). This behavior can be changed by overwriting one of our Conventions called `FindIdentityProperty`.

## Syntax

{CODE identity_1@ClientApi\Session\Configuration\FindIdentityProperty.cs /}

where `MemberInfo` will represent the member of a stored entity and return value (bool) will indicate if given member should is an identity property.

## Example

The simplest example would be to check if the property name is equal to 'Identifier'.

{CODE identity_2@ClientApi\Session\Configuration\FindIdentityProperty.cs /}
