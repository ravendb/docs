# Session: How to Customize the Identity Property Lookup for Entities

The client must know which property of your entity is considered as an identity. By default, it always looks for the `id` field (case-sensitive). This behavior can be changed by overwriting one of our conventions called `identityProperty`.

## Syntax

{CODE:nodejs identity_1@client-api\session\configuration\findIdentityProperty.js /}

## Example

Here's how to change it to `Identifier` field.

{CODE:nodejs identity_2@client-api\session\configuration\findIdentityProperty.js /}

## Related articles

### Configuration

- [Conventions](../../../client-api/configuration/conventions)
- [How to Customize ID Generation for Entities](../../../client-api/session/configuration/how-to-customize-id-generation-for-entities)
- [How to Customize Collection Assignment for Entities](../../../client-api/session/configuration/how-to-customize-collection-assignment-for-entities)

### Knowledge Base

- [Document Identifier Generation](../../../server/kb/document-identifier-generation)
