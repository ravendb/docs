import * as assert from "assert";
import { 
    DocumentStore,
    PutIndexesOperation,
    AbstractIndexCreationTask,
    IndexDefinition
} from "ravendb";

const store = new DocumentStore();
const session = store.openSession();

{
    //region static_sorting2
    class Products_ByName extends AbstractIndexCreationTask {
        constructor() {
            super();

            this.map = `docs.Products.Select(product => new {     
                Name = product.Name 
            })`;

            this.analyze("Name", 
                "Raven.Server.Documents.Indexes.Persistence.Lucene.Analyzers.Collation.Cultures.SvCollationAnalyzer, Raven.Server");
        }
    }
    //endregion
}
