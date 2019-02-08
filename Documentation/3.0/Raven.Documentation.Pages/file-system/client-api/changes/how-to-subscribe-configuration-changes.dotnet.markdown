#How to subscribe to configuration item changes?

All configuration changes can be tracked by using `ForConfiguration` method.

##Syntax

{CODE for_configuration_1@FileSystem\ClientApi\Changes.cs /}


| Return Value | |
| ------------- | ------------- |
| **IObservableWithTask&lt;[ConfigurationChangeNotification](../../../glossary/configuration-change-notification)&gt;** | The observable that allows to add subscriptions to received notifications |

##Example

{CODE for_configuration_2@FileSystem\ClientApi\Changes.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}
