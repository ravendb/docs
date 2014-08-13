# Client API : How to subscribe to index changes?

Following methods allow you to subscribe to index changes:

- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)

## ForIndex

Index changes for one index can be observed using `ForIndex` method.

### Syntax

{CODE index_changes_1@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

**Parameters**   

indexName
:   Type: string   
Name of an index for which notifications will be processed.

**Return value**

Type: IObservableWithTask<[IndexChangeNotification](../../glossary/client-api/changes/index-change-notification)>   
Observable that allows to add subscribtions to notifications for index with given name.

### Example

{CODE index_changes_2@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

## ForAllIndexes

Index changes for all indexex can be observed using `ForAllIndexes` method.

**Return value**

Type: IObservableWithTask<[IndexChangeNotification](../../glossary/client-api/changes/index-change-notification)>   
Observable that allows to add subscribtions to notifications for all indexes.

### Syntax

{CODE index_changes_3@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

### Example

{CODE index_changes_4@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](http://nuget.org/packages/Rx-Main) package to your project. /}

#### Related articles

TODO