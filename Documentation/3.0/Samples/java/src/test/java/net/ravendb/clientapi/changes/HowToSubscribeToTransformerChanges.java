package net.ravendb.clientapi.changes;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.TransformerChangeNotification;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;


public class HowToSubscribeToTransformerChanges {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region transformer_changes_1
    public IObservable<TransformerChangeNotification> forAllTransformers();
    //endregion
  }

  public HowToSubscribeToTransformerChanges() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region transformer_changes_2
      store
        .changes()
        .forAllTransformers()
        .subscribe(Observers.create(new Action1<TransformerChangeNotification>() {
          @Override
          public void apply(TransformerChangeNotification change) {
            switch (change.getType()) {
              case TRANSFORMER_ADDED:
                // do something
                break;
              case TRANSFORMER_REMOVED:
                // do something
                break;
              default:
                break;
            }
          }
        }));
      //endregion
    }
  }
}
