using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Documentation.Parser.Data;

namespace Raven.Documentation.Web.Core.ViewModels
{
    public class DocPageViewModel : DocumentationViewModel
    {
        public DocPageViewModel(string version, Language language)
            : base(version, language)
        {
            this.AvailableLanguages = new List<Language>();
            this.AvailableVersions = new Dictionary<float, string>();
        }

        public DocumentationPage Page { get; set; }

        public List<Language> AvailableLanguages { get; private set; }

        public Dictionary<float, string> AvailableVersions { get; private set; }

        public IEnumerable<Language> GetLanguagesForDropDown()
        {
            if (this.AvailableLanguages.Contains(Language.All))
            {
                return new List<Language>(); //DocumentationLanguage.AllLangs.Where(lang => lang != Language.All && lang != this.Page.Language);
            }

            return DocumentationLanguage.AllLangs.Where(lang => lang != Language.All && lang != this.Page.Language && this.AvailableLanguages.Contains(lang));
        }
    }
}
