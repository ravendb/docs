# Plugins: Triggers

This type of extensions grants the ability to manipulate while a certain action is taking place e.g. document is being deleted or index is being updated.

Triggers can be divided into four categories:    
* **PUT** triggers   
* **DELETE** triggers  
* **Read** triggers   
* **Index Query** triggers   
* **Index Update** triggers    

{PANEL:**PUT triggers**}

To create his own trigger, one must inherit from the `AbstractPutTrigger` or `AbstractAttachmentPutTrigger`, but before we will do that let's look at them more closely.

{CODE plugins_1@Server\Plugins.cs /}

{CODE plugins_3@Server\Plugins.cs /}

{CODE plugins_2@Server\Plugins.cs /}

where:   
* **AllowPut** is used to `grant` or `deny` the put operation.   
* **OnPut** is used to perform any logic just before the document is saved to the disk.   
* **AfterPut** is used to perform any logic after the document was inserted but still in the same transaction as in `OnPut` method.   
* **AfterCommit** is used to perform any logic `after` the transaction was committed.    
* **Initialize** and **SecondStageInit** are used to trigger the initialization process.   

### Example - Security trigger

{CODE plugins_4@Server\Plugins.cs /}

Most of the logic is in the `AllowPut` method, where we check the existing owner (by checking the current version of the document) and reject the update if the owner doesn't match.
In the `OnPut` method, we ensure that the metadata we need is set up correctly. To control attachment putting, similar trigger can be created.

{PANEL/}

{PANEL:**DELETE triggers**}

Delete triggers is similar in shape to put triggers, yet in contrast to put triggers they control the delete operations. To build your own trigger, you must inherit from `AbstractDeleteTrigger` or `AbstractAttachmentDeleteTrigger`.

{CODE plugins_5@Server\Plugins.cs /}

{CODE plugins_6@Server\Plugins.cs /}

where:   
* **AllowDelete** is used to `grant` or `deny` the delete operation.   
* **OnDelete** is used to perform any logic just before the document is deleted.   
* **AfterDelete** is used to perform any logic after the document has been deleted but still in the same transaction as in `OnDelete` method.   
* **AfterCommit** is used to perform any logic `after` the transaction was committed.    
* **Initialize** and **SecondStageInit** are used to trigger the initialization process.   

### Example - Cascading deletes

{CODE plugins_7@Server\Plugins.cs /}

In this case, we perform another delete operation as a part of the current delete operation. This operation is performed under the same transaction as the original operation.

{PANEL/}

{PANEL:**Read triggers**}

Another type of triggers is used to control the access to documents and manipulate their context when performing read operations. As in the case of the previous triggers, two classes were introduced: the `AbstractReadTrigger` and `AbstractAttachmentReadTrigger`.

{CODE plugins_8@Server\Plugins.cs /}

{CODE plugins_9@Server\Plugins.cs /}

{CODE plugins_1_0@Server\Plugins.cs /}

where:   
* **AllowRead** is used to `grant` or `deny` the read operation.   
* **OnRead** is used to perform any logic just before the document is read e.g. modify the document or document metadata (modified values are transient and are `not saved` to the database).     
* **Initialize** and **SecondStageInit** are used to trigger the initialization process.      

### Example - Information hiding

{CODE plugins_1_1@Server\Plugins.cs /}

In the example above, we only let the owner of a document read it. You can see that a Read trigger can deny the read to the user (returning an error to the user) or ignore the read (hiding the presence of the document). You can also make decisions based on whether that specific document was requested, or if the document was read as a part of a query.

### Example - Linking document on the server side

{CODE plugins_1_2@Server\Plugins.cs /}

In this case, we detect if a document with a link was requested, and we stitch such document together with its link to create a single document.

{PANEL/}

{PANEL:**Index Query triggers**}

Query triggers have been introduced to extend the query parsing capabilities and provide users with a way to modify the queries before they are executed against the index. To write your own query trigger, you must inherit from `AbstractIndexQueryTrigger` class.

{CODE plugins_1_3@Server\Plugins.cs /}

where:   
* **ProcessQuery** is used to perform any logic on the query provided.   
* **Initialize** and **SecondStageInit** are used to trigger the the initialization process.    

### Example - Combining current query with our additional custom logic

{CODE plugins_1_4@Server\Plugins.cs /}

{PANEL/}

{PANEL:**Index Update triggers**}

Index Update triggers allow users to perform custom actions every time an index entry has been created or deleted. To write your own trigger we must consider two classes. The `AbstractIndexUpdateTrigger` and `AbstractIndexUpdateTriggerBatcher` defined below.

{CODE plugins_2_0@Server\Plugins.cs /}

where:   
* **CreateBatcher** is used to construct a batcher for a given index.   
* **Initialize** and **SecondStageInit** are used to trigger the initialization process.    

{CODE plugins_2_1@Server\Plugins.cs /}

where:   
* **OnIndexEntryDeleted** is executed when index entry is being removed from the index. The provided key may represent an already deleted document.    
* **OnIndexEntryCreated** is executed when specified document with a given key is being inserted. The changes introduced to the provided lucene document will be written to the Lucene index.    
* **AnErrorOccured** is used to notify the batcher that an error occurred.   

### Example - Creating static snapshot from the indexed document

{CODE plugins_2_2@Server\Plugins.cs /}

This index works on the [following index](https://ayende.com/blog/4530/raven-event-sourcing) in order to create a static snapshot of the indexed document whenever it is indexed. Note that we use identity insert here (the key we use ends with '/') so we will have documents like this:

* shoppingcarts/12/snapshots/1
* shoppingcarts/12/snapshots/2
* shoppingcarts/12/snapshots/3

This is useful if we want to keep a record of all the changes introduced to the index. Note that we also change the document to store the snapshot key for this particular version.   

{PANEL/}
