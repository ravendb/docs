import { DocumentStore } from "ravendb";

const store = new DocumentStore();

async function includeRevisions() {

    const session = store.openSession();
    {
        //region include_1
        // The revision creation time
        // For example - looking for a revision from last month
        const creationTime = new Date();
        creationTime.setMonth(creationTime.getMonth() - 1).toLocaleString();

        // Load a document:
        const order = await session.load("orders/1-A", {
            // Pass the revision creation time to 'includeRevisions'
            // The revision will be 'loaded' to the session along with the document
            includes: builder => builder.includeRevisions(creationTime)
        });

        // Get the revision by creation time - it will be retrieved from the SESSION
        // No additional trip to the server is made
        const revision = await session
            .advanced.revisions.get("orders/1-A", creationTime);
        //endregion
    }
    {
        //region include_2
        // Load a document:
        const contract = await session.load("contracts/1-A", {
            includes: builder => builder
                // Pass the path to the document property that contains the revision change vector(s)
                // The revision(s) will be 'loaded' to the session along with the document
                .includeRevisions("revisionChangeVector")  // Include a single revision
                .includeRevisions("revisionChangeVectors") // Include multiple revisions
        });

        // Get the revision(s) by change vectors - it will be retrieved from the SESSION
        // No additional trip to the server is made
        const revision = await session
            .advanced.revisions.get(contract.revisionChangeVector);
        const revisions = await session
            .advanced.revisions.get(contract.revisionChangeVectors);
        //endregion
    }
    {
        //region include_3
        // The revision creation time
        // For example - looking for a revision from last month
        const creationTime = new Date();
        creationTime.setMonth(creationTime.getMonth() - 1).toLocaleString();

        // Define the query:
        const query = session.query({collection: "Orders"})
            .whereEquals("ShipTo.Country", "Canada")
            // Pass the revision creation time to 'includeRevisions'
            .include(builder => builder.includeRevisions(creationTime));

        // Execute the query:
        // For each document in the query results,
        // the matching revision will be 'loaded' to the session along with the document
        const orderDocuments = await query.all();

        // Get a revision by its creation time for a document from the query results
        // It will be retrieved from the SESSION - no additional trip to the server is made
        const revision = await session
            .advanced.revisions.get(orderDocuments[0].id, creationTime);
        //endregion
    }
    {
        //region include_4
        // Define the query:
        const query = session.query({collection: "Contracts"})
            // Pass the path to the document property that contains the revision change vector(s)
            .include(builder => {
                builder
                    .includeRevisions("revisionChangeVector")  // Include a single revision
                    .includeRevisions("revisionChangeVectors") // Include multiple revisions
            });

        // Execute the query:
        // For each document in the query results,
        // the matching revisions will be 'loaded' to the session along with the document
        const orderDocuments = await query.all();

        // Get the revision(s) by change vectors - it will be retrieved from the SESSION
        // No additional trip to the server is made
        const revision = await session
            .advanced.revisions.get(orderDocuments[0].revisionChangeVector);
        const revisions = await session
            .advanced.revisions.get(orderDocuments[0].revisionChangeVectors);
        //endregion
    }
    {
        //region include_5
        // The revision creation time
        // For example - looking for a revision from last month
        const creationTime = new Date();
        creationTime.setMonth(creationTime.getMonth() - 1).toLocaleString();

        // Define the Raw Query:
        const rawQuery = session.advanced
            // Use 'include revisions' in the RQL
            .rawQuery("from Orders include revisions($p0)")
            // Pass the revision creation time 
            .addParameter("p0", creationTime);

        // Execute the query:
        // For each document in the query results,
        // the matching revision will be 'loaded' to the session along with the document
        const orderDocuments = await rawQuery.all();

        // Get a revision by its creation time for a document from the query results
        // It will be retrieved from the SESSION - no additional trip to the server is made
        const revision = await session
            .advanced.revisions.get(orderDocuments[0].Id, creationTime);
        //endregion
    }
    {
        //region include_6
        // Define the Raw Query:
        const rawQuery = session.advanced
            // Use 'include revisions' in the RQL
            .rawQuery("from Contracts include revisions($p0, $p1)")
            // Pass the path to the document properties containing the change vectors
            .addParameter("p0", "revisionChangeVector")
            .addParameter("p1", "revisionChangeVectors");

        // Execute the raw query:
        // For each document in the query results,
        // the matching revisions will be 'loaded' to the session along with the document
        const orderDocuments = await rawQuery.all();

        // Get the revision(s) by change vectors - it will be retrieved from the SESSION
        // No additional trip to the server is made
        const revision = await session
            .advanced.revisions.get(orderDocuments[0].revisionChangeVector);
        const revisions = await session
            .advanced.revisions.get(orderDocuments[0].revisionChangeVectors);
        //endregion
    }
    {
        //region include_7
        // Get the revisions' metadata for document 'contracts/1-A'
        const contractRevisionsMetadata = await session
            .advanced.revisions.getMetadataFor("contracts/1-A");

        // Get a change vector from the metadata
        const metadata = orderRevisionsMetadata[0];
        const changeVector = metadata[CONSTANTS.Documents.Metadata.CHANGE_VECTOR];

        // Patch the document - add the revision change vector to a specific document property
        await session.advanced.patch("contracts/1-A", "revisionChangeVector", changeVector);

        // Save your changes
        await session.saveChanges();
        //endregion
    }
}

{
    //region syntax
    object includeRevisions(before);
    object includeRevisions(path);
    //endregion
}

{
    //region sample_document
    // Sample Contract document
    class Contract {
        id: string;
        name: string;
        revisionChangeVector: string;
        revisionChangeVectors: string[];
    }
    //endregion
}

