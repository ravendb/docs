namespace Raven.Documentation.Web.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text;
	using System.Web.Mvc;
	using System.Web.Mvc.Html;
	using System.Web.Routing;

	using Raven.Documentation.Parser.Data;

	public static class HtmlHelperExtensions
	{
		public static bool IsDebug(this HtmlHelper htmlHelper)
		{
			return DebugHelper.IsDebug();
		}

		public static MvcHtmlString VersionLink(this HtmlHelper htmlHelper, string version, string currentVersion, Language currentLanguage)
		{
			var isCurrentVersion = version == currentVersion;

			var css = "btn";
			if (isCurrentVersion)
				css += " btn-success";
			else
				css += " btn-default";

			return htmlHelper.ActionLink(version, MVC.Docs.ActionNames.Welcome, MVC.Docs.Name, new { version = version, language = currentLanguage }, new { @class = css });
		}

		public static MvcHtmlString LanguageLink(this HtmlHelper htmlHelper, Language language, ViewContext viewContext)
		{
			var controllerName = viewContext.Controller.ControllerContext.RouteData.Values["controller"].ToString();
			var actionName = viewContext.Controller.ControllerContext.RouteData.Values["action"].ToString();

			var currentLanguage = (Language)viewContext.ViewBag.CurrentLanguage;

			var isCurrentLanguage = language == currentLanguage;

			var css = "btn";
			if (isCurrentLanguage)
				css += " btn-success";
			else
				css += " btn-default";

			var routeValues = new RouteValueDictionary(viewContext.Controller.ControllerContext.RouteData.Values);
			routeValues["language"] = language;

			return htmlHelper.ActionLink(language.GetDescription(), actionName, controllerName, routeValues, new Dictionary<string, object> { { "class", css } });
		}

		public static MvcHtmlString GenerateNavigation(this HtmlHelper htmlHelper, Language language, string version)
		{
			var mode = GetMode(version);

			switch (mode)
			{
				case DocumentationMode.Normal:
					return GenerateNavigationFor30(htmlHelper, language);
				case DocumentationMode.Legacy:
					return GenerateNavigationFor25(htmlHelper, language);
			}

			return null;
		}

		private static MvcHtmlString GenerateNavigationFor30(HtmlHelper htmlHelper, Language language)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<ul class='nav navbar-nav'>");

			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Getting started", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "3.0", key = "start/getting-started" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Indexes", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "3.0", key = "indexes/what-are-indexes" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Transformers", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "3.0", key = "transformers/what-are-transformers" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Client API", MVC.Docs.ActionNames.Client, MVC.Docs.Name, new { language = language, version = "3.0" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Server", MVC.Docs.ActionNames.Server, MVC.Docs.Name, new { language = language, version = "3.0" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Studio", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "3.0", key = "studio/accessing-studio" }, null)));

			builder.AppendLine("<li class='dropdown'>");
			builder.AppendLine("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>Other <span class='caret'></span></a>");
			builder.AppendLine("<ul class='dropdown-menu' role='menu'>");
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Samples", MVC.Docs.ActionNames.Samples, MVC.Docs.Name, new { language = language, version = "3.0" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Glossary", MVC.Docs.ActionNames.Glossary, MVC.Docs.Name, new { language = language, version = "3.0" }, null)));
			builder.AppendLine("</ul>");
			builder.AppendLine("</li>");

			builder.AppendLine("</ul>");

			return new MvcHtmlString(builder.ToString());
		}

		private static MvcHtmlString GenerateNavigationFor25(HtmlHelper htmlHelper, Language language)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<ul class='nav navbar-nav'>");

			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Intro", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "intro" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Theory", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "theory" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink(".NET Client API", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "client-api" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("HTTP API", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "http-api" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Server side", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "server" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Studio", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "studio" }, null)));

			builder.AppendLine("<li class='dropdown'>");
			builder.AppendLine("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>Other <span class='caret'></span></a>");
			builder.AppendLine("<ul class='dropdown-menu' role='menu'>");
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Appendixes", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "appendixes" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("FAQ", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "faq" }, null)));
			builder.AppendLine(string.Format("<li>{0}</li>", htmlHelper.ActionLink("Samples", MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { language = language, version = "2.5", key = "samples" }, null)));
			builder.AppendLine("</ul>");
			builder.AppendLine("</li>");

			builder.AppendLine("</ul>");

			return new MvcHtmlString(builder.ToString());
		}

		public static MvcHtmlString GenerateTableOfContents(this HtmlHelper htmlHelper, UrlHelper urlHelper, TableOfContents tableOfContents, string key)
		{
			Debug.Assert(tableOfContents != null);

			var mode = GetMode(tableOfContents.Version);

			var builder = new StringBuilder();
			builder.AppendLine("<div class='panel-group' id='sidebar'>");

			for (int index = 0; index < tableOfContents.Items.Count; index++)
			{
				var item = tableOfContents.Items[index];
				var containsKey = ContainsKey(item, key, 2);
				if (item.IsFolder)
				{
					var id = "collapse" + index;

					builder.AppendLine("<div class='panel panel-default'>");
					builder.AppendLine("<div class='panel-heading'>");
					builder.AppendLine("<h4 class='panel-title'>");

					switch (mode)
					{
						case DocumentationMode.Normal:
							builder.AppendLine(string.Format("<a data-toggle='collapse' data-parent='#sidebar' href='#{0}'>", id));
							break;
						case DocumentationMode.Legacy:
							builder.AppendLine(string.Format("<a href='{0}'>", urlHelper.Action(MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { version = tableOfContents.Version, language = Language.Csharp, key = item.Key })));
							break;
					}

					builder.AppendLine(string.Format("<span class='fa fa-folder-o'></span><span><strong>{0}</strong></span>", item.Title));
					builder.AppendLine("</a>");
					builder.AppendLine("</h4>");
					builder.AppendLine("</div>");

					builder.AppendLine(string.Format("<div id='{0}' class='panel-collapse collapse {1}'>", id, containsKey ? "in" : string.Empty));
					builder.AppendLine("<ul class='list-group'>");
					GenerateTableOfContents(htmlHelper, urlHelper, builder, item.Items, key, 0, tableOfContents.Version, mode);
					builder.AppendLine("</ul>");
					builder.AppendLine("</div>");
					builder.AppendLine("</div>");

					continue;
				}

				var link = htmlHelper.ActionLink(item.Title, MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { key = item.Key }, null).ToHtmlString();

				builder.AppendLine(string.Format("<div class='panel panel-default panel-article {0}'>", containsKey ? "selected" : string.Empty));
				builder.AppendLine("<div class='panel-heading'>");
				builder.AppendLine("<h4 class='panel-title'>");
				builder.AppendLine(string.Format("<span class='fa fa-file-text-o'></span>{0}", link));
				builder.AppendLine("</h4>");
				builder.AppendLine("</div>");
				builder.AppendLine("</div>");
			}

			builder.AppendLine("</div>");
			return new MvcHtmlString(builder.ToString());
		}

		private static void GenerateTableOfContents(HtmlHelper htmlHelper, UrlHelper urlHelper, StringBuilder builder, IEnumerable<TableOfContents.TableOfContentsItem> items, string key, int level, string version, DocumentationMode mode)
		{
			foreach (var item in items)
			{
				var containsKey = level == 0 && item.IsFolder && ContainsKey(item, key, 3);

				var css = string.Empty;
				var isCurrent = string.Equals(key, item.Key, StringComparison.OrdinalIgnoreCase);
				if (isCurrent)
					css += "selected ";

				if (item.IsFolder)
				{
					css += "folder ";
					if (containsKey)
						css += "in ";
					else
						css += "out ";
				}

				builder.AppendLine(string.Format("<li class='list-group-item {0}'>", css));

				if (item.IsFolder)
					GenerateForFolder(htmlHelper, urlHelper, builder, item, key, level, containsKey, version, mode);
				else
					GenerateForArticle(htmlHelper, builder, item);

				builder.AppendLine("</li>");
			}
		}

		private static void GenerateForArticle(HtmlHelper htmlHelper, StringBuilder builder, TableOfContents.TableOfContentsItem item)
		{
			var link = htmlHelper.ActionLink(item.Title, MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { key = item.Key }, null).ToHtmlString();
			builder.AppendLine(string.Format("<span class='fa fa-file-text-o'></span>{0}", link));
		}

		private static void GenerateForFolder(HtmlHelper htmlHelper, UrlHelper urlHelper, StringBuilder builder, TableOfContents.TableOfContentsItem item, string key, int level, bool containsKey, string version, DocumentationMode mode)
		{
			if (mode == DocumentationMode.Legacy)
				builder.AppendLine(string.Format("<a href='{0}'>", urlHelper.Action(MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { version = version, language = Language.Csharp, key = item.Key })));

			builder.AppendLine(string.Format("<span class='fa fa-folder {1}'></span><span>{0}</span>", item.Title, containsKey ? "text-danger" : string.Empty));

			if (mode == DocumentationMode.Legacy)
				builder.AppendLine("</a>");

			builder.AppendLine("<ul class='list-group'>");

			GenerateTableOfContents(htmlHelper, urlHelper, builder, item.Items, key, ++level, version, mode);

			builder.AppendLine("</ul>");
		}

		private static bool ContainsKey(TableOfContents.TableOfContentsItem item, string key, int minNumberOfPartsThatMustMatch)
		{
			if (string.IsNullOrEmpty(key))
				return false;

			var numberOfParts = 0;

			var itemKey = item.Key;
			for (var i = 0; i < Math.Min(itemKey.Length, key.Length); i++)
			{
				if (itemKey[i] != key[i])
					return false;

				if (key[i] == '/')
					numberOfParts++;

				if (numberOfParts >= minNumberOfPartsThatMustMatch)
					return true;

				if (i == itemKey.Length - 1)
					return true;
			}

			return false;
		}

		private static DocumentationMode GetMode(string version)
		{
			switch (version)
			{
				case "3.0":
					return DocumentationMode.Normal;
				default:
					return DocumentationMode.Legacy;
			}
		}

		private enum DocumentationMode
		{
			Normal,
			Legacy
		}
	}
}