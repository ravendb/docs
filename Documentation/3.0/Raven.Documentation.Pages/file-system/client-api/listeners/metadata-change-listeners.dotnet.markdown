#Metadata change listeners

Implement and register `IMetadataChangeListener` if you want to provide custom logic when file metadata was changed in a session and
the metadata update operation is executed. The interface looks as follow:

{CODE interface@FileSystem\ClientApi\Listeners\MetadataChange.cs /}

The same like for delete operations you can prevent from updating metadata by returning `false` from `BeforeChange` method. The operation will be skipped then
and `AfterChange` won't be executed.

#Example

You can modify the metadata before it will be sent to the server. For example you can remove tags that are banned:

{CODE example@FileSystem\ClientApi\Listeners\MetadataChange.cs /}