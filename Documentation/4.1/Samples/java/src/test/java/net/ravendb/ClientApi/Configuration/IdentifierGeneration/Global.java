package net.ravendb.ClientApi.Configuration.IdentifierGeneration;

import com.fasterxml.jackson.databind.node.ObjectNode;
import net.ravendb.client.Constants;
import net.ravendb.client.documents.DocumentStore;
import net.ravendb.client.documents.IDocumentStore;
import net.ravendb.client.documents.conventions.DocumentConventions;

import java.util.Optional;

public class Global {

    public Global() {
        try (IDocumentStore store = new DocumentStore()) {
            DocumentConventions conventions = store.getConventions();


            //region find_type_name
            conventions.setFindJavaClassName(
                clazz -> // use reflection to determinate the type
            // endregion
            ""
            );

            //region find_clr_type
            conventions.setFindJavaClass((id, doc) -> {
                return Optional.ofNullable((ObjectNode) doc.get(Constants.Documents.Metadata.KEY))
                    .map(x -> x.get(Constants.Documents.Metadata.RAVEN_JAVA_TYPE))
                    .map(x -> x.asText())
                    .orElse(null);
            });
            //endregion

            //region find_type_collection_name
            conventions.setFindCollectionName(
                clazz -> // function that provides the collection name based on the entity class
            //endregion
                ""
            );

            //region transform_collection_name_to_prefix
            conventions.setTransformClassCollectionNameToDocumentIdPrefix(
                collectionName -> // transform the collection name to the prefix of a identifier, e.g. [prefix]/12
            //endregion
                ""
            );

            //region find_identity_property
            conventions.setFindIdentityProperty(fieldInfo -> "Id".equals(fieldInfo.getName()));
            //endregion

            //region find_identity_property_name_from_collection_name
            conventions.setFindIdentityPropertyNameFromCollectionName(
                collectionName -> "Id"
            );
            //endregion

            //region identity_part_separator
            conventions.setIdentityPartsSeparator("/");
            //endregion
        }
    }
}
