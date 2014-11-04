namespace Raven.Documentation.Web
{
	using System.Web.Mvc;
	using System.Web.Routing;

	using Raven.Documentation.Parser.Data;
	using Raven.Documentation.Web.Routing;

	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRouteLowerCase(
				"Docs",
				"article/{version}/{language}/{*key}",
				new
					{
						controller = MVC.Docs.Name,
						action = MVC.Docs.ActionNames.Articles
					},
				new
					{
						version = "1.0|2.0|2.5|3.0",
						language = "csharp|java|http"
					},
				new[] { "Raven.Documentation.Web.Controllers" });

			routes.MapRouteLowerCase(
				"Client",
				"client-api/{version}/{language}",
				new
				{
					controller = MVC.Docs.Name,
					action = MVC.Docs.ActionNames.Client
				},
				new
				{
					version = "1.0|2.0|2.5|3.0",
					language = "csharp|java|http"
				},
				new[] { "Raven.Documentation.Web.Controllers" });

			routes.MapRouteLowerCase(
				"Routes",
				"{action}/{version}/{language}",
				new
					{
						controller = MVC.Docs.Name
					},
				new
					{
						version = "1.0|2.0|2.5|3.0",
						language = "csharp|java|http"
					},
				new[] { "Raven.Documentation.Web.Controllers" });

			routes.MapRouteLowerCase(
				"Default",
				"{action}/{version}/{language}",
				new
					{
						controller = MVC.Docs.Name,
						action = MVC.Docs.ActionNames.Index,
						version = "3.0",
						language = Language.Csharp
					},
				new
					{
						version = "1.0|2.0|2.5|3.0",
						language = "csharp|java|http"
					},
				new[] { "Raven.Documentation.Web.Controllers" });
		}
	}
}
