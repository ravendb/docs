using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;

using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers
{
	public class DocumentationFileHelper
	{
		public static IEnumerable<FolderItem> ParseFile(string filePath)
		{
			var contents = File.ReadAllText(filePath);
			var docFiles = JsonConvert.DeserializeObject<List<DocumentationFile>>(contents);

			foreach (var file in docFiles)
			{
				var path = file.Path;
				var name = file.Name;
				var isFolder = path.StartsWith("/");
				var item = new FolderItem(isFolder)
				{
					Language = Language.All,
					Description = name,
					Name = isFolder ? path.Substring(1, path.Length - 1) : path.Substring(0, path.Length - Constants.MarkdownFileExtension.Length),
					Mappings = file.Mappings
				};

				yield return item;
			}
		}
	}
}