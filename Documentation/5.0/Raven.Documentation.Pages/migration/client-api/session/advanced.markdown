# Migration: Changes Related to IAdvancedDocumentSessionOperations

Changes made to Advanced session operations are cosmetic and are related to abstracting the JSON serialization layer. You can read more about it [here](../../../migration/client-api/conventions). Beside JSON serialization changes, starting from version 5.0, the Session starts to track CompareExchange values and because of that the 'UpdateCompareExchangeValue' is no longer needed.

{PANEL:EntityToBlittable}

This property is gone and was substituted by the `JsonConverter` property with `ISessionBlittableJsonConverter` type. The converter contains following methods:

- `ToBlittable` - similar to `EntityToBlittable.ConvertEntityToBlittable`
- `FromBlittable` - similar to `EntityToBlittable.ConvertToEntity`
- `PopulateEntity`
- `RemoveFromMissing`
- `Clear`

If you were using the `EntityToBlittable` then please just switch to the new `JsonConverter` and take leverage from one of the new methods with similar functionality.

{PANEL/}

{PANEL:ClusterTransaction}

In ClusterTransaction the `UpdateCompareExchangeValue` method was removed. The method is no longer needed, because the Session, starting from version 5.0 keeps track of the changes to compare exchange values.

To give an example. in version 4.x following flow was valid:

{CODE-BLOCK:csharp}

var user = DocumentSession.ClusterTransaction.GetCompareExchangeValue<User>("users/ayende");
user.Value.Name = "New Name";

DocumentSession.ClusterTransaction.UpdateCompareExchangeValue(user);
DocumentSession.SaveChanges();

{CODE-BLOCK/}

Starting from 5.0 the flow is simplified:

{CODE-BLOCK:csharp}

var user = DocumentSession.ClusterTransaction.GetCompareExchangeValue<User>("users/ayende");
user.Value.Name = "New Name";

DocumentSession.SaveChanges();

{CODE-BLOCK/}

{PANEL/}
