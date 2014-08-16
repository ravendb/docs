# Changes API : How to subscribe to transformer changes?

All transformer changes can be tracked using `ForAllTransformers` method.

## Syntax

{CODE transformer_changes_1@ClientApi\Changes\HowToSubscribeToTransformerChanges.cs /}

**Return value**

Type: IObservableWithTask<[TransformerChangeNotification](../../glossary/client-api/changes/transformer-change-notification)>   
Observable that allows to add subscribtions to notifications for all transformers.

## Example

{CODE transformer_changes_2@ClientApi\Changes\HowToSubscribeToTransformerChanges.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](http://nuget.org/packages/Rx-Main) package to your project. /}

#### Related articles

TODO