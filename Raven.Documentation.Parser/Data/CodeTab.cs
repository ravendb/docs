using System;

namespace Raven.Documentation.Parser.Data
{
    public abstract class CodeTabBase
    {
        public string Content { get; set; }

        public string Title { get; set; }

        public string Id { get; set; }

        public abstract string GetLanguageCssClass();

        public abstract string GetLanguageDisplayName();
    }

    public class CodeTab : CodeTabBase
    {
        private readonly Func<Language, string> _languageToCssClass;
        private readonly Func<Language, string> _languageToDisplayName;

        public CodeTab(Func<Language, string> languageToCssClass, Func<Language, string> languageToDisplayName)
        {
            _languageToCssClass = languageToCssClass;
            _languageToDisplayName = languageToDisplayName;
        }

        public Language Language { get; set; }

        public override string GetLanguageCssClass()
        {
            return _languageToCssClass(Language);
        }

        public override string GetLanguageDisplayName()
        {
            return _languageToDisplayName(Language);
        }
    }

    public class CodeTabBlock : CodeTabBase
    {
        private readonly Func<CodeBlockLanguage, string> _languageToCssClass;
        private readonly Func<CodeBlockLanguage, string> _languageToDisplayName;

        public CodeTabBlock(Func<CodeBlockLanguage, string> languageToCssClass, Func<CodeBlockLanguage, string> languageToDisplayName)
        {
            _languageToCssClass = languageToCssClass;
            _languageToDisplayName = languageToDisplayName;
        }

        public CodeBlockLanguage Language { get; set; }

        public override string GetLanguageCssClass()
        {
            return _languageToCssClass(Language);
        }

        public override string GetLanguageDisplayName()
        {
            return _languageToDisplayName(Language);
        }
    }
}
