// -----------------------------------------------------------------------
//  <copyright file="IDocsOutput.cs" company="Hibernating Rhinos LTD">
//      Copyright (c) Hibernating Rhinos LTD. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------
using System;
using RavenDB.DocsCompiler.Model;

namespace RavenDB.DocsCompiler.Output
{
    /// <summary>
    /// The DocsOutput interface.
    /// </summary>
    public interface IDocsOutput : IDisposable
    {
        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        OutputType ContentType { get; set; }

        /// <summary>
        /// Gets or sets the root url.
        /// </summary>
        string RootUrl { get; set; }

        /// <summary>
        /// Gets or sets the images path.
        /// </summary>
        string ImagesPath { get; set; }

        /// <summary>
        /// Save doc item.
        /// </summary>
        /// <param name="doc">
        /// The doc.
        /// </param>
        void SaveDocItem(Document doc);

        /// <summary>
        /// Save image.
        /// </summary>
        /// <param name="folder">
        /// The folder.
        /// </param>
        /// <param name="fullFilePath">
        /// The full file path.
        /// </param>
        void SaveImage(Folder folder, string fullFilePath);

        /// <summary>
        /// Generate the table of contents.
        /// </summary>
        /// <param name="rootItem">
        /// The root item.
        /// </param>
        void GenerateTableOfContents(IDocumentationItem rootItem);
    }
}