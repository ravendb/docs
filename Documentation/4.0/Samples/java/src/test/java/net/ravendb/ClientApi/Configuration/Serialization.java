package net.ravendb.ClientApi.Configuration;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import net.ravendb.client.documents.conventions.DocumentConventions;

public class Serialization {
    public Serialization() {
        DocumentConventions conventions = new DocumentConventions();


        //region customize_object_mapper
        ObjectMapper entityMapper = conventions.getEntityMapper();
        entityMapper.configure(SerializationFeature.FAIL_ON_EMPTY_BEANS, true);
        //endregion
    }
}
