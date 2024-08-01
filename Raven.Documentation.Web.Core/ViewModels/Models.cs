using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Web.Core.ViewModels
{
    public class TableOfContentsViewModel
    {
        public DocsVersion.DocsMode Mode { get; set; }

        private readonly List<Language> langs;

        public TableOfContentsViewModel(TableOfContents fromDb, DocsVersion.DocsMode mode)
        {
            Mode = mode;
            this.Key = PrefixHelper.GetCategoryPrefix(fromDb.Category);
            this.Items = new List<TableOfContentsViewModel>();
            this.IsFolder = true;
            this.Title = fromDb.Category.GetDescription();
            this.AddChildren(fromDb.Items);
        }

        public TableOfContentsViewModel(TableOfContents.TableOfContentsItem itemFromDb, DocsVersion.DocsMode mode)
        {
            Mode = mode;
            this.Key = itemFromDb.Key;
            this.Items = new List<TableOfContentsViewModel>();
            this.Title = itemFromDb.Title;
            this.IsFolder = itemFromDb.IsFolder;
            this.langs = itemFromDb.Languages;
            this.AddChildren(itemFromDb.Items);
        }

        private void AddChildren(IEnumerable<TableOfContents.TableOfContentsItem> items)
        {
            foreach (var item in items)
            {
                this.Items.Add(new TableOfContentsViewModel(item, Mode));
            }
        }

        public string Key { get; set; }

        public string Title { get; set; }

        public bool IsFolder { get; set; }

        public IEnumerable<Language> Langs
        {
            get
            {
                if (this.IsFolder)
                {
                    IEnumerable<Language> langsCollected = new List<Language>();
                    langsCollected = this.Items.Aggregate(langsCollected, (current, item) => current.Union(item.Langs));
                    return langsCollected;
                }

                return this.langs;
            }
        }

        public List<TableOfContentsViewModel> Items { get; set; }

        public string GetCssClass()
        {
            if (this.IsFolder)
            {
                return string.Join(" ", this.Langs.Select(x => "has-" + DocumentationLanguage.GetClientName(x)));
            }

            return string.Join(" ", this.Langs.Select(DocumentationLanguage.ToCssClass));
        }

        public bool IsActive(string currentKey)
        {
            if (string.IsNullOrWhiteSpace(Key) || string.IsNullOrWhiteSpace(currentKey))
                return false;

            return IsFolder ? currentKey.StartsWith(Key.ToLowerInvariant()) : currentKey == Key;
        }
    }

    public class DocsVersion
    {
        public static readonly List<string> AllVersions = new List<string>
        {
            "6.1",
            "6.0",
            "5.4",
            "5.3",
            "5.2",
            "5.1",
            "5.0",
            "4.2",
            "4.1",
            "4.0",
            "3.5",
            "3.0",
            "2.5",
            "2.0",
            "1.0"
        };

        private static List<float> _allVersionsAsFloat;
        public static List<float> AllVersionsAsFloat
        {
            get
            {
                if (_allVersionsAsFloat == null)
                    _allVersionsAsFloat = AllVersions.Select(float.Parse).ToList();

                return _allVersionsAsFloat;
            }
        }

        public const string Default = "6.0";

        public enum DocsMode
        {
            Normal,
            Legacy
        }

        public static DocsMode GetModeForVersion(string version)
        {
            switch (version)
            {
                case "1.0":
                case "2.0":
                case "2.5":
                    return DocsMode.Legacy;
                default:
                    return DocsMode.Normal;
            }
        }

        public static string Parse(string version)
        {
            return AllVersions.Contains(version) ? version : Default;
        }
    }

    public class DocumentationLanguage
    {
        public const Language Default = Language.Csharp;

        public static IEnumerable<Language> AllLangs
        {
            get
            {
                return Enum.GetValues(typeof(Language)).Cast<Language>();
            }
        }

        public static Language Parse(string language)
        {
            Language result;
            if (Enum.TryParse(language, true, out result))
            {
                return result;
            }

            return Default;
        }

        public static string GetDescription(string lang)
        {
            var lng = Parse(lang);
            return lng.GetDescription();
        }

        public static string GetClientName(Language lang)
        {
            switch (lang)
            {
                case Language.Csharp:
                    return "dotnet";
                case Language.Java:
                    return "java";
                case Language.Http:
                    return "http";
                case Language.Python:
                    return "python";
                case Language.Php:
                    return "php";
                default:
                    return "general";
            }
        }

        public static string ToCssClass(Language lang)
        {
            return string.Concat("g-", GetClientName(lang));
        }
    }
}
