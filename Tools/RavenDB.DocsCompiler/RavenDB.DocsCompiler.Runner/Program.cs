using System;
using System.IO;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.Runner
{
	class Program
	{
		static void Main(string[] args)
		{
			const string basePath = @"z:\Projects\RavenDB\RavenDB-docs\";

			IDocsOutput output = new HtmlDocsOutput
									{
										OutputPath = Path.Combine(basePath, "html-compiled"),
										PageTemplate = File.ReadAllText(Path.Combine(basePath, @"..\Tools\html-template.html")),
										RootUrl = "http://ravendb.net/docs/",
									};

			try
			{
				Compiler.CompileFolder(output, basePath, "Home");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
