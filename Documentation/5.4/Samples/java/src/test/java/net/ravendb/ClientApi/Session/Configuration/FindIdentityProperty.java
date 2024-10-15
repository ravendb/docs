package net.ravendb.ClientApi.Session.Configuration;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;

import java.beans.PropertyDescriptor;
import java.util.function.Function;

public class FindIdentityProperty {

    private interface IFoo {
        //region identity_1
        public void setFindIdentityProperty(Function<PropertyDescriptor, Boolean> findIdentityProperty);
        //endregion
    }

    public FindIdentityProperty() {
        try (IDocumentStore store = new DocumentStore()) {
            //region identity_2
            store.getConventions().setFindIdentityProperty(property -> "Identifier".equals(property.getName()));
            //endregion
        }
    }
}
