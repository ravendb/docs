namespace Raven.Documentation.Web.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Dynamic;
	using System.Text;
	using System.Web.Mvc;
	using System.Web.Mvc.Html;
	using System.Web.Routing;
	using System.Web.UI.WebControls;

	using Raven.Documentation.Parser.Data;

	public static class HtmlHelperExtensions
	{
		public static bool IsDebug(this HtmlHelper htmlHelper)
		{
#if DEBUG
			return true;
#else 
			return false;
#endif
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

		public static MvcHtmlString GenerateTableOfContents(this HtmlHelper htmlHelper, TableOfContents tableOfContents, string key)
		{
			Debug.Assert(tableOfContents != null);

			var builder = new StringBuilder();
			builder.AppendLine("<div class='panel-group' id='sidebar'>");

			for (int index = 0; index < tableOfContents.Items.Count; index++)
			{
				var item = tableOfContents.Items[index];
				if (item.IsFolder)
				{
					var id = "collapse" + index;

					builder.AppendLine("<div class='panel panel-default'>");
					builder.AppendLine("<div class='panel-heading'>");
					builder.AppendLine("<h4 class='panel-title'>");
					builder.AppendLine(string.Format("<a data-toggle='collapse' data-parent='#sidebar' href='#{0}'>", id));
					builder.AppendLine(string.Format("<span class='fa fa-folder-o'></span><span>{0}</span>", item.Title));
					builder.AppendLine("</a>");
					builder.AppendLine("</h4>");
					builder.AppendLine("</div>");

					var containsKey = ContainsKey(item, key, 2);

					builder.AppendLine(string.Format("<div id='{0}' class='panel-collapse collapse {1}'>", id, containsKey ? "in" : string.Empty));
					builder.AppendLine("<ul class='list-group'>");
					GenerateTableOfContents(htmlHelper, builder, item.Items, key, 0);
					builder.AppendLine("</ul>");
					builder.AppendLine("</div>");
					builder.AppendLine("</div>");

					continue;
				}

				var link = htmlHelper.ActionLink(item.Title, MVC.Docs.ActionNames.Articles, MVC.Docs.Name, new { key = item.Key }, null).ToHtmlString();

				builder.AppendLine("<div class='panel panel-default'>");
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

		private static void GenerateTableOfContents(HtmlHelper htmlHelper, StringBuilder builder, IEnumerable<TableOfContents.TableOfContentsItem> items, string key, int level)
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
					GenerateForFolder(htmlHelper, builder, item, key, level, containsKey);
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

		private static void GenerateForFolder(HtmlHelper htmlHelper, StringBuilder builder, TableOfContents.TableOfContentsItem item, string key, int level, bool containsKey)
		{
			builder.AppendLine(string.Format("<span class='fa fa-folder {1}'></span><span>{0}</span>", item.Title, containsKey ? "text-danger" : string.Empty));
			builder.AppendLine("<ul class='list-group'>");

			GenerateTableOfContents(htmlHelper, builder, item.Items, key, ++level);

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
	}
}