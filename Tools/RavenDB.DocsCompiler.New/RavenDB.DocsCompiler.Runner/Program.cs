// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hibernating Rhinos">
//   COPYRIGHT GOES HERE
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using RavenDB.DocsCompiler.Output;

namespace RavenDB.DocsCompiler.Runner
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method.  This is where the program execution begins
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Main(string[] args)
        {
            var outputType = OutputType.Html;
            if (args.Length > 0 && args[0].Equals("markdown", StringComparison.CurrentCultureIgnoreCase))
            {
                outputType = OutputType.Markdown;
            }

	        bool debugMode = args.Length > 1 && args[1].Equals("debug", StringComparison.InvariantCultureIgnoreCase);

	        var rootPath = Path.GetFullPath("./../../../../../");
            var documentationVersions = new List<string> { "version_3_0" };
            
            foreach (var documentationVersion in documentationVersions)
            {
				Generate(rootPath, documentationVersion, outputType, debugMode);
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Generates the documentation.
        /// </summary>
        /// <param name="rootPath">
        /// The root path of the documentation to generate.
        /// </param>
        /// <param name="clientVersion">
        /// The client language to generate.
        /// </param>
        /// <param name="version">
        /// The version of the documentation to generate.
        /// </param>
        /// <param name="outputType">
        /// The output type (HTML or Markdown).
        /// </param>
        private static void Generate(string rootPath, string version, OutputType outputType, bool debugMode)
        {
            var docsPath = Path.Combine(rootPath, version);

            var outputPath = CalculateOutputPath(outputType, docsPath);
            
            var output = CreateDocumentationOutputSpecification(rootPath, outputPath, outputType, version, debugMode);

            try
            {
                Console.WriteLine(
                    "Generating documentation using docsPath: {0} and output path {1}", docsPath, outputPath);
                Compiler.CompileFolder(output, docsPath, "Home", version);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.WriteLine("Done");
        }

        /// <summary>
        /// Calculates the output path for the documentation.
        /// </summary>
        /// <param name="outputType">
        /// The output type.
        /// </param>
        /// <param name="docsPath">
        /// The docs path.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string CalculateOutputPath(OutputType outputType, string docsPath)
        {
            var outputPath = Path.Combine(docsPath, "html-compiled");
            if (outputType == OutputType.Markdown)
            {
                outputPath = Path.Combine(docsPath, "markdown-compiled");
            }

            return outputPath;
        }

	    /// <summary>
	    /// Create documentation output specification.
	    /// </summary>
	    /// <param name="rootPath">
	    /// The root path.
	    /// </param>
	    /// <param name="outputPath">
	    /// The output path.
	    /// </param>
	    /// <param name="outputType">
	    /// The output type.
	    /// </param>
	    /// <param name="debugMode"></param>
	    /// <returns>
	    /// The <see cref="IDocsOutput"/>.
	    /// </returns>
	    private static IDocsOutput CreateDocumentationOutputSpecification(string rootPath, string outputPath, OutputType outputType, string version, bool debugMode)
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
                                             PageTemplate =
                                                 File.ReadAllText(
                                                     Path.Combine(rootPath, version, @"html-template.html")),
                                             RootUrl = "http://ravendb.net/docs/",
											 ImagesPath = debugMode ? "images/" : null
                                         };
                return output;
            }

            return null;
        }
    }
}
