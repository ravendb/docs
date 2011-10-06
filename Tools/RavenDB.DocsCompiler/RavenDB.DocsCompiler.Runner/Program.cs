using System;
using System.IO;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.Runner
{
	class Program
	{
		static void Main(string[] args)
		{
			Settings.BasePath = @"z:\Projects\RavenDB\RavenDB-docs\";
			Settings.CodeSamplesPath = @"z:\Projects\RavenDB\RavenDB-docs\code-samples\";
			Settings.DocsPath = @"z:\Projects\RavenDB\RavenDB-docs\docs\";

			IDocsOutput output = new HtmlDocsOutput
									{
										OutputPath = Path.Combine(Settings.BasePath, "html-compiled"),
										PageTemplate = File.ReadAllText(Path.Combine(Settings.BasePath, @"Tools\html-template.html")),
									};

			try
			{
				Compiler.CompileFolder(output, Settings.DocsPath, "Home");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
