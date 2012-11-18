using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using ASP.NET_sample.Models;
using Raven.Client;

namespace ASP.NET_sample.Controllers
{
    public class SampleController : RavenDbController
    {
		public Task<IList<string>> GetDocs()
		{
			return Session.Query<WebData>().Select(data => data.Name).ToListAsync();
		}

		public void PutItem(string name)
		{
			Session.Store(new WebData{Name = name});
		}
    }
}
