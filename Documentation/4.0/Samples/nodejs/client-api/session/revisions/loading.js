
import { DocumentStore } from "ravendb";

const store = new DocumentStore();

{
    let id, options, callback, changeVector, documentType, changeVectors;

    const session = store.openSession();
    //region syntax_1
    session.advanced.revisions.getFor(id, [options], [callback]);
    //endregion

    //region syntax_2
    session.advanced.revisions.getMetadataFor(id, [options], [callback]);
    //endregion

    //region syntax_3
    session.advanced.revisions.get(changeVector, [documentType], [callback]);
    session.advanced.revisions.get(changeVectors, [documentType], [callback]);
    //endregion
}

class Order { }

async function samples() {
    {
        const session = store.openSession();

        {
            //region example_1_sync
            const orderRevisions = await session.advanced.revisions
                .getFor("orders/1-A", {
                    start: 0,
                    pageSize: 10
                });
            //endregion
        }

        {
            //region example_2_sync
            const orderRevisionsMetadata = await session.advanced.revisions
                .getMetadataFor("orders/1-A", {
                    start: 0,
                    pageSize: 10
                });
            //endregion
        }

        {
            let orderRevisionChangeVector;
            //region example_3_sync
            const orderRevision = await session.advanced.revisions
                .get(orderRevisionChangeVector);
            //endregion
        }
    }
}
