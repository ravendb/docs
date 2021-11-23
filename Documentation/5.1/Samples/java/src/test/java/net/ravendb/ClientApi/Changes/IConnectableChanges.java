package net.ravendb.ClientApi.Changes;

import net.ravendb.client.documents.changes.IDatabaseChanges;
import net.ravendb.client.primitives.CleanCloseable;
import net.ravendb.client.primitives.EventHandler;
import net.ravendb.client.primitives.VoidArgs;

import java.util.function.Consumer;

//region connectable_changes
public interface IConnectableChanges<TChanges extends IDatabaseChanges> extends CleanCloseable {

    boolean isConnected();

    void ensureConnectedNow();

    void addConnectionStatusChanged(EventHandler<VoidArgs> handler);

    void removeConnectionStatusChanged(EventHandler<VoidArgs> handler);

    void addOnError(Consumer<Exception> handler);

    void removeOnError(Consumer<Exception> handler);
}
//endregion

