package net.ravendb.clientapi.commands.patches;

import net.ravendb.abstractions.data.Etag;
import net.ravendb.abstractions.data.PatchCommandType;
import net.ravendb.abstractions.data.PatchRequest;
import net.ravendb.abstractions.json.linq.RavenJObject;
import net.ravendb.abstractions.json.linq.RavenJToken;
import net.ravendb.client.document.DocumentStore;
import net.ravendb.client.IDocumentStore;
import net.ravendb.samples.BlogComment;


public class PatchRequests {

  @SuppressWarnings("unused")
  private interface IFoo {
    //region patch_1
    /**
     * Sends a patch request for a specific document, ignoring document's Etag and if it is missing
     * @param key Key of the document to patch
     * @param patches Array of patch requests
     * @return
     */
    public RavenJObject patch(String key, PatchRequest[] patches);

    /**
     * Sends a patch request for a specific document, ignoring the document's Etag
     * @param key Key of the document to patch
     * @param patches Array of patch requests
     * @param ignoreMissing true if the patch request should ignore a missing document, false to throw DocumentDoesNotExistException
     * @return
     */
    public RavenJObject patch(String key, PatchRequest[] patches, boolean ignoreMissing);

    /**
     * Sends a patch request for a specific document
     * @param key Key of the document to patch
     * @param patches Array of patch requests
     * @param etag Require specific Etag [null to ignore]
     * @return
     */
    public RavenJObject patch(String key, PatchRequest[] patches, Etag etag);

    /**
     * Sends a patch request for a specific document which may or may not currently exist
     * @param key Id of the document to patch
     * @param patchesToExisting Array of patch requests to apply to an existing document
     * @param patchesToDefault Array of patch requests to apply to a default document when the document is missing
     * @param defaultMetadata The metadata for the default document when the document is missing
     * @return
     */
    public RavenJObject patch(String key, PatchRequest[] patchesToExisting, PatchRequest[] patchesToDefault, RavenJObject defaultMetadata);
    //endregion
  }

  public PatchRequests() throws Exception {
    try (IDocumentStore store = new DocumentStore()) {
      //region patch_2
      // change FirstName to Robert
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.SET, "FirstName", RavenJToken.fromObject("Robert"))
      });
      //endregion

      //region patch_1_0
      // change FirstName to Robert and LastName to Carter in single request
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.SET, "FirstName", RavenJToken.fromObject("Robert")),
        new PatchRequest(PatchCommandType.SET, "LastName", RavenJToken.fromObject("Carter")),
      });
      //endregion

      //region patch_3
      // add new property Age with value of 30
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.ADD, "Age", RavenJToken.fromObject(30)),
      });
      //endregion

      //region patch_4
      // increment Age property value by 10
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.INC, "Age", RavenJToken.fromObject(10)),
      });
      //endregion

      //region patch_5
      // remove property Age
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.UNSET, "Age", null),
      });
      //endregion

      //region patch_6
      // rename FirstName to First
      store.getDatabaseCommands().patch("employees/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.RENAME, "FirstName", RavenJToken.fromObject("First")),
      });
      //endregion

      //region patch_7
      // add a new comment to Comments
      BlogComment blogComment = new BlogComment();
      blogComment.setContent("Lore ipsum");
      blogComment.setTitle("Some title");
      store.getDatabaseCommands().patch("blogposts/1", new PatchRequest[] {
        new PatchRequest(PatchCommandType.ADD, "Comments", RavenJToken.fromObject(blogComment)),
      });
      //endregion

      //region patch_8
      // insert a new comment at position 0 to Comments
      BlogComment comment = new BlogComment();
      comment.setContent("Lore ipsum");
      comment.setTitle("Some title");
      PatchRequest patchRequest = new PatchRequest(PatchCommandType.INSERT, "Comments", RavenJToken.fromObject(comment));
      patchRequest.setPosition(0);
      store.getDatabaseCommands().patch("blogposts/1", new PatchRequest[] {
        patchRequest
      });
      //endregion

      //region patch_9
      // modify a comment at position 3 in Comments
      PatchRequest subPatch = new PatchRequest(PatchCommandType.SET, "Title", RavenJToken.fromObject("New title"));
      PatchRequest mainPatch = new PatchRequest();
      mainPatch.setType(PatchCommandType.MODIFY);
      mainPatch.setPosition(3);
      mainPatch.setName("Comments");
      mainPatch.setNested(new PatchRequest[] { subPatch });
      store.getDatabaseCommands().patch("blogposts/1", new PatchRequest[] {
        mainPatch
      });

      //endregion
    }
  }
}
