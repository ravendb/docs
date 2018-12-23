package net.ravendb.ClientApi.Configuration;

import net.ravendb.client.documents.DocumentStore;

public class Querying {
    public Querying() {

        DocumentStore store = new DocumentStore();

        //region throw_if_query_page_is_not_set
        store.getConventions().setThrowIfQueryPageSizeIsNotSet(true);
        //endregion
    }
}
