package net.ravendb.clientapi.configuration.conventions;

import java.io.IOException;

import org.apache.http.HttpRequest;
import org.apache.http.HttpResponse;
import org.codehaus.jackson.JsonProcessingException;
import org.codehaus.jackson.map.DeserializationContext;
import org.codehaus.jackson.map.DeserializationProblemHandler;
import org.codehaus.jackson.map.JsonDeserializer;

import net.ravendb.abstractions.closure.Action1;
import net.ravendb.abstractions.connection.OperationCredentials;
import net.ravendb.client.delegates.HttpResponseHandler;
import net.ravendb.client.delegates.HttpResponseWithMetaHandler;
import net.ravendb.client.document.DocumentConvention;
import net.ravendb.client.document.DocumentStore;


public class RequestHandling {

  public RequestHandling() {
    DocumentStore store = new DocumentStore();
    DocumentConvention conventions = store.getConventions();

    //region use_paraller_multi_get
    conventions.setUseParallelMultiGet(true);
    //endregion

    //region handle_forbidden_response
    conventions.setHandleForbiddenResponse(new HttpResponseHandler() {
      @Override
      public Action1<HttpRequest> handle(HttpResponse httpResponse) {
        return null;
      }
    });
    //endregion


    //region handle_unauthorized_response
    conventions.setHandleUnauthorizedResponse(new HttpResponseWithMetaHandler() {
      @Override
      public Action1<HttpRequest> handle(HttpResponse httpResponse, OperationCredentials credentials) {
        return null;
      }
    });
    //endregion

    //region json_contract_resolver
    conventions.setJsonContractResolver(new CustomJsonContractResolver());
    //endregion

    //region preserve_doc_props_not_found_on_model
    conventions.setPreserveDocumentPropertiesNotFoundOnModel(true);
    //endregion

  }

  //region custom_json_contract_resolver
  public static class CustomJsonContractResolver extends DeserializationProblemHandler
  {
    @Override
    public boolean handleUnknownProperty(DeserializationContext ctxt, JsonDeserializer<?> deserializer,
      Object beanOrClass, String propertyName) throws IOException, JsonProcessingException {
      return super.handleUnknownProperty(ctxt, deserializer, beanOrClass, propertyName);
    }

  }
  //endregion


}
