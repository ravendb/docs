#Plugins

Under `Raven.Database.Plugins` namespace various interfaces and classes can be found that might be used to extend the database behavior.

{NOTE All DLL's containing custom plugins must be placed in `Plugins` directory that by default are found in `~\Plugins`. To change the default location of this directory, please refer to [this](../administration/configuration#bundles) page. /}

##Triggers

This type of extensions grants the ability to manipulate when certain action is taking place e.g. document is deleted or index is updated.

Triggers can be divided to four categories:    
* **PUT** triggers   
* **DELETE** triggers  
* **Read** triggers   
* **Index Query** triggers   
* **Index Update** triggers    

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

In the example above, we only let the owner of a document to read it. You can see that a Read trigger can deny the read to the user (returning an error to the user) or ignoring the read (hiding the presence of the document). You can also make decisions based on wheter that specific document was requested, or if the document was read as part of a query.

**Example: Linking document on the server side**

{CODE plugins_1_2@Server\Extending\Plugins\Index.cs /}

In this case, we detect that a document with a link was requested, and we stitch the document together with its link to create a single document.

###Index Query triggers

Query triggers have been introduced to extend the query parsing capabilities and provide users with a way to modify the queries before they are executed against the index. To write your own query trigger, you must inherit from `AbstractIndexQueryTrigger` class.

{CODE plugins_1_3@Server\Extending\Plugins\Index.cs /}

where:   
* **ProcessQuery** is used to perform any logic on the provided query.   
* **Initialize** and **SecondStageInit** are used in trigger initialization process.    

**Example: Combining current query with our additional custom logic**

{CODE plugins_1_4@Server\Extending\Plugins\Index.cs /}

###Index Update triggers

Index Update triggers allow users to perform custom actions every time an index entry has been created or deleted. To write your own trigger we must consider two classes. The `AbstractIndexUpdateTrigger` and `AbstractIndexUpdateTriggerBatcher` defined below.

{CODE plugins_2_0@Server\Extending\Plugins\Index.cs /}

where:   
* **CreateBatcher** is used to construct a batcher for given index.   
* **Initialize** and **SecondStageInit** are used in trigger initialization process.    

{CODE plugins_2_1@Server\Extending\Plugins\Index.cs /}

where:   
* **OnIndexEntryDeleted** is executed when index entry is being removed from the index. The provided key may represent an already deleted document.    
* **OnIndexEntryCreated** is executed when specified document with a given key is being inserted. The changes to the provided lucene document will be writen to the Lucene index.    
* **AnErrorOccured** is used to notify the batcher that an error occured.   

**Example: Creating static snapshot from the indexed document**

{CODE plugins_2_2@Server\Extending\Plugins\Index.cs /}

This index works on the [following index](https://ayende.com/blog/4530/raven-event-sourcing) in order to create a static snapshot of the indexed document whenever it is indexed. Note that we use identity insert here (the key we use ends with '/') so we will have documents like this:

* shoppingcarts/12/snapshots/1
* shoppingcarts/12/snapshots/2
* shoppingcarts/12/snapshots/3

This is nice if we want to keep a record of all the changes to the index. Note that we also change the document to store the snapshot key for this particular version.    

##Codecs

The `AbstractDocumentCodec` and `AbstractIndexCodec` classes have been introduced as an entry point to custom compression methods.

{CODE plugins_3_0@Server\Extending\Plugins\Index.cs /}

{CODE plugins_3_1@Server\Extending\Plugins\Index.cs /}

where:   
* **Encode** is executed when given document/index is written.   
* **Decode** is executed when provided document/index is read.    
* **Initialize** and **SecondStageInit** are used in trigger initialization process.   

**Example: Compression**

{CODE plugins_3_2@Server\Extending\Plugins\Index.cs /}

**Example: Encryption**

{CODE plugins_3_3@Server\Extending\Plugins\Index.cs /}

##Tasks

Another type of plugins gives us the ability to perform various actions during server/database startup process or enables us to perform actions periodically. For these needs we have introduced two interfaces and one abstract class.

{CODE plugins_4_0@Server\Extending\Plugins\Index.cs /}

{CODE plugins_4_1@Server\Extending\Plugins\Index.cs /}

where:   
* `IStartupTask` can be used to implement a task that will be started during database initialization.   
* `IServerStartupTask` can be used to implement a task that will be started during server initialization.    
* `AbstractBackgroundTask ` is a base for all periodic tasks.    

**Example: Send email when server is starting**

{CODE plugins_4_2@Server\Extending\Plugins\Index.cs /}

**Example: Perform a cleanup task during database initialization**

{CODE plugins_4_3@Server\Extending\Plugins\Index.cs /}

**Example: Perform a cleanup task every six hours**

{CODE plugins_4_4@Server\Extending\Plugins\Index.cs /}

##Compilation Extensions

There might be a certian situations when users want to put more complex logic to calculate a value of an index entry field. To do this, in RavenDB, we have introduced an `AbstractDynamicCompilationExtension`.

{CODE plugins_6_0@Server\Extending\Plugins\Index.cs /}

where:   
* **GetNamespacesToImport** returns a list of namespaces that RavenDB will have to import   
* **GetAssembliesToReference** returns a list of full paths to assemblies    

**Example: Check if a given word is a [palindrome](https://en.wikipedia.org/wiki/Palindrome)**

{CODE plugins_6_1@Server\Extending\Plugins\Index.cs /}

{CODE plugins_6_2@Server\Extending\Plugins\Index.cs /}

Now we can use our `Palindrome` in our index definition.

{CODE plugins_6_3@Server\Extending\Plugins\Index.cs /}

##Analyzer Generators

To add your custom analyzer, one must implement the `AbstractAnalyzerGenerator` class and provide logic when your custom analyzer should be used.

{CODE plugins_7_0@Server\Extending\Plugins\Index.cs /}

where:   
* **GenerateAnalyzerForIndexing** returns an analyzer that will be used while performing indexing operation.   
* **GenerateAnalyzerForQuerying** returns an analyzer that will be used while performing querying.    

**Example: Using different analyzer for specific index**

{CODE plugins_7_1@Server\Extending\Plugins\Index.cs /}

##Database configuration

To alter database configuration you can edit the configuration document (more about how it can be done and what configuration options are available can be found [here](../../server/administration/configuration)), but sometimes it might be better to change configuration programatically e.g. imagine a situation, where you have 100 databases and you want to change one setting in every one of them. This is why the `IAlterConfiguration` interfaces was created.

{CODE plugins_5_0@Server\Extending\Plugins\Index.cs /}

**Example: Disable compression and extend temp index cleanup period**

{CODE plugins_5_1@Server\Extending\Plugins\Index.cs /}