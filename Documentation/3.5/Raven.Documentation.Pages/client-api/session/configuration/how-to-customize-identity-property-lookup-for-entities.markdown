# Session: How to customize identity property lookup for entities?

The client must know which property of your entity is considered as an identity. By default it always look for `Id` property. However is configurable by using [`FindIdentityProperty` convention](../../configuration/conventions/identifier-generation/global#findidentityproperty).
