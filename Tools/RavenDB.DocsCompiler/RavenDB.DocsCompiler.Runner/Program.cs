using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RavenDB.DocsCompiler.Runner
{
	class Program
	{
		static void Main(string[] args)
		{
			Settings.BasePath = @"z:\Projects\RavenDB\RavenDB-docs\";
			Settings.CodeSamplesPath = @"z:\Projects\RavenDB\RavenDB-docs\code-samples\";
			Settings.DocsPath = @"z:\Projects\RavenDB\RavenDB-docs\docs\";

			Compiler.CompileFolder("Home", Settings.DocsPath, Path.Combine(Settings.BasePath, "html-compiled"));
		}
	}
}
