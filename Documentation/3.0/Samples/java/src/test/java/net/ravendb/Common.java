package net.ravendb;

import java.util.List;
import java.util.Map;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.PatchCommandType;
import net.ravendb.abstractions.data.PatchRequest;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.json.linq.RavenJToken;


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

  //region patchrequest
  public class PatchRequest {
    private PatchCommandType type = PatchCommandType.SET;
    private RavenJToken prevVal;
    private RavenJToken value;
    private PatchRequest[] nested;
    private String name;
    private Integer position;
    private Boolean allPositions;

    public PatchRequest() {
      super();
    }
    public PatchRequest(PatchCommandType type, String name, RavenJToken value) {
      super();
      this.type = type;
      this.name = name;
      this.value = value;
    }

    public PatchCommandType getType() {
      return type;
    }

    public void setType(PatchCommandType type) {
      this.type = type;
    }

    public RavenJToken getPrevVal() {
      return prevVal;
    }

    public void setPrevVal(RavenJToken prevVal) {
      this.prevVal = prevVal;
    }

    public RavenJToken getValue() {
      return value;
    }

    public void setValue(RavenJToken value) {
      this.value = value;
    }

    public PatchRequest[] getNested() {
      return nested;
    }

    public void setNested(PatchRequest[] nested) {
      this.nested = nested;
    }

    public String getName() {
      return name;
    }

    public void setName(String name) {
      this.name = name;
    }

    public Integer getPosition() {
      return position;
    }

    public void setPosition(Integer position) {
      this.position = position;
    }

    public Boolean getAllPositions() {
      return allPositions;
    }

    public void setAllPositions(Boolean allPositions) {
      this.allPositions = allPositions;
    }
  }
  //endregion

  //region patchcommandtype
  public enum PatchCommandType {

    /**
     * Set a property
     */
    SET,
    /**
     * Unset (remove) a property
     */
    UNSET,

    /**
     * Add an item to an array
     */
    ADD,

    /**
     *  Insert an item to an array at a specified position
     */
    INSERT,

    /**
     * Remove an item from an array at a specified position
     */
    REMOVE,

    /**
     *  Modify a property value by providing a nested set of patch operation
     */
    MODIFY,

    /**
     *  Increment a property by a specified value
     */
    INC,

    /**
     * Copy a property value to another property
     */
    COPY,

    /**
     * Rename a property
     */
    RENAME;
  }

  //endregion

  //region scriptedpatchrequest
  public class ScriptedPatchRequest {
    private String script;
    private Map<String, Object> values;

    public String getScript() {
      return script;
    }

    public void setScript(String script) {
      this.script = script;
    }

    public Map<String, Object> getValues() {
      return values;
    }

    public void setValues(Map<String, Object> values) {
      this.values = values;
    }
  }
  //endregion

  //region buildnumber
  public class BuildNumber {
    private String productVersion;
    private String buildVersion;

    public String getProductVersion() {
      return productVersion;
    }

    public void setProductVersion(String productVersion) {
      this.productVersion = productVersion;
    }

    public String getBuildVersion() {
      return buildVersion;
    }

    public void setBuildVersion(String buildVersion) {
      this.buildVersion = buildVersion;
    }
  }
  //endregion
}
