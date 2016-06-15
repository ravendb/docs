#Data subscription events

The `Subscription` instance exposes a few events which allow to provide control over the data stream. There are four subscription events that you can hook up:

{CODE:java events@ClientApi\DataSubscriptions\DataSubscriptions.java /}

Each of them is invoked on a different level of documents processing:

- `beforeBatch` - called when a first document from the batch is about to be processed by handlers, if the batch is empty then the event is not raised,
- `beforeAcknowledgment` - triggered after processing all documents in batch, the returned value determines if the batch can be acknowledged (default: `true`),
- `afterAcknowledgment` - invoked after the batch processed acknowledgment had been sent to the server,
- `afterBatch` - called after processing all docs from the batch.

