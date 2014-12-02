using System.Collections.Generic;
using System.Web.Mvc;
using ASP.NET_MVC4.Models;
using System.Linq;
using Raven.Abstractions;

namespace ASP.NET_MVC4.Controllers
{
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
}
