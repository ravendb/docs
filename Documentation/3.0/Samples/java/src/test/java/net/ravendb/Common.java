package net.ravendb;

import java.util.List;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.json.linq.RavenJObject;


public class Common {

  //region multiloadresult
  public class MultiLoadResult
  {
    private List<RavenJObject> results;
    private List<RavenJObject> includes;

    public List<RavenJObject> getResults() {
      return results;
    }

    public void setResults(List<RavenJObject> results) {
      this.results = results;
    }

    public List<RavenJObject> getIncludes() {
      return includes;
    }

    public void setIncludes(List<RavenJObject> includes) {
      this.includes = includes;
    }
  }
  //endregion

  //region putresult
  public class PutResult
  {
    private String key;
    private Etag etag;

    public String getKey() {
      return key;
    }

    public void setKey(String key) {
      this.key = key;
    }

    public Etag getEtag() {
      return etag;
    }

    public void setEtag(Etag etag) {
      this.etag = etag;
    }
  }
  //endregion
}
