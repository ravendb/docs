﻿namespace Raven.Documentation.Parser.Data
{
	using System.ComponentModel;

	using Raven.Documentation.Parser.Attributes;

	public enum Language
	{
		[FileExtension(".dotnet")]
		[Description("C#")]
		Csharp,

		[FileExtension(".java")]
		[Description("Java")]
		Java,

		[FileExtension(".http")]
		[Description("HTTP")]
		Http,

        [FileExtension(".py")]
        [Description("Python")]
        Python,

	    [FileExtension(".js")]
	    [Description("Node.js")]
	    NodeJs,

        [FileExtension("")]
		[Description("General")]
		All
	}
}
