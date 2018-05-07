package net.ravendb.ClientApi.Session.Configuration;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;

public class CustomizeCollectionAssignmentForEntities {
    public CustomizeCollectionAssignmentForEntities() {
        DocumentStore store = new DocumentStore();

        //region custom_collection_name
        store.getConventions().setFindCollectionName(clazz -> {
            if (Category.class.isAssignableFrom(clazz)) {
                return "ProductGroups";
            }

            return DocumentConventions.defaultGetCollectionName(clazz);
        });
        //endregion
    }

    private static class Category {
    }
}
