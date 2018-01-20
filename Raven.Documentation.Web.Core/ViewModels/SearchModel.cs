using System.Globalization;
using System.Linq;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Core.ViewModels
{
    public class ArticleSearchResult
    {
        public string Title { get; set; }

        public string Key { get; set; }

        public string Category { get; set; }
    
        public string Version { get; set; }

        public Language Language { get; set; }
    }

    public class SuggestItem
    {
        public string Title { get; set; }

        public string Key { get; set; }

        public string Version { get; set; }

        public string Language { get; set; }

        public string Path
        {
            get
            {
                TextInfo ti = CultureInfo.CurrentCulture.TextInfo;
                var pathLevels = this.Key.Split('/').Select(x => ti.ToTitleCase(x.Replace('-', ' '))).ToArray();
                if (pathLevels.Length == 1)
                {
                    return string.Empty;
                }

                return string.Join(" / ", pathLevels.Take(pathLevels.Length - 1));
            }
        }
    }
}
