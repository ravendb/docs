namespace RavenCodeSamples.Samples.WebApi
{
	namespace Foo
	{
		using System;
		using System.Collections.Generic;
		using System.Net;
		using System.Threading;
		using System.Threading.Tasks;

		using Raven.Client;
		using Raven.Client.Document;
		using Raven.Client.Linq;

		#region create_asp_net_web_api_project_1
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

			public async override Task<HttpResponseMessage> ExecuteAsync(
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

		#endregion

		public class WebData
		{
			public string Name { get; set; }
		}

		#region create_asp_net_web_api_project_2
		public class SampleController : RavenDbController
		{
			public Task<IList<string>> GetDocs()
			{
				return Session.Query<WebData>().Select(data => data.Name).ToListAsync();
			}

			public HttpResponseMessage Put([FromBody]string value)
			{
				Session.Store(new WebData { Name = value });

				return new HttpResponseMessage(HttpStatusCode.Created);
			}
		}

		#endregion
	}

	public class CreateAspNetWebApiProject : MvcCodeSampleBase
	{

	}
}