package net.ravendb.server.troubleshooting;

import net.ravendb.abstractions.basic.EventHandler;
import net.ravendb.abstractions.connection.WebRequestEventArgs;
import net.ravendb.abstractions.data.DatabaseStatistics;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.RavenDBAwareTests.FiddlerConfigureRequestHandler;
import net.ravendb.client.document.DocumentStore;

import org.apache.http.HttpHost;
import org.apache.http.client.config.RequestConfig;
import org.apache.http.client.methods.HttpRequestBase;


public class SendingSupportTicket {
  //region fiddler_setup
  public static void useFiddler(IDocumentStore store) {
    store.getJsonRequestFactory().addConfigureRequestEventHandler(new FiddlerConfigureRequestHandler());
  }

  public static class FiddlerConfigureRequestHandler implements EventHandler<WebRequestEventArgs> {
    @Override
    public void handle(Object sender, WebRequestEventArgs event) {
      HttpRequestBase requestBase = (HttpRequestBase) event.getRequest();
      HttpHost proxy = new HttpHost("127.0.0.1", 8888, "http");
      RequestConfig requestConfig = requestBase.getConfig();
      if (requestConfig == null) {
        requestConfig = RequestConfig.DEFAULT;
      }
      requestConfig = RequestConfig.copy(requestConfig).setProxy(proxy).build();
      requestBase.setConfig(requestConfig);

    }
  }
  //endregion

  public static void main(String[] args) throws Exception {
    //region fiddler_usage
    try (IDocumentStore store = new DocumentStore("http://localhost:8080").initialize()) {
      useFiddler(store);
      // your code
    }
    //endregion
  }



}
