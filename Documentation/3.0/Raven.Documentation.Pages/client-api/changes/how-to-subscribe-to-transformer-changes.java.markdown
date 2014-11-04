# Changes API : How to subscribe to transformer changes?

All transformer changes can be tracked using `forAllTransformers` method.

## Syntax

{CODE:java transformer_changes_1@ClientApi\Changes\HowToSubscribeToTransformerChanges.java /}

| Return value | |
| ------------- | ----- |
| IObservable<[TransformerChangeNotification](../../glossary/transformer-change-notification)> | Observable that allows to add subscribtions to notifications for all transformers. |

Type: IObservable<[TransformerChangeNotification](../../glossary/transformer-change-notification)>   
Observable that allows to add subscribtions to notifications for all transformers.

## Example

{CODE:java transformer_changes_2@ClientApi\Changes\HowToSubscribeToTransformerChanges.java /}

## Related articles

TODO