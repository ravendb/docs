# Migration: Changes Related to IAdvancedDocumentSessionOperations

Changes made to Advanced session operations are cosmetic and are related to abstracting the JSON serialization layer. You can read more about it [here](../../../migration/client-api/conventions).

{PANEL:EntityToBlittable}

This property is gone and was substituted by the `JsonConverter` property with `ISessionBlittableJsonConverter` type. The converter contains following methods:

- `ToBlittable` - similar to `EntityToBlittable.ConvertEntityToBlittable`
- `FromBlittable` - similar to `EntityToBlittable.ConvertToEntity`
- `PopulateEntity`
- `RemoveFromMissing`
- `Clear`

If you were using the `EntityToBlittable` then please just switch to the new `JsonConverter` and take leverage from one of the new methods with similar functionality.

{PANEL/}
