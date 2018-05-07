package net.ravendb.ClientApi.Operations.Indexes;

import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.indexes.IndexDefinition;
import net.ravendb.client.documents.indexes.IndexDefinitionBuilder;
import net.ravendb.client.documents.operations.indexes.PutIndexesOperation;

import java.util.Collections;

public class Put {

    private interface IFoo {
        /*
        //region put_1_0
        PutIndexesOperation(IndexDefinition... indexToAdd)
        //endregion
        */
    }

    public Put() {
        try (IDocumentStore store = new DocumentStore()) {
            //region put_1_1
            IndexDefinition indexDefinition = new IndexDefinition();
            indexDefinition.setMaps(Collections.singleton("from order in docs.Orders select new { " +
                "   order.Employee," +
                "   order.Company," +
                "   Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" +
                "}"));

            store.maintenance().send(new PutIndexesOperation(indexDefinition));
            //endregion

            //region put_1_2
            IndexDefinitionBuilder builder = new IndexDefinitionBuilder();
            builder.setMap("from order in docs.Orders select new { " +
                "   order.Employee," +
                "   order.Company," +
                "   Total = order.Lines.Sum(l => (l.Quantity * l.PricePerUnit) * (1 - l.Discount))" +
                "}");

            IndexDefinition definition = builder.toIndexDefinition(store.getConventions());
            store.maintenance()
                .send(new PutIndexesOperation(definition));
            //endregion
        }
    }
}
