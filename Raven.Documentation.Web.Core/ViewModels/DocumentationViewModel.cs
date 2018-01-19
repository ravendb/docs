using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Documentation.Parser.Data;
using Raven.Documentation.Parser.Helpers;

namespace Raven.Documentation.Web.Core.ViewModels
{
    public abstract class DocumentationViewModel
    {
        public string SelectedVersion { get; set; }

        public Language SelectedLanguage { get; private set; }

        protected DocumentationViewModel(string version, Language language)
        {
            this.Toc = new List<TableOfContentsViewModel>();
            this.SelectedVersion = version;
            this.SelectedLanguage = language;
        }

        public void FillToc(List<TableOfContents> fromDb)
        {
            var mode = DocsVersion.GetModeForVersion(this.SelectedVersion);
            if (mode == DocsVersion.DocsMode.Normal)
            {
                this.Toc.Add(
                    new TableOfContentsViewModel(
                        new TableOfContents.TableOfContentsItem
                        {
                            IsFolder = false,
                            Items = new List<TableOfContents.TableOfContentsItem>(),
                            Key = string.Empty,
                            Languages = new List<Language>() { Language.All },
                            Title = "Start"
                        }, mode));
            }
            foreach (var toc in fromDb)
            {
                this.Toc.Add(new TableOfContentsViewModel(toc, mode));
            }
        }

        public List<TableOfContentsViewModel> Toc { get; private set; }
    }
}
