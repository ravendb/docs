package net.ravendb.clientapi.changes;

import net.ravendb.abstractions.basic.CleanCloseable;
import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.data.DataSubscriptionChangeNotification;
import net.ravendb.client.changes.IObservable;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.utils.Observers;

public class HowToSubscribeToDataSubscriptionChanges {


    @SuppressWarnings("unused")
    private interface IFoo {

        //region data_subscription_changes_1
        IObservable<DataSubscriptionChangeNotification> forAllDataSubscriptions();
        //endregion

        //region data_subscription_changes_3
        IObservable<DataSubscriptionChangeNotification> forDataSubscription(long id);
        //endregion
    }

    public HowToSubscribeToDataSubscriptionChanges() {
        try (DocumentStore store = new DocumentStore()) {
            {
                //region data_subscription_changes_2
                CleanCloseable subscription = store.
                        changes()
                        .forAllDataSubscriptions()
                        .subscribe(Observers.create(new Action1<DataSubscriptionChangeNotification>() {
                            @Override
                            public void apply(DataSubscriptionChangeNotification change) {
                                long subscriptionId = change.getId();

                                switch (change.getType()) {
                                    case SUBSCRIPTION_OPENED:
                                        // do something
                                        break;
                                    case SUBSCRIPTION_RELEASED:
                                        // do something
                                        break;
                                }
                            }
                        }));
                //endregion

                //region data_subscription_changes_4
                int subscriptionId = 3;

                CleanCloseable subscription2 = store.
                        changes()
                        .forDataSubscription(3)
                        .subscribe(Observers.create(new Action1<DataSubscriptionChangeNotification>() {
                            @Override
                            public void apply(DataSubscriptionChangeNotification change) {
                                long subscriptionId = change.getId();

                                switch (change.getType()) {
                                    case SUBSCRIPTION_OPENED:
                                        // do something
                                        break;
                                    case SUBSCRIPTION_RELEASED:
                                        // do something
                                        break;
                                }
                            }
                        }));

                //endregion
            }
        }
    }
}
