using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

		public HttpResponseMessage Put([FromBody]string value)
		{
			Session.Store(new WebData{Name = value});

			return new HttpResponseMessage(HttpStatusCode.Created);
		}
    }
}