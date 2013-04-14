// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Hibernating Rhinos">
//   COPYRIGHT GOES HERE
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace RavenDB.DocsCompiler.Runner
{
    

    using RavenDB.DocsCompiler.Output;

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

            var rootPath = Path.GetFullPath("./../../../../../");
            var documentationVersions = new List<string> { "version_1", "version_2", "version_2_5" };

            foreach (var documentationVersion in documentationVersions)
            {
                Generate(rootPath, documentationVersion, outputType);
            }
        }

        /// <summary>
        /// Generates the documentation.
        /// </summary>
        /// <param name="rootPath">
        /// The root path of the documentation to generate.
        /// </param>
        /// <param name="version">
        /// The version of the documentation to generate.
        /// </param>
        /// <param name="outputType">
        /// The output type (HTML or Markdown).
        /// </param>
        private static void Generate(string rootPath, string version, OutputType outputType)
        {
            var docsPath = Path.Combine(rootPath, version);

            var outputPath = CalculateOutputPath(outputType, docsPath);
            var output = CreateDocumentationOutputSpecification(rootPath, outputPath, outputType);

            try
            {
                Console.WriteLine(
                    "Generating documentation using docsPath: {0} and output path {1}", docsPath, outputPath);
                Compiler.CompileFolder(output, docsPath, "Home", "2.0");
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
        /// <returns>
        /// The <see cref="IDocsOutput"/>.
        /// </returns>
        private static IDocsOutput CreateDocumentationOutputSpecification(
            string rootPath, string outputPath, OutputType outputType)
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
                                                     Path.Combine(rootPath, @"Tools\html-template.html")),
                                             RootUrl = "http://ravendb.net/docs/",
                                         };
                return output;
            }

            return null;
        }
    }
}
