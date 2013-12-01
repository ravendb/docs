// -----------------------------------------------------------------------
//  <copyright file="Compiler.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using RavenDB.DocsCompiler.MagicWorkers;
using RavenDB.DocsCompiler.Model;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler
{
	/// <summary>
	/// The output type.
	/// </summary>
	public enum OutputType
	{
		/// <summary>
		/// HTML output.
		/// </summary>
		Html,

		/// <summary>
		/// Markdown output.
		/// </summary>
		Markdown
	}

	/// <summary>
	/// The Client Type
	/// </summary>
	public enum ClientType
	{
		/// <summary>
		/// Java client
		/// </summary>
		None,

		/// <summary>
		/// Java client
		/// </summary>
		Java,

		/// <summary>
		/// csharp
		/// </summary>
		Csharp,

		/// <summary>
		/// HTTP
		/// </summary>
		Http
	}

	/// <summary>
	/// The documentation compiler.
	/// </summary>
	public class Compiler
	{
		/// <summary>
		/// The file name representing a documentation list.
		/// </summary>
		private const string DocsListFileName = ".docslist";

		/// <summary>
		/// The destination full path.
		/// </summary>
		private readonly string destinationFullPath;

		/// <summary>
		/// Initializes a new instance of the <see cref="Compiler"/> class.
		/// </summary>
		/// <param name="destinationFullPath">
		/// The full path.
		/// </param>
		private Compiler(string destinationFullPath)
		{
			this.destinationFullPath = destinationFullPath;

			SupportedLanguages = new List<ClientType>
		    {
			    ClientType.Csharp,
			    ClientType.Http,
			    ClientType.Java
		    };
		}

		/// <summary>
		/// Gets or sets the output.
		/// </summary>
		public IDocsOutput Output { get; set; }

		public IList<ClientType> SupportedLanguages { get; private set; }

		/// <summary>
		/// Gets a value indicating whether to convert to html.
		/// </summary>
		public bool ConvertToHtml
		{
			get
			{
				return this.Output.ContentType == OutputType.Html;
			}
		}

		public string Brush
		{
			get
			{
				switch (Output.ClientType)
				{
					case ClientType.Csharp:
						return "csharp";
					case ClientType.Java:
						return "java";
					case ClientType.Http:
						return "plain";
					default:
						return null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the C# code samples path.
		/// </summary>
		public string CsharpCodeSamplesPath { get; set; }

		/// <summary>
		/// Gets or sets the java code samples path.
		/// </summary>
		public string JavaCodeSamplesPath { get; set; }

		public string CodeSamplesPath
		{
			get { return Output.ClientType == ClientType.Csharp ? CsharpCodeSamplesPath : JavaCodeSamplesPath; }
		}

		/// <summary>
		/// Gets or sets the root folder.
		/// </summary>
		public Folder RootFolder { get; protected set; }

		public static void CompileFolder(IDocsOutput output, string fullPath, string homeTitle, string versionUrl)
		{
			if (output == null)
				throw new ArgumentNullException("output");

			var compiler = CreateDocumentationCompiler(output, fullPath);

			compiler.ParseDocumentation(compiler.RootFolder = new Folder
			{
				Title = homeTitle,
				Trail = string.Empty,
				VirtualTrail = string.Empty
			}, versionUrl, ClientType.None);

			compiler.CompileDocumentation(compiler.RootFolder, versionUrl, ClientType.None);
		}

		private void CompileDocumentation(Folder folder, string versionUrl, ClientType currentLanguage)
		{
			var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
			var fullPath = Path.Combine(this.destinationFullPath, fullFolderSlug);

			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			var languagesToProces = new List<ClientType>();
			if (folder.Multilanguage)
			{
				languagesToProces.AddRange(SupportedLanguages);
			}
			else
			{
				languagesToProces.Add(currentLanguage);
			}

			foreach (var language in languagesToProces)
			{
				if (ConvertToHtml)
					CompileAsHtml(folder, versionUrl, fullPath, fullFolderSlug, language);
			}
		}

		private void CompileAsHtml(Folder folder, string versionUrl, string fullPath, string fullFolderSlug, ClientType currentLanguage)
		{
			foreach (var child in folder.Children)
			{
				var document = child as Document;
				if (document != null)
					CompileDocument(versionUrl, document, fullPath, currentLanguage);

				var subFolder = child as Folder;
				if (subFolder != null)
					CompileDocumentation(subFolder, versionUrl, currentLanguage);
			}

			CopyImages(folder, fullPath);
		}

		private void ParseDocumentation(Folder folder, string versionUrl, ClientType currentLanguage)
		{
			var fullFolderSlug = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
			var fullPath = Path.Combine(this.destinationFullPath, fullFolderSlug);

			if (!File.Exists(Path.Combine(fullPath, DocsListFileName)))
				return;

			var languagesToProces = new List<ClientType>();
			if (folder.Multilanguage)
			{
				languagesToProces.AddRange(SupportedLanguages);
			}
			else
			{
				languagesToProces.Add(currentLanguage);
			}

			foreach (var language in languagesToProces)
			{
				var docs = DocsListParser.Parse(Path.Combine(fullPath, DocsListFileName), folder).ToArray();
				foreach (var item in docs)
				{
					item.Language = language;

					if (item.Slug != null)
						item.Slug = item.Slug.TrimStart('\\', '/');

					item.Trail = Path.Combine(folder.Trail, folder.Slug ?? string.Empty);
					item.VirtualTrail = Path.Combine(folder.VirtualTrail, folder.Slug ?? string.Empty);

					if (language != ClientType.None && !item.VirtualTrail.Contains(language.ToString()))
					{
						item.VirtualTrail = Path.Combine(item.VirtualTrail, language.ToString());
					}

					var document = item as Document;
					if (document != null)
						continue;

					var subFolder = item as Folder;
					if (subFolder != null)
						ParseDocumentation(subFolder, versionUrl, language);
				}
			}
		}

		private static Compiler CreateDocumentationCompiler(IDocsOutput output, string fullPath)
		{
			return new Compiler(Path.Combine(fullPath, "docs"))
					   {
						   Output = output,
						   CsharpCodeSamplesPath =
							   Path.Combine(fullPath, "code-samples"),
						   JavaCodeSamplesPath =
							   Path.Combine(fullPath, "java-code-samples/src/test/java/net/ravendb")
					   };
		}

		private void CopyImages(Folder folder, string fullPath)
		{
			// Copy images
			var imagesPath = Path.Combine(fullPath, "images");
			if (!Directory.Exists(imagesPath))
				return;

			var images = Directory.GetFiles(imagesPath);
			foreach (var image in images)
			{
				var imageFileName = Path.GetFileName(image);
				if (imageFileName == null)
				{
					continue;
				}

				this.Output.SaveImage(folder, image);
			}
		}

		private string CompileDocument(string versionUrl, Document document, string fullPath, ClientType language)
		{
			var strippedSlug = document.Slug.Replace(".markdown", string.Empty);

			string path = Path.Combine(fullPath, strippedSlug + "." + language + ".markdown");
			if (!File.Exists(path))
			{
				// fall back to plain mode
				path = Path.Combine(fullPath, document.Slug);
			}

			if (!File.Exists(path))
			{
				return CompileNotDocumented(versionUrl, document, strippedSlug, fullPath, language);
			}

			document.Content = DocumentationParser.Parse(this, null, path, document.Trail, versionUrl);
			document.Slug = strippedSlug;
			this.Output.SaveDocItem(document);
			return document.Content;
		}

		private string CompileNotDocumented(string versionUrl, Document document, string strippedSlug, string fullPath, ClientType currentLanguage)
		{
			var builder = new StringBuilder();
			builder.Append("<ul>");
			foreach (var language in SupportedLanguages.Where(x => x != currentLanguage))
			{
				var path = Path.Combine(fullPath, strippedSlug + "." + language + ".markdown");
				if (File.Exists(path))
				{
					var url = "/"
						+ document.VirtualTrail
						.Replace(currentLanguage.ToString(), language.ToString())
						.Replace("\\", "/")
						+ "/" + strippedSlug + ".html";

					builder.AppendFormat("<li><a href='{0}'>{1}</a></li>", url, language);
				}
			}
			builder.Append("</ul>");

			var pathToNotDocumented = Path.Combine(destinationFullPath, "not-documented.markdown");
			document.Content = DocumentationParser.Parse(this, null, pathToNotDocumented, document.Trail, versionUrl);
			document.Content = string.Format(document.Content, currentLanguage, builder);
			document.Slug = strippedSlug;

			Output.SaveDocItem(document);

			return document.Content;
		}
	}
}
