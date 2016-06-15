using System.Net.Http;
using Raven.Client.Document;

namespace Raven.Documentation.Samples.ClientApi.HowTo
{
    public class CreateCustomHttpMessageHandler
    {
        public CreateCustomHttpMessageHandler()
        {
            using (var documentStore = new DocumentStore())
            {
                #region custom_handler_provided
                documentStore.HttpMessageHandlerFactory = () => new WebRequestHandler
                {
                    UnsafeAuthenticatedConnectionSharing = true,
                    PreAuthenticate = true
                };
                #endregion
            }
        }
    }
}