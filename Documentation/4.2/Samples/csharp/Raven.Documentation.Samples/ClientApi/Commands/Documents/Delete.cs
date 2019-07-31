
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;

namespace Raven.Documentation.Samples.ClientApi.Commands.Documents
{
    public class DeleteInterfaces
    {
        private class DeleteDocumentCommand
        {
            #region delete_interface
            public DeleteDocumentCommand(string id, string changeVector)
            #endregion
            {
            }
        }
    }

    public class DeleteSamples
	{
		public void Foo()
		{
            using (var store = new DocumentStore())
            using (var session = store.OpenSession())
            {
                #region delete_sample
                var command = new DeleteDocumentCommand("employees/1-A", null);
                session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                #endregion
            }
		}
	}
}
