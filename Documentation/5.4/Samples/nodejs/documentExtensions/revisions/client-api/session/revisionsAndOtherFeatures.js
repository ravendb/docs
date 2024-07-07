import { DocumentStore } from "ravendb";

const store = new DocumentStore();

async function getCount() {

    const session = store.openSession();

    //region extract_counters    
    // Use getMetadataFor to get revisions metadata for document 'orders/1-A'
    const revisionsMetadata = await session.advanced.revisions.getMetadataFor("orders/1-A");

    // Extract the counters data from the metadata
    const countersDataInRevisions = revisionsMetadata
        .filter(x => !!x[CONSTANTS.Documents.Metadata.REVISION_COUNTERS])
        .map(x => x[CONSTANTS.Documents.Metadata.REVISION_COUNTERS]);
    //endregion
}
