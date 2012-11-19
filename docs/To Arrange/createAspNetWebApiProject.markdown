# Create  Asp.Net Project with RavenDB
In this section we will go over the steps to creating you own Asp.Net Web Application.

## Step by Step Instructions
1) Make sure you have ASP.NET Web API installed.  
2) In visual studio and create a new "ASP.NET MVC 4 Web Application" project.  
3) As Project template select "Web API".  
4) Add the NuGet Package named "RavenDB Client".  
5) Create the following controller:

{CODE-START:csharp /}

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
						Url = "http://localhost:8080",
						DefaultDatabase = "Asp.Net-Sample"
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

{CODE-END/}

6) From now on write you application as you would normally but Inherit from RavenDbController in any controller you want to contact RavenDB