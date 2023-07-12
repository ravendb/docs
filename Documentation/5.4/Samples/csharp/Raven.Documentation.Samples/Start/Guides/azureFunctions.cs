using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Raven.Client.Documents.Session;
using System.Threading.Tasks;

namespace Raven.Documentation.Samples.Start
{
    public class HttpTrigger_1
    {
        object user = "user";

        #region HttpTrigger1
        private readonly IAsyncDocumentSession session;

        public HttpTrigger_1(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        [FunctionName("HttpTrigger_1")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
        {
            // Access `session` within the body of the function

            var user = await session.LoadAsync<object>("users/100");

            return new OkObjectResult(user);
        }
        #endregion
    }
    
    public class HttpTrigger_2
    {
        #region HttpTrigger2
        private readonly IAsyncDocumentSession session;

        public HttpTrigger_2(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        [FunctionName("HttpTrigger_2")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "{id:int}")] int id,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var user = await session.LoadAsync<object>("users/" + id);

            return new OkObjectResult(user);
        }
        #endregion
    }
}
