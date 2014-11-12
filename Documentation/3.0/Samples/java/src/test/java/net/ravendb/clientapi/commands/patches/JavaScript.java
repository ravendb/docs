package net.ravendb.clientapi.commands.patches;

import java.util.HashMap;
import java.util.Map;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.ScriptedPatchRequest;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.client.IDocumentStore;
import net.ravendb.client.document.DocumentStore;


public class JavaScript {
  @SuppressWarnings("unused")
  private interface IFoo {
    //region patch_1
    /**
     * Sends a patch request for a specific document, ignoring the document's Etag and  if the document is missing
     * @param key Key of the document to patch
     * @param patch The patch request to use (using JavaScript)
     * @return
     */
    public RavenJObject patch(String key, ScriptedPatchRequest patch);

    /**
     * Sends a patch request for a specific document, ignoring the document's Etag
     * @param key Key of the document to patch
     * @param patch The patch request to use (using JavaScript)
     * @param ignoreMissing true if the patch request should ignore a missing document, false to throw DocumentDoesNotExistException
     * @return
     */
    public RavenJObject patch(String key, ScriptedPatchRequest patch, boolean ignoreMissing);

    /**
     * Sends a patch request for a specific document
     * @param key Key of the document to patch
     * @param patch The patch request to use (using JavaScript)
     * @param etag Require specific Etag [null to ignore]
     * @return
     */
    public RavenJObject patch(String key, ScriptedPatchRequest patch, Etag etag);

    /**
     * Sends a patch request for a specific document which may or may not currently exist
     * @param key Id of the document to patch
     * @param patchExisting The patch request to use (using JavaScript) to an existing document
     * @param patchDefault The patch request to use (using JavaScript)  to a default document when the document is missing
     * @param defaultMetadata The metadata for the default document when the document is missing
     * @return
     */
    public RavenJObject patch(String key, ScriptedPatchRequest patchExisting, ScriptedPatchRequest patchDefault, RavenJObject defaultMetadata);
    //endregion
  }

  public JavaScript() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region patch_2
      // change FirstName to Robert
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("this.FirstName = 'Robert';")
      );
      //endregion

      //region patch_3
      // trim FirstName
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("this.FirstName = this.FirstName.trim();")
      );
      //endregion

      //region patch_4
      // add new property Age with value of 30
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("this.Age = 30;")
      );
      //endregion

      //region patch_5
      // add new property Age with value of 30 using LoDash
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("_.extend(this, { 'Age': '30'});")
      );
      //endregion

      //region patch_6
      // passing data and loading different document
      ScriptedPatchRequest scriptedPatchRequest =
        new ScriptedPatchRequest("var employee = LoadDocument(differentEmployeeId); this.FirstName = employee.FirstName;");
      Map<String, Object> params = new HashMap<>();
      params.put("differentEmployeeId", "employees/2");
      scriptedPatchRequest.setValues(params);
      store.getDatabaseCommands().patch("employees/1", scriptedPatchRequest);
      //endregion

      //region patch_7
      // accessing metadata (added JavaClass property with value from @metadata)
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("this.JavaClass = this['@metadata']['Raven-Java-Type'];")
      );
      //endregion

      //region patch_8
      // creating new document with auto-assigned key e.g. 'Comments/100'
      store.getDatabaseCommands().patch("employees/1",
        new ScriptedPatchRequest("PutDocument('Comments/', { 'Author': this.LastName }, { });")
      );
      //endregion

      //region patch_9
      // add a new comment to Comments
      store.getDatabaseCommands().patch("blogposts/1",
        new ScriptedPatchRequest("this.Comments.push({ 'Title': 'Some title', 'Content': 'Lore ipsum' });")
      );
      //endregion

      //region patch_1_0
      // removing comments with 'Some title' as a title
      store.getDatabaseCommands().patch("blogposts/1",
        new ScriptedPatchRequest("this.Comments.RemoveWhere(function(comment) { " +
                                 "           return comment.Title == 'Some title'; " +
                                 "       });"));
      //endregion

      //region patch_1_1
      // modifying each comment
      store.getDatabaseCommands().patch("blogposts/1",
        new ScriptedPatchRequest("this.Comments.Map(function(comment) { " +
                                 "           comment.Title = 'New title'; " +
                                 "           return comment; " +
                                 "       });"));
      //endregion

    }
  }
}
