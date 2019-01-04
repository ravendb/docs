package net.ravendb.Indexes;

import net.ravendb.client.documents.indexes.AbstractIndexCreationTask;

public class Sorting {
    //region static_sorting2
    public static class Products_ByName extends AbstractIndexCreationTask {
        public Products_ByName() {
            map = "docs.Products.Select(product => new { " +
                "    Name = product.Name " +
                "})";

            analyze("Name", "Raven.Server.Documents.Indexes.Persistence.Lucene.Analyzers.Collation.Cultures.SvCollationAnalyzer, Raven.Server");
        }
    }
    //endregion

}
