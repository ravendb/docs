using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Documentation.Samples.Orders;
using Xunit;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class DeleteSamples
    {
        public async Task Examples()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region delete_document_1
                    var command = new DeleteDocumentCommand("employees/1-A", null);
                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region delete_document_1_async
                    var command = new DeleteDocumentCommand("employees/1-A", null);
                    await asyncSession.Advanced.RequestExecutor.ExecuteAsync(command, asyncSession.Advanced.Context);
                    #endregion
                }
                
                using (var session = store.OpenSession())
                {
                    #region delete_document_2
                    // Load a document
                    var employeeDocument = session.Load<Employee>("employees/2-A");
                    var cv = session.Advanced.GetChangeVectorFor(employeeDocument);

                    // Modify the document content and save changes
                    // The change-vector of the stored document will change
                    employeeDocument.Title = "Some new title";
                    session.SaveChanges();
                    
                    try
                    {
                        // Try to delete the document with the previous change-vector
                        var command = new DeleteDocumentCommand("employees/2-A", cv);
                        session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                    }
                    catch (Exception e)
                    {
                        // A concurrency exception is thrown
                        // since the change-vector of the document in the database
                        // does not match the change-vector specified in the delete command
                        Assert.IsType<Raven.Client.Exceptions.ConcurrencyException>(e);
                    }
                    #endregion
                }
                
                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region delete_document_2_async
                    // Load a document
                    var employeeDocument = await asyncSession.LoadAsync<Employee>("employees/2-A");
                    var cv = asyncSession.Advanced.GetChangeVectorFor(employeeDocument);

                    // Modify the document content and save changes
                    // The change-vector of the stored document will change
                    employeeDocument.Title = "Some new title";
                    asyncSession.SaveChangesAsync();
                    
                    try
                    {
                        // Try to delete the document with the previous change-vector
                        var command = new DeleteDocumentCommand("employees/2-A", cv);
                        await asyncSession.Advanced.RequestExecutor.ExecuteAsync(command, asyncSession.Advanced.Context);
                    }
                    catch (Exception e)
                    {
                        // A concurrency exception is thrown
                        // since the change-vector of the document in the database
                        // does not match the change-vector specified in the delete command
                        Assert.IsType<Raven.Client.Exceptions.ConcurrencyException>(e);
                    }
                    #endregion
                }
            }
        }
    }
    
    public class DeleteInterfaces
    {
        private class DeleteDocumentCommand
        {
            #region syntax
            public DeleteDocumentCommand(string id, string changeVector)
            #endregion
            {
            }
        }
    }
}
