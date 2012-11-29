namespace RavenCodeSamples
{
	using System;

	public abstract class MvcCodeSampleBase : CodeSampleBase
	{
		public dynamic ModelState { get; set; }

		public dynamic MvcApplication { get; set; }

		public dynamic Response { get; set; }

		public JsonResult Json(object value)
		{
			return null;
		}
	}

	public class JsonResult
	{
	}

	public class ActionNameAttribute : Attribute
	{
		public string Name { get; set; }

		public ActionNameAttribute(string name)
		{
			this.Name = name;
		}
	}

	public class HttpPostAttribute : Attribute
	{
	}
}