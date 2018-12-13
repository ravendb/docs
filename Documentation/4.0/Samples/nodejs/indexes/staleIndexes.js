import { 
    DocumentStore,
    AbstractMultiMapIndexCreationTask
} from "ravendb";

const store = new DocumentStore();
{
    class Product { }

    async function example() {
        const session = store.openSession();

            {
                //region stale1
                let stats;

                const results = await session.query(Product)
                    .statistics($ => stats = $)
                    .whereGreaterThan("PricePerUnit", 10)
                    .all();

                if (stats.stale) {
                    // results are known to be stale
                }
                //endregion
            }

            {
                //region stale2
                const results = session
                    .query(Product)
                    .waitForNonStaleResults(5000)
                    .whereGreaterThan("PricePerUnit", 10)
                    .all();
                //endregion

                //region stale3
                store.addSessionListener("beforeQuery", event => {
                    event.queryCustomization.waitForNonStaleResults();
                });
                //endregion
            }

            {
                //region stale4
                session.advanced.waitForIndexesAfterSaveChanges();
                //endregion

                //region stale5
                session.advanced.waitForIndexesAfterSaveChanges({
                    withTimeout: 5000,
                    throwOnTimeout: false,
                    waitForIndexes: "Products/ByName"
                });
                //endregion
            }
    }
}
