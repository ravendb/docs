#How to subscribe to file changes?

In order to retrieve notifications about file changes located in a given directory use `ForFolder` method.

##Syntax

{CODE for_folder_1@FileSystem\ClientApi\Changes.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **folder** | string | The name of a directory for which notifications will be sent. |

<hr />

| Return Value | |
| ------------- | ------------- |
| **IObservableWithTask&lt;FileChangeNotification&gt;** | The observable that allows to add subscriptions to received notifications |

##Example

{CODE for_folder_2@FileSystem\ClientApi\Changes.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](http://nuget.org/packages/Rx-Main) package to your project. /}
