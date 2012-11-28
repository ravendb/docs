using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using Raven.Client;
using Raven.Client.Document;

namespace ASP.NET_WebApi.Controllers
{
	public abstract class RavenDbController : ApiController
	{
		public IDocumentStore Store
		{
			get { return LazyDocStore.Value; }
		}

		private static readonly Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
		{
			var docStore = new DocumentStore
			{
				Url = "http://localhost:8080",
				DefaultDatabase = "WebApiSample"
			};

			docStore.Initialize();
			return docStore;
		});

		public IAsyncDocumentSession Session { get; set; }


		public async override Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(
			HttpControllerContext controllerContext,
			CancellationToken cancellationToken)
		{
			using (Session = Store.OpenAsyncSession())
			{
				var result = await base.ExecuteAsync(controllerContext, cancellationToken);
				await Session.SaveChangesAsync();

				return result;
			}
		}
	}
}