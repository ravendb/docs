
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

async function queryAndLuceneQuery() {
    
        {
            //region query_1a
            session.query(Order);
            //endregion

            //region query_1b
            session.advanced.documentQuery(Order);
            //endregion

            //region query_2a
            session.query({ indexName: "Orders/ByShipToAndLines" });
            //endregion

            //region query_2b
            session.advanced.documentQuery({ indexName: "Orders/ByShipToAndLines" });
            //endregion

            //region query_3a
            session.query({ indexName: "Orders/ByShipToAndLines" });
            //endregion

            //region query_3b
            session.advanced.documentQuery({ 
                indexName: "Orders/ByShipToAndLines", 
                isMapReduce: false,
                collection: null
            });
            //endregion

            //region query_4a
            session.query({ collection: "orders" });
            //endregion

            //region query_4b
            session.advanced.documentQuery({ 
                collection: "orders",
                indexName: null,
                isMapReduce: false
            });
            //endregion
        }
}

