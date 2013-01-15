#Plugins

Under `Raven.Database.Plugins` namespace various interfaces and classes can be found that might be used to extend the database behavior.

##Triggers

This type of extensions grants the ability to manipulate when certain action is taking place e.g. document is deleted or index is updated.

Triggers can be divided to four categories:    
* **PUT** triggers   
* **DELETE** triggers  
* **Read** triggers   
* **Query** triggers   

###PUT triggers

To create his own trigger, one must inherit from `AbstractPutTrigger` or `AbstractAttachmentPutTrigger`, but before we will do that lets take a look at them more closely.

{CODE plugins_1@Server\Extending\Plugins\Index.cs /}

{CODE plugins_3@Server\Extending\Plugins\Index.cs /}

{CODE plugins_2@Server\Extending\Plugins\Index.cs /}

where:   
* **AllowPut** is used to `grant` or `deny` the put operation.   
* **OnPut** is used to perform any logic just before the document is saved to the disk.   
* **AfterPut** is used to perform any logic after the document was inserted but still in the same transaction as in `OnPut` method.   
* **AfterCommit** is used to perform any logic `after` the transaction was commited.    
* **Initialize** and **SecondStageInit** are used in trigger initialization process.   

**Example: Security trigger**

{CODE plugins_4@Server\Extending\Plugins\Index.cs /}

Most of the logic is in `AllowPut` method, where we check the existing owner (by checking the current version of the document) and reject the update if it if the owner doesn't match.
In the `OnPut` method, we ensure that the metadata we need is setup correctly. To control attachment putting, similar trigger can be created.

###DELETE triggers

Delete triggers are similar in shape to the put triggers, but in contrast to them they control the delete operations. To build your own trigger, one must inherit from `AbstractDeleteTrigger` or `AbstractAttachmentDeleteTrigger`.

{CODE plugins_5@Server\Extending\Plugins\Index.cs /}

{CODE plugins_6@Server\Extending\Plugins\Index.cs /}

where:   
* **AllowDelete** is used to `grant` or `deny` the delete operation.   
* **OnDelete** is used to perform any logic just before the document is deleted.   
* **AfterDelete** is used to perform any logic after the document has been deleted but still in the same transaction as in `OnDelete` method.   
* **AfterCommit** is used to perform any logic `after` the transaction was commited.    
* **Initialize** and **SecondStageInit** are used in trigger initialization process.   

**Example: Cascading deletes**

{CODE plugins_7@Server\Extending\Plugins\Index.cs /}

In this case, we perform another delete operation as part of the current delete operation. This operation is done under the same transaction as the original operation.

###Read triggers

Another type of triggers is used to control the access to the documents and manipulate their context when performing read operations. Similar to the previous triggers, two classes were introduced, the `AbstractReadTrigger` and `AbstractAttachmentReadTrigger`.

{CODE plugins_8@Server\Extending\Plugins\Index.cs /}

{CODE plugins_9@Server\Extending\Plugins\Index.cs /}

{CODE plugins_1_0@Server\Extending\Plugins\Index.cs /}

where:   
* **AllowRead** is used to `grant` or `deny` the read operation.   
* **OnRead** is used to perform any logic just before the document is read e.g. modify the document or document metadata (modified values are transient and are `not saved` to the database).     
* **Initialize** and **SecondStageInit** are used in trigger initialization process.      

**Example: Information hiding**

{CODE plugins_1_1@Server\Extending\Plugins\Index.cs /}

In the example above, we only let the owner of a document to read it. You can see that a Read trigger can deny the read to the user (returning an error to the user) or ignoring the read (hiding the presence of the document. You can also make decisions based on whatever that specific document was requested, or if the document was read as part of a query.

**Example: Linking document on the server side**

{CODE plugins_1_2@Server\Extending\Plugins\Index.cs /}

In this case, we detect that a document with a link was requested, and we stitch the document together with its link to create a single document.

###Query triggers

Query triggers have been introduced to extend the query parsing capabilities and provide users with a way to modify the queries before they are executed against the index. To write your own query trigger, you must inherit from `AbstractIndexQueryTrigger` class.

{CODE plugins_1_3@Server\Extending\Plugins\Index.cs /}

where:   
* **ProcessQuery** is used to perform any logic on the providen query.   
* **Initialize** and **SecondStageInit** are used in trigger initialization process.    

