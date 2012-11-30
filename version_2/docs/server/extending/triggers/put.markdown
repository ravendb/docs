#Put triggers
PUT triggers are classes that inherit the AbstractPutTrigger class:

![Figure 1: Triggers - Put](images\triggers_put_docs.png)

1. AllowPut gives the trigger the chance to reject the PUT operation.
2. OnPut gives the trigger the chance to modify the document and metadata before the changes are saved.
3. AfterPut gives the trigger the chance to perform operations in the same transaction as the put.
4. AfterCommit gives the trigger the chance to notify external parties about the Put operations.

**Example: Security trigger**

{CODE put_1@Server\Extending\Triggers\Put.cs /}

Most of the logic is in AllowPut method, where we check the existing owner (by checking the current version of the document) and reject the update if it if the owner doesn't match.
In the OnPut method, we ensure that the metadata we need is setup correctly.