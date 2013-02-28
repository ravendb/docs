namespace RavenCodeSamples.Samples.Mvc
{
	namespace Foo
	{
		using System;
		using System.Collections.Generic;
		using System.Linq;
		using System.Xml.Linq;

		using Raven.Abstractions;
		using Raven.Client;

		#region create_asp_net_mvc_4_project_1
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

		#endregion

		public class SampleData
		{
			public string Id { get; set; }

			public DateTime CreatedAt { get; set; }

			public string Name { get; set; }
		}

		#region create_asp_net_mvc_4_project_2
		public class HomeController : RavenController
		{
			//
			// GET: /Home/

			public ActionResult Index()
			{
				var items = RavenSession.Query<SampleData>().ToList();
				return View("Index", items);
			}

			public ActionResult Add(string name)
			{
				var sampleData = new SampleData { CreatedAt = SystemTime.UtcNow, Name = name };
				RavenSession.Store(sampleData);
				return RedirectToAction("Edit", new { id = sampleData.Id });
			}

			public ActionResult Edit(int id)
			{
				var sampleData = RavenSession.Load<SampleData>(id);
				return View("Index", new List<SampleData> { sampleData });
			}
		}

		#endregion
	}

	public class CreateAspNetMvc4Project : MvcCodeSampleBase
	{

	}
}