# Create  Asp.Net MVC4 Project with RavenDB
In this section we will go over the steps to creating you own Asp.Net MVC4 Application.

## Step by Step Instructions
1) Make sure you have ASP.NET MVC4 installed.  
2) In visual studio and create a new "ASP.NET MVC 4 Web Application" project.  
3) As Project template select "Basic".  
4) Add the NuGet Package named "RavenDB Client".  
5) Create the following controller:

//TODO: connect the code to code sample  

{CODE-START:csharp /}

	public abstract class RavenController : Controller
	{
		public static IDocumentStore DocumentStore
		{
			get { return DocumentStoreHolder.Store; }
		}

		public IDocumentSession RavenSession { get; protected set; }

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			RavenSession = DocumentStoreHolder.Store.OpenSession();
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (filterContext.IsChildAction)
				return;

			using (RavenSession)
			{
				if (filterContext.Exception != null)
					return;

				if (RavenSession != null)
					RavenSession.SaveChanges();
			}

			TaskExecutor.StartExecuting();
		}

		protected HttpStatusCodeResult HttpNotModified()
		{
			return new HttpStatusCodeResult(304);
		}

		protected ActionResult Xml(XDocument xml, string etag)
		{
			return new XmlResult(xml, etag);
		}

		protected new JsonResult Json(object data)
		{
			return base.Json(data, JsonRequestBehavior.AllowGet);
		}
	}

{CODE-END/}

6) From now on write you application as you would normally but Inherit from RavenController in any controller you want to contact RavenDB

Example of a controller: 

{CODE-START:csharp /}

    public class HomeController : RavenController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
	        var items = RavenSession.Query<SampleData>().ToList();
            return View("Index",items);
        }

		public ActionResult Add(string name)
		{
			var sampleData = new SampleData {CreatedAt = SystemTime.UtcNow, Name = name};
			RavenSession.Store(sampleData);
			return RedirectToAction("Edit",new{ id = sampleData.Id});
		}

	    public ActionResult Edit(int id)
	    {
		    var sampleData = RavenSession.Load<SampleData>(id);
		    return View("Index", new List<SampleData> {sampleData});
	    }
    }

{CODE-END/}