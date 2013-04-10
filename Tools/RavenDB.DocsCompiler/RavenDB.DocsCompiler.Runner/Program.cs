using System;
using System.Collections.Generic;
using System.IO;
using RavenDB.DocsCompiler;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.Runner
{
	public class Program
	{
		public static void Main(string[] args)
		{
		    var outputType = OutputType.Html;
		    if (args.Length > 0 && args[0].Equals("markdown", StringComparison.CurrentCultureIgnoreCase))
		        outputType = OutputType.Markdown;
			var rootPath = Path.GetFullPath("./../../../../../");
		    var documentationVersions = new List<String> {"version_1", "version_2", "version_2_5"};
		    foreach (var documentationVersion in documentationVersions)
		    {
		        Generate(rootPath, documentationVersion, outputType);
		    }			
		}

		private static void Generate(string rootPath, string version, OutputType outputType)
		{
			var docsPath = Path.Combine(rootPath, version);

            var outputPath = calculateOutputPath(outputType, docsPath);
		    var output = CreateDocumentationOutputSpecification(rootPath, outputPath, outputType);

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

	    private static string calculateOutputPath(OutputType outputType, string docsPath)
	    {
	        var outputPath = Path.Combine(docsPath, "html-compiled");
	        if (outputType == OutputType.Markdown)
	        {
	            outputPath = Path.Combine(docsPath, "markdown-compiled");
	        }
	        return outputPath;
	    }

	    private static IDocsOutput CreateDocumentationOutputSpecification(string rootPath, string outputPath, OutputType outputType)
	    {
            if (outputType == OutputType.Markdown)
            {
                return new MarkdownDocsOutput
                {
                    ContentType = OutputType.Markdown,
                    OutputPath = outputPath,
                    RootUrl = "http://ravendb.net/docs/",

                };    
            }
            if (outputType == OutputType.Html)
            {
                IDocsOutput output = new HtmlDocsOutput
                {
                    ContentType = OutputType.Html,
                    OutputPath = outputPath,
                    PageTemplate = File.ReadAllText(Path.Combine(rootPath, @"Tools\html-template.html")),
                    RootUrl = "http://ravendb.net/docs/",
                };
                return output;    
            }
	        return null;
	    }
	}
}
