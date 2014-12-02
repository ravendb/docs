namespace RavenCodeSamples.Server.Extending.RequestResponders
{
	using Raven.Database.Extensions;
	using Raven.Database.Server;
	using Raven.Database.Server.Abstractions;
	using Raven.Imports.Newtonsoft.Json;

	#region index_1
	public class DocSize : AbstractRequestResponder
	{
		public override string UrlPattern
		{
			get { return "/docsize/(.+)"; }
		}

		public override string[] SupportedVerbs
		{
			get { return new[] { "GET" }; }
		}

		public override void Respond(IHttpContext context)
		{
			var match = urlMatcher.Match(context.Request.Url.LocalPath);
			var docId = match.Groups[1].Value;
			var jsonDocument = Database.Get(docId, null);
			if (jsonDocument == null)
			{
				context.SetStatusToNotFound();
				return;
			}
			context.WriteJson(new
				                  {
					                  Key = docId, 
									  Size = jsonDocument.DataAsJson.ToString(Formatting.Indented).Length
				                  });
		}
	}

	#endregion

	public class Index : CodeSampleBase
	{
	}
}