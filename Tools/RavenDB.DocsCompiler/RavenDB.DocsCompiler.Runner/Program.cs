using System;
using System.IO;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.Runner
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var rootPath = Path.GetFullPath("./../../../../../");

			Generate(rootPath, "version_1");
			Generate(rootPath, "version_2");
		}

		private static void Generate(string rootPath, string version)
		{
			var docsPath = Path.Combine(rootPath, version);

			IDocsOutput output = new HtmlDocsOutput
			{
				OutputPath = Path.Combine(docsPath, "html-compiled"),
				PageTemplate = File.ReadAllText(Path.Combine(rootPath, @"Tools\html-template.html")),
				RootUrl = "http://ravendb.net/docs/",
			};

			try
			{
				Compiler.CompileFolder(output, docsPath, "Home", "2.0");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
