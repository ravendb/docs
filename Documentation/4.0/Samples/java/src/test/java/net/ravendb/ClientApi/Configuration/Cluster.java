package net.ravendb.ClientApi.Configuration;

import net.ravendb.client.documents.conventions.DocumentConventions;
import net.ravendb.client.http.ReadBalanceBehavior;

public class Cluster {
    public Cluster() {

        DocumentConventions conventions = new DocumentConventions();

        //region ReadBalanceBehavior
        conventions.setReadBalanceBehavior(ReadBalanceBehavior.FASTEST_NODE);
        //endregion
    }
}
