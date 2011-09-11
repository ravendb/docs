using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDB.DocsCompiler
{
	public enum Mode
	{
		FullHtml,
		OnsiteDocs,
	}

	public class Settings
	{
		public static string BasePath { get; set; }

		public static string DocsPath { get; set; }

		public static string CodeSamplesPath { get; set; }
	}
}
