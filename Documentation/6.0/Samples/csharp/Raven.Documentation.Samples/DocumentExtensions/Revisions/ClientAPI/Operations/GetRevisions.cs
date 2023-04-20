using System.Collections.Generic;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Revisions;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.DocumentExtensions.Revisions.ClientAPI.Operations
{
    public class GetRevisions
    {
        public GetRevisions()
        {
            using (var documentStore = new DocumentStore())
            {
                #region getAllRevisions
                // Define the get revisions operation, pass the document id
                var getRevisionsOp = new GetRevisionsOperation<Company>("Companies/1-A");
                
                // Execute the operation by passing it to Operations.Send
                RevisionsResult<Company> revisions = documentStore.Operations.Send(getRevisionsOp);

                // The revisions info:
                List<Company> allRevisions = revisions.Results; // All the revisions
                int revisionsCount = revisions.TotalResults;    // Total number of revisions
                #endregion
            }
            
            using (var documentStore = new DocumentStore())
            {
                #region getRevisionsWithPaging
                var start = 0;
                var pageSize = 100;
                
                while (true)
                {
                    // Execute the get revisions operation
                    // Pass the document id, start & page size to get
                    RevisionsResult<Company> revisions = documentStore.Operations.Send(
                        new GetRevisionsOperation<Company>("Companies/1-A", start, pageSize));
                    
                    {
                        // Process the retrieved revisions here
                    }
                    
                    if (revisions.Results.Count < pageSize)
                        break; // No more revisions to retrieve

                    // Increment 'start' by page-size, to get the "next page" in next iteration
                    start += pageSize;
                }
                #endregion
            }
            
            using (var documentStore = new DocumentStore())
            {
                #region getRevisionsWithPagingParams
                var parameters = new GetRevisionsOperation<Company>.Parameters
                {
                    Id = "Companies/1-A",
                    Start = 0,
                    PageSize = 100
                };

                RevisionsResult<Company> revisions = documentStore.Operations.Send(
                    new GetRevisionsOperation<Company>(parameters));
                #endregion
            }
        }
        
        public async Task GetRevisionsAsync()
        {
            using (var documentStore = new DocumentStore())
            {
                #region getAllRevisions_async
                // Define the get revisions operation, pass the document id
                var getRevisionsOp = new GetRevisionsOperation<Company>("Companies/1-A");
                
                // Execute the operation by passing it to Operations.Send
                RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(getRevisionsOp);

                // The revisions info:
                List<Company> allRevisions = revisions.Results; // All the revisions
                int revisionsCount = revisions.TotalResults;    // Number of revisions
                #endregion
            }

            using (var documentStore = new DocumentStore())
            {
                #region getRevisionsWithPaging_async
                var start = 0;
                var pageSize = 100;
                
                while (true)
                {
                    // Execute the get revisions operation
                    // Pass the document id, start & page size to get
                    RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(
                        new GetRevisionsOperation<Company>("Companies/1-A", start, pageSize));
                    {
                        // Process the retrieved revisions here
                    }
                    
                    if (revisions.Results.Count < pageSize)
                        break; // No more revisions to retrieve

                    // Increment 'start' by page-size, to get the "next page" in next iteration
                    start += pageSize;
                }
                #endregion
            }

            using (var documentStore = new DocumentStore())
            {
                #region getRevisionsWithPagingParams_async
                var parameters = new GetRevisionsOperation<Company>.Parameters
                {
                    Id = "Companies/1-A",
                    Start = 0,
                    PageSize = 100
                };

                RevisionsResult<Company> revisions = await documentStore.Operations.SendAsync(
                    new GetRevisionsOperation<Company>(parameters));
                #endregion
            }
        }
    }
}
