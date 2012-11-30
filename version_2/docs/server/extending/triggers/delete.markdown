#Delete Triggers
Delete triggers implement the AbstractDeleteTrigger interface and follow much the same pattern as PUT triggers.

![Figure 1: Triggers - Delete](images\triggers_delete_docs.png)

**Example: Cascading deletes**

{CODE delete_1@Server\Extending\Triggers\Delete.cs /}

In this case, we perform another delete operation as part of the current delete operation. This operation is done under the same transaction as the original operation.