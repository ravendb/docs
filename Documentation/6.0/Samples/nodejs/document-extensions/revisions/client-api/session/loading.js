import { DocumentStore } from "ravendb";

const store = new DocumentStore();

async function samples() {
    {
        const session = store.openSession();
        
        {
            //region example_1
            // Get revisions for document 'orders/1-A'
            const orderRevisions = await session.advanced.revisions
                .getFor("orders/1-A", {
                    start: 0,
                    pageSize: 10
                });
            //endregion
        }
        {
            //region example_2
            // Get revisions' metadata for document 'orders/1-A'
            const orderRevisionsMetadata = await session.advanced.revisions
                .getMetadataFor("orders/1-A", {
                    start: 0,
                    pageSize: 10
                });

            // Each item returned is a revision's metadata, as can be verified in the @flags key
            const metadata = orderRevisionsMetadata[0];
            const flagsValue = metadata[CONSTANTS.Documents.Metadata.FLAGS];

            assertThat(flagsValue).contains("Revision");
            //endregion
        }
        {
            //region example_3
            // Creation time to use, e.g. last year: 
            const creationTime = new Date();
            creationTime.setFullYear(creationTime.getFullYear() - 1);

            // Get a revision by its creation time
            // If no revision was created at the specified time,
            // then the first revision that precedes it will be returned
            const orderRevision = await session.advanced.revisions
                .get("orders/1-A", creationTime.toLocaleDateString());
            //endregion
        }
        {
            //region example_4
            // Get revisions metadata 
            const revisionsMetadata = await session.advanced.revisions
                .getMetadataFor("orders/1-A", {
                    start: 0,
                    pageSize: 25
                });
            
            // Get the change-vector from the metadata
            var changeVector = revisionsMetadata[0][CONSTANTS.Documents.Metadata.CHANGE_VECTOR];
            
            // Get the revision by its change-vector
            const orderRevision = await session.advanced.revisions
                .get(changeVector);
            //endregion
        }
    }
}

{
    const session = store.openSession();
    //region syntax_1
    // Available overloads:
    await session.advanced.revisions.getFor(id);
    await session.advanced.revisions.getFor(id, options);
    //endregion

    //region syntax_2
    // Available overloads:
    await session.advanced.revisions.getMetadataFor(id);
    await session.advanced.revisions.getMetadataFor(id, options);
    //endregion

    //region syntax_3
    await session.advanced.revisions.get(id, date);
    //endregion

    //region syntax_4
    // Available overloads:
    await session.advanced.revisions.get(changeVector);
    await session.advanced.revisions.get(changeVectors);
    //endregion

    //region syntax_5
    // options object
    {
        start,   // The first revision to retrieve, used for paging. Default is 0.
        pageSize // Number of revisions to retrieve per results page. Default is 25.
    }
    //endregion
}
