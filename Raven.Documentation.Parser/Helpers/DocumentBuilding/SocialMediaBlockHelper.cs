using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Raven.Documentation.Parser.Helpers.DocumentBuilding
{
    public class SocialMediaBlockHelper
    {
		private static readonly Regex SocialMediaFollowRegex = new Regex(@"{SOCIAL-MEDIA-FOLLOW?\s*/}", RegexOptions.Compiled);

		private static readonly Regex SocialMediaLikeRegex = new Regex(@"{SOCIAL-MEDIA-LIKE?\s*/}", RegexOptions.Compiled);

        private static string _socialMediaLikeSectionHtml;

        private static string _socialMediaFollowSectionHtml;

        private static string SocialMediaFollowSectionHtml
        {
            get
            {
                if (_socialMediaFollowSectionHtml == null)
                {
                    _socialMediaFollowSectionHtml = LoadEmbeddedResource(SocialMediaFollowSectionHtmlResourceName);
                }

                return _socialMediaFollowSectionHtml;
            }
        }

        private static string SocialMediaLikeSectionHtml
        {
            get
            {
                if (_socialMediaLikeSectionHtml == null)
                {
                    _socialMediaLikeSectionHtml = LoadEmbeddedResource(SocialMediaLikeSectionHtmlResourceName);
                }

                return _socialMediaLikeSectionHtml;
            }
        }

        private const string SocialMediaFollowSectionHtmlResourceName =
            "Raven.Documentation.Parser.Assets.social-media-follow.html";

        private const string SocialMediaLikeSectionHtmlResourceName =
            "Raven.Documentation.Parser.Assets.social-media-like.html";

        public static string ReplaceSocialMediaBlocks(string content, string expectedPageUrl)
        {
            content = SocialMediaFollowRegex.Replace(content, match => SocialMediaFollowSectionHtml);

            if (expectedPageUrl != null)
            {
                var likeSection = SocialMediaLikeSectionHtml.Replace("{{URL}}", expectedPageUrl);
                content = SocialMediaLikeRegex.Replace(content, match => likeSection);
            }

            return content;
        }

        private static string LoadEmbeddedResource(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(name)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
