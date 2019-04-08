using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Parser.Helpers
{
    public class DocumentationFileHelper
    {
        public static IEnumerable<FolderItem> ParseFile(string filePath)
        {
            var docFiles = DeserializeFile(filePath);

            foreach (var file in docFiles)
            {
                var path = file.Path;
                var name = file.Name;
                var isFolder = IsFolder(file);

                var item = new FolderItem(isFolder)
                {
                    Language = Language.All,
                    Description = name,
                    Name = isFolder ? path.Substring(1, path.Length - 1) : path.Substring(0, path.Length - Constants.MarkdownFileExtension.Length),
                    LastSupportedVersion = file.LastSupportedVersion,
                    DiscussionId = file.DiscussionId,
                    Mappings = file.Mappings,
                    Metadata = file.Metadata,
                    SeoMetaProperties = file.SeoMetaProperties
                };

                yield return item;
            }
        }

        private static bool IsFolder(DocumentationFile docFile) => docFile.Path.StartsWith("/");

        public static void AssignDiscussionIdIfNeeded(string filePath, FolderItem itemToCheck, string discussionId = null)
        {
            var docFiles = DeserializeFile(filePath);
            var overwriteNeeded = false;

            var docFilesToUpdate = docFiles.Where(x => x.Name == itemToCheck.Description);

            foreach (var item in docFilesToUpdate)
            {
                if (IsFolder(item) || string.IsNullOrEmpty(item.DiscussionId) == false)
                    continue;

                item.DiscussionId = discussionId ?? Guid.NewGuid().ToString();
                overwriteNeeded = true;
            }

            if (overwriteNeeded)
                SerializeFile(filePath, docFiles);
        }

        private static List<DocumentationFile> DeserializeFile(string filePath)
        {
            try
            {
                var contents = File.ReadAllText(filePath);
                var docFiles = JsonConvert.DeserializeObject<List<DocumentationFile>>(contents);
                return docFiles;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Could not parse file: " + filePath, e);
            }
        }

        private static void SerializeFile(string filePath, List<DocumentationFile> items)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };

                var json = JsonConvert.SerializeObject(items, Formatting.Indented, serializerSettings);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Could not save file: {filePath}", e);
            }
        }

        private static readonly Regex DocsListLine = new Regex(@"^([\w\-/\.]{2,})\t(.+)$", RegexOptions.Compiled | RegexOptions.Multiline);

        // used to convert .docslist files to .docs.json ones which support mappings
        public static void CovertDocslistFileToNewFormatWithMappings()
        {
            string folder = @"C:\workspaces\HIRS\ravendb_docs\Documentation\3.0\Raven.Documentation.Pages";

            if (string.IsNullOrEmpty(folder))
                return;

            if (Directory.Exists(folder) == false)
                throw new DirectoryNotFoundException();

            var docslistFiles = Directory.GetFiles(folder, ".docslist", SearchOption.AllDirectories);

            foreach (var docListFilePath in docslistFiles)
            {
                var converted = new List<DocumentationFile>();
                var content = File.ReadAllText(docListFilePath);

                var matches = DocsListLine.Matches(content);

                foreach (Match match in matches)
                {
                    var fileName = match.Groups[1].Value.Trim();
                    var description = match.Groups[2].Value.Trim();

                    converted.Add(new DocumentationFile()
                    {
                        Path = fileName,
                        Name = description
                    });
                }

                var directoryName = Path.GetDirectoryName(docListFilePath);

                using (var convertedFile = File.Open(Path.Combine(directoryName, ".docs.json"), FileMode.OpenOrCreate))
                using (var streamWriter = new StreamWriter(convertedFile))
                {
                    JsonSerializer.Create(new JsonSerializerSettings()
                    {
                        Formatting = Formatting.Indented
                    }).Serialize(streamWriter, converted);

                    convertedFile.Flush();
                }

                File.Delete(docListFilePath);
            }
        }
    }
}
