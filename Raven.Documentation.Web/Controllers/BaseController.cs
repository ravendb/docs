namespace Raven.Documentation.Web.Controllers
{
	using System;
	using System.Web.Mvc;

	using Raven.Client;
	using Raven.Client.Document;
	using Raven.Documentation.Parser.Data;

	public abstract partial class BaseController : Controller
	{
		protected Language CurrentLanguage { get; private set; }

		protected string CurrentVersion { get; private set; }

		private readonly DocumentStore _store;

		protected DocumentStore DocumentStore
		{
			get
			{
				return _store;
			}
		}

		protected IDocumentSession DocumentSession { get; private set; }

		protected BaseController(DocumentStore store)
		{
			_store = store;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			CurrentLanguage = GetLanguage(filterContext);
			CurrentVersion = GetVersion(filterContext);

			ViewBag.CurrentLanguage = CurrentLanguage;
			ViewBag.CurrentVersion = CurrentVersion;

			DocumentSession = _store.OpenSession();
			base.OnActionExecuting(filterContext);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (DocumentSession != null)
			{
				DocumentSession.Dispose();
			}

			base.OnActionExecuted(filterContext);
		}

		private static string GetVersion(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionParameters.ContainsKey("version") == false)
				return DocsController.DefaultVersion;

			return filterContext.ActionParameters["version"].ToString();
		}

		private static Language GetLanguage(ActionExecutingContext filterContext)
		{
			if (filterContext.ActionParameters.ContainsKey("language") == false)
				return DocsController.DefaultLanguage;

			return (Language)Enum.Parse(typeof(Language), filterContext.ActionParameters["language"].ToString(), true);
		}
	}
}