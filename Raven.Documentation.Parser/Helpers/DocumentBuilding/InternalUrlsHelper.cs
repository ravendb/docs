using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers.DocumentBuilding
{
    public class InternalUrlsHelper
    {
        private static readonly List<string> InternalSites = new List<string>
        {
            "ravendb.net",
            "ayende.com"
        };

        private static readonly Lazy<List<Regex>> InternalUrlFinders = new Lazy<List<Regex>>(CreateFinders);

        private static List<Regex> CreateFinders()
        {
            var finders = new List<Regex>();

            foreach (var site in InternalSites)
            {
                var finder = CreateFinder(site);
                finders.Add(finder);
            }

            return finders;
        }

        private static Regex CreateFinder(string site) => new Regex($@"http://{site}", RegexOptions.Compiled);

        public static string ConvertHttpToHttps(string content)
        {
            var finders = InternalUrlFinders.Value;

            foreach (var finder in finders)
            {
                content = finder.Replace(content, match => ReplaceHttpWithHttps(match.Value));
            }

            return content;
        }

        private static string ReplaceHttpWithHttps(string match) => match.Replace("http:", "https:");
    }
}
