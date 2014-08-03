namespace Raven.Documentation.Parser.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Raven.Documentation.Parser.Attributes;
	using Raven.Documentation.Parser.Data;

	public static class FileExtensionHelper
	{
		public static IDictionary<Language, string> GetLanguageFileExtensions()
		{
			return Enum.GetValues(typeof(Language))
				.Cast<Language>()
				.Where(x => x != Language.All)
				.ToDictionary(value => value, GetLanguageFileExtension);
		}

		public static string GetLanguageFileExtension(Language language)
		{
			var type = typeof(Language);
			var memInfo = type.GetMember(language.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(FileExtensionAttribute), false);
			return ((FileExtensionAttribute)attributes[0]).Extension;
		}
	}
}