using System.IO;

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

	    public static string GetMarkdownFileName(FolderItem item)
	    {
	        var languageExtension = GetLanguageFileExtension(item.Language);
	        return $"{item.Name}{languageExtension}{Constants.MarkdownFileExtension}";
	    }

	    public static string GetMarkdownFilePath(string directory, FolderItem item)
	    {
	        var fileName = GetMarkdownFileName(item);
	        return Path.Combine(directory, fileName);
	    }

        public static IEnumerable<FolderItemLanguageEntry> GetItemsForLanguages(string directory, FolderItem item)
        {
            var genericFileName = item.Name + Constants.MarkdownFileExtension;
            var path = Path.Combine(directory, genericFileName);
            if (File.Exists(path))
            {
                yield return FolderItemLanguageEntry.Create(Language.All, genericFileName, item);
                yield break;
            }

            var languageFileExtensions = GetLanguageFileExtensions();

            foreach (var language in languageFileExtensions.Keys)
            {
                var extension = languageFileExtensions[language];
                var fileNameForLang = item.Name + extension + Constants.MarkdownFileExtension;
                path = Path.Combine(directory, fileNameForLang);

                if (!File.Exists(path))
                    continue;

                var folderItem = new FolderItem(item) { Language = language };

                yield return FolderItemLanguageEntry.Create(language, fileNameForLang, folderItem);
            }
        }

	    public class FolderItemLanguageEntry
	    {
	        public Language Language { get; set; }
	        public string FileName { get; set; }
	        public FolderItem Item { get; set; }

	        public static FolderItemLanguageEntry Create(Language language, string fileName, FolderItem item)
	        {
	            return new FolderItemLanguageEntry
	            {
	                Language = language,
	                FileName = fileName,
	                Item = item
	            };
	        }
        }
	}
}
