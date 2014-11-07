using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ASP.NET_MVC4.Extensions
{
	public class XmlResult : ActionResult
	{
		private readonly XDocument _document;
		private readonly string _etag;

		public XmlResult(XDocument document, string etag)
		{
			_document = document;
			_etag = etag;
		}

		public override void ExecuteResult(ControllerContext context)
		{
			if (_etag != null)
				context.HttpContext.Response.AddHeader("ETag", _etag);

			context.HttpContext.Response.ContentType = "text/xml";

			using (var xmlWriter = XmlWriter.Create(context.HttpContext.Response.OutputStream))
			{
				_document.WriteTo(xmlWriter);
				xmlWriter.Flush();
			}
		}
	}
}