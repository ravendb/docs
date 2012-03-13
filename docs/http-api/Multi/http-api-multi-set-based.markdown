#Set based operations

Typically, document databases don't support set based operations. Raven does for deletes and updates, for inserts, you can POST to the [bulk_docs]()//"TODO: link to batching" endpoint (this is how the client API behaves).

Set based operations are based on very simple idea, you pass a query to a Raven index, and Raven will delete all the documents matching the query. All operations that are supported with an [index query]()//"TODO: link to index query" are supported for set based operations. You need to specify the index that you intend to operate on, the actual query, the [optional cut off point]()//"TODO: link to stale indexes" and whatever to allow this operation over a stale index.

Note that Raven indexes are allowed to be stale. If the index for the set based operation is stale, Raven will fail the operation. You can control this behavior using the following options:

* cutOff - determines what is the cut off point for considering the index stale.
* allowStale - determines if the operation is allowed to proceed on a stale index (default: false)

##Set based deletes

For example, let us say that we wanted to delete all the inactive users, we can define an index for the user activity status:

    from user in docs.Users
    select new{user.IsActive}

And now we can issue the following command:

DELETE [http://localhost:8080/bulk_docs/UsersByActivityStatus?query=IsActive:False](http://localhost:8080/bulk_docs/UsersByActivityStatus?query=IsActive:False)

This will remove all the documents from the UsersByActivityStatus where IsActive equals to false.

This is the equivalent for:

    DELETE FROM Users
    WHERE IsActive = 0

##Set based updates

Set based updates work very similarly to set based deletes. They require an index to operate on an a query for this index. But they use the [PATCH format]()//"TODO: link to patch" as their payload. For example, if we wanted to mark all the users who haven't logged on recently as inactive, we could define the following index:

    from user in docs.Users
    select new { user.LastLoginDate }

And then issue the following command:

PATCH [http://localhost:8080/bulk_docs/UsersByLastLoginDate?query=LastLoginDate:[NULL TO 20100527]](http://localhost:8080/bulk_docs/UsersByLastLoginDate?query=LastLoginDate:[NULL TO 20100527])

    [
       { "Type": "Set", "Name": "IsActive", "Value": false
    ]

This is the equivalent for:

    UPDATE Users
    SET IsActive = 0
    WHERE LastLoginDate < '2010-05-27'