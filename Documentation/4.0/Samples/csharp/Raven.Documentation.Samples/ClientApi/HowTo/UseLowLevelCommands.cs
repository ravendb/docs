using System.Threading;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands;
using Raven.Client.Documents.Session;
using Raven.Client.Http;
using Sparrow.Json;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
    public class UseLowLevelCommands
    {
        public interface IInterface
        {
            #region Execute
            void Execute<TResult>(RavenCommand<TResult> command, JsonOperationContext context, SessionInfo sessionInfo = null)
            #endregion
            ;
            #region Execute_async
            Task ExecuteAsync<TResult>(RavenCommand<TResult> command, JsonOperationContext context, SessionInfo sessionInfo = null, CancellationToken token = default(CancellationToken))
            #endregion
            ;
        }

        public async Task Examples()
        {
            using (var documentStore = new DocumentStore())
            {
                #region commands_1
                using (var session = documentStore.OpenSession())
                {
                    var command = new GetDocumentsCommand("orders/1-A", null, false);
                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                }
                #endregion

                #region commands_1_async
                using (var session = documentStore.OpenAsyncSession())
                {
                    var command = new GetDocumentsCommand("orders/1-A", null, false);
                    await session.Advanced.RequestExecutor.ExecuteAsync(command, session.Advanced.Context);
                    var order = (BlittableJsonReaderObject)command.Result.Results[0];
                }
                #endregion

                #region commands_2
                using (var session = documentStore.OpenSession())
                {
                    var command = new DeleteDocumentCommand("employees/1-A", null);
                    session.Advanced.RequestExecutor.Execute(command, session.Advanced.Context);
                }
                #endregion

                #region commands_2_async
                using (var session = documentStore.OpenAsyncSession())
                {
                    var command = new DeleteDocumentCommand("employees/1-A", null);
                    await session.Advanced.RequestExecutor.ExecuteAsync(command, session.Advanced.Context);
                }
                #endregion
            }
        }
    }
}
