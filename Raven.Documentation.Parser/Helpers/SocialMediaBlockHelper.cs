using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Raven.Documentation.Parser.Helpers
{
    public class SocialMediaBlockHelper
    {
		private static readonly Regex SocialMediaFollowRegex = new Regex(@"{SOCIAL-MEDIA-FOLLOW?\s*/}", RegexOptions.Compiled);

        private static string _socialMediaFollowSectionHtml;

        private static string SocialMediaFollowSectionHtml
        {
            get
            {
                if (_socialMediaFollowSectionHtml == null)
                {
                    _socialMediaFollowSectionHtml = LoadSocialMediaFollowHtml();
                }

                return _socialMediaFollowSectionHtml;
            }
        }

        private const string SocialMediaSectionHtmlResourceName =
            "Raven.Documentation.Parser.Assets.social-media-follow.html";

        public static string ReplaceSocialMediaBlocks(string content)
        {
            content = SocialMediaFollowRegex.Replace(content, match => SocialMediaFollowSectionHtml);
            return content;
        }

        private static string LoadSocialMediaFollowHtml()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(SocialMediaSectionHtmlResourceName)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
