# Authorization: How to work with authorization bundle?

In order to use this bundle first activate it on the server. The description can be found in [server section](../../server/bundles/authorization).

To work with a database where authorization is configured you need to reference `Raven.Client.Authorization` on the client side.

{CODE using@ClientApi\Bundles\HowToWorkWithAuthorizationBundle.cs /}

The Authorization Bundle usage on the client side is limited to three methods:

* SetAuthorizationFor - which sets up permissions and tags for a specific document.
* GetAuthorizationFor - which allows you to read what permission and tags were set on a specific document.
* SecureFor - setup which user and operation are being performed.

For example, here is the code for authorization hospitalization:

{CODE secure_for@ClientApi\Bundles\HowToWorkWithAuthorizationBundle.cs /}

If the user doesn't have the permissions to authorize hospitalization, an error will be raised when the change to the database in the SaveChanges call is persistent.

## Related articles

* [Bundle : Authorization](../../server/bundles/authorization)

