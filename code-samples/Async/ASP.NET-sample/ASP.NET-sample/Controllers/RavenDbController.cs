using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using Raven.Client;
using Raven.Client.Document;

namespace ASP.NET_sample.Controllers
{
	public abstract class RavenDbController : ApiController
	{
		public IDocumentStore Store
		{
			get { return LazyDocStore.Value; }
		}

		private static Lazy<IDocumentStore> LazyDocStore = new Lazy<IDocumentStore>(() =>
			{
				var docStore = new DocumentStore
					{
						Url = "http://localhost:8080"
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
			//return base.ExecuteAsync(controllerContext, cancellationToken)
			//	.ContinueWith(task =>
			//	{
			//		using (Session)
			//		{
			//			if (task.Status != TaskStatus.Faulted && Session!= null)
			//				Session.SaveChangesAsync();
			//		}
			//		return task;
			//	}).Unwrap();
		}
	}
}