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
			Generate(rootPath, "version_2_5");
		}

		private static void Generate(string rootPath, string version)
		{
			var docsPath = Path.Combine(rootPath, version);
			var outputPath = Path.Combine(docsPath, "html-compiled");

			IDocsOutput output = new HtmlDocsOutput
			{
				OutputPath = outputPath,
				PageTemplate = File.ReadAllText(Path.Combine(rootPath, @"Tools\html-template.html")),
				RootUrl = "http://ravendb.net/docs/",
			};

			try
			{
				Console.WriteLine("Generating documentation using docsPath: {0} and output path {1}", docsPath, outputPath);
				Compiler.CompileFolder(output, docsPath, "Home", "2.0");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			Console.WriteLine("Done");

		}
	}
}
