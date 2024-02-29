import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

class Syntax {
    //region create_compare_exchange
    session.advanced.clusterTransaction.createCompareExchangeValue(key, item);
    //endregion

    //region get_compare_exchange_1
    await session.advanced.clusterTransaction.getCompareExchangeValue(key);
    //endregion

    //region get_compare_exchange_2
    await session.advanced.clusterTransaction.getCompareExchangeValues(keys);
    //endregion

    //region get_compare_exchange_3
    // Single item
    const lazyItem = session.advanced.clusterTransaction.lazily.getCompareExchangeValue(key);
    const item = await lazyItem.getValue();

    // Multiple items
    const lazyItems = session.advanced.clusterTransaction.lazily.getCompareExchangeValues(keys);
    const items = await lazyItems.getValue();
    //endregion

    //region delete_compare_exchange
    // Delete by key & index
    session.advanced.clusterTransaction.deleteCompareExchangeValue(key, index);
    
    // Delete by compare-exchange item
    session.advanced.clusterTransaction.deleteCompareExchangeValue(item);
    //endregion
}


async function examples() {
    {
        //region create_compare_exchange_example
        // The session must be first opened with cluster-wide mode
        const session = documentStore.openSession({
            transactionMode: "ClusterWide"
        });
        
        session.advanced.clusterTransaction.createCompareExchangeValue(
            "Best NoSQL Transactional Database", "RavenDB"  // key, value
        );
        
        await session.saveChanges();
        //endregion
    }
}
