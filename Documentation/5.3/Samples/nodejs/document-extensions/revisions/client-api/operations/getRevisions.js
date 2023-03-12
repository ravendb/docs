import { DocumentStore } from "ravendb";

const documentStore = new DocumentStore();

async function getRevisions() {
    {
        //region getAllRevisions
        // Define the get revisions operation, pass the document id
        const getRevisionsOp = new GetRevisionsOperation("Companies/1-A");

        // Execute the operation by passing it to operations.send
        const revisions = await documentStore.operations.send(getRevisionsOp);

        // The revisions info:
        const allRevisions = revisions.results;        // All the revisions
        const revisionsCount = revisions.totalResults; // Total number of revisions
        //endregion
    }
    {
        //region getRevisionsWithPaging
        const parameters = {
            start: 0,
            pageSize: 100
        };
        
        while (true)
        {
            // Execute the get revisions operation
            // Pass parameters with document id, start & page size
            const revisions = await documentStore.operations.send(
                new GetRevisionsOperation("Companies/1-A", parameters));
            
            {
                // Process the retrieved revisions here
            }

            if (revisions.results.length < parameters.pageSize)
                break; // No more revisions to retrieve

            // Increment 'start' by page-size, to get the "next page" in next iteration
            parameters.start += parameters.pageSize;
        }
        //endregion
    }
}

{
    //region syntax_1  
    // Available overloads:
    const getRevisionsOp = new GetRevisionsOperation(id);
    const getRevisionsOp = new GetRevisionsOperation(id, parameters);
    //endregion

    //region syntax_2
    // The parameters object
    {
        start,   // Revision number to start from
        pageSize // Number of revisions to get
    }
    //endregion

    //region syntax_3
    class RevisionsResult
    {
        results;      // The retrieved revisions
        totalResults; // Total number of revisions that exist for the document
    }
    //endregion
}
