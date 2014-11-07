using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ASP.NET_WebApi.Models;
using Raven.Client;

namespace ASP.NET_WebApi.Controllers
{
	public class SampleController : RavenDbController
	{
		public Task<IList<string>> GetDocs()
		{
			return Session.Query<WebData>().Select(data => data.Name).ToListAsync();
		}

		public HttpResponseMessage Put(WebData data)
		{
			if (data == null)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Null data.");
			}

			Session.Store(data);

			return Request.CreateResponse(HttpStatusCode.Created);
		}
	}
}