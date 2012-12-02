namespace RavenCodeSamples
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

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

	public static class TaskExecutor
	{
		public static void StartExecuting()
		{
		}
	}

	public static class DocumentStoreHolder
	{
		public static dynamic Store { get; set; }
	}

	public class HttpResponseMessage
	{
		public HttpResponseMessage(object a)
		{
		}
	}

	public class HttpControllerContext
	{
	}

	public abstract class ApiController
	{
		public virtual async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
		{
			return null;
		}
	}

	public abstract class Controller
	{
		public ActionResult RedirectToAction(object a, object b)
		{
			return null;
		}

		public ActionResult View(object a, object b)
		{
			return null;
		}

		public JsonResult Json(object a, object b)
		{
			return null;
		}

		protected virtual void OnActionExecuted(ActionExecutedContext filterContext)
		{
		}

		protected virtual void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}
	}

	public class ActionResult
	{
	}

	public class XmlResult : ActionResult
	{
		public XmlResult(object a, object b)
		{
		}
	}

	public class HttpStatusCodeResult
	{
		public HttpStatusCodeResult(object a)
		{
		}
	}

	public class ActionExecutingContext
	{
	}

	public class ActionExecutedContext
	{
		public bool IsChildAction { get; set; }

		public object Exception { get; set; }
	}

	public enum JsonRequestBehavior
	{
		AllowGet
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

	public class FromBodyAttribute : Attribute
	{
	}
}