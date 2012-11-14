# Create  Asp.Net Project with RavenDB
In this section we will go over the steps to creating you own Asp.Net Web Application.

## Step by Step Instructions
1) Make sure you have ASP.NET Web API installed.  
2) In visual studio and create a new "ASP.NET MVC 4 Web Application" project.  
3) As Project template select "Web API".  
4) Add the NuGet Package named "RavenDB Client".  
5) Create the following controller:

{CODE-START:csharp /}

	public class RavenDbController : ApiController
    {
		public DocumentStore Store { get; set; }

	    public RavenDbController()
	    {
		    Store = new DocumentStore
			    {
				    Url = "http://localhost:8080"				
			    };
	    }

		public override Task<System.Net.Http.HttpResponseMessage> ExecuteAsync(
			HttpControllerContext controllerContext, 
			CancellationToken cancellationToken)
		{
			return base.ExecuteAsync(controllerContext, cancellationToken)
				.ContinueWith(task =>
				{
					using (var asyncSession = Store.OpenAsyncSession())
					{
						if (task.Status != TaskStatus.Faulted && asyncSession != null)
							asyncSession.SaveChangesAsync();
					}
					return task;
				}).Unwrap();
		}
    }

{CODE-END/}

6) From now on write you application as you would normally but Inherit from RavenDbController in any controller you want to contact RavenDB