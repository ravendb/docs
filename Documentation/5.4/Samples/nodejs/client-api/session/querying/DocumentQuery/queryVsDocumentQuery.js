import { 
    DocumentStore, 
    AbstractIndexCreationTask,
    MoreLikeThisStopWords,
    QueryData
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

class Order { }

class Orders_ByShipToAndLines extends AbstractIndexCreationTask { }

async function queryVsDocumentQuery() {
    
    //region query_1
    // This collection query:
    session.query(Order);
    
    // is equivalent to this documentQuery:    
    session.advanced.documentQuery(Order);
    //endregion

    //region query_2
    // This collection query:
    session.query({ collection: "orders" });

    // is equivalent to this documentQuery
    session.advanced.documentQuery({
        collection: "orders",
        indexName: null,
        isMapReduce: false
    });
    //endregion
    
    //region query_3
    // This index query:
    session.query({ indexName: "Orders/ByShipToAndLines" });
    
    // is equivalent to this documentQuery:
    session.advanced.documentQuery({ indexName: "Orders/ByShipToAndLines" });
    //endregion
    
    //region query_4
    // This index query:
    session.query({ indexName: "Orders/ByShipToAndLines" });
    
    // is equivalent to this documentQuery:
    session.advanced.documentQuery({
        indexName: "Orders/ByShipToAndLines",
        isMapReduce: false,
        collection: null
    });
    //endregion
}

