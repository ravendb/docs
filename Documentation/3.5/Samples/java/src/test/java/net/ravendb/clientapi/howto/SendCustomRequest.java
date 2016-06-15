package net.ravendb.clientapi.howto;

import net.ravendb.abstractions.data.HttpMethods;
import net.ravendb.abstractions.data.JsonDocument;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.connection.CreateHttpJsonRequestParams;
import net.ravendb.client.connection.IDatabaseCommands;
import net.ravendb.client.connection.RavenUrlExtensions;
import net.ravendb.client.connection.SerializationHelper;
import net.ravendb.client.connection.implementation.HttpJsonRequest;
import net.ravendb.client.document.DocumentStore;


public class SendCustomRequest {
  @SuppressWarnings("unused")
  public SendCustomRequest() throws Exception {
    try (DocumentStore store = new DocumentStore()) {
      //region custom_request_1
      String key = "employees/1";

      // http://localhost:8080/databases/Northwind/docs/employees/1
      String forDatabase = RavenUrlExtensions.forDatabase(store.getUrl(), "Northwind");
      String url = forDatabase + "/docs/" + key;

      IDatabaseCommands commands = store.getDatabaseCommands();
      HttpJsonRequest request = store
        .getJsonRequestFactory()
        .createHttpJsonRequest(new CreateHttpJsonRequestParams(commands, url, HttpMethods.GET, null, commands.getPrimaryCredentials(), store.getConventions()));

      RavenJToken json = request.readResponseJson();
      JsonDocument jsonDocument = SerializationHelper.deserializeJsonDocument(key, json, request.getResponseHeaders(), request.getResponseStatusCode());
      //endregion
    }
  }
}
