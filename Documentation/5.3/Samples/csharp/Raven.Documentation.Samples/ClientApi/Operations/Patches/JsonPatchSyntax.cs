using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Commands.Batches;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using Microsoft.AspNetCore.JsonPatch;
using Raven.Client.Exceptions;

//using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Operations.Patches
{
    public class JsonPatchRequests
    {
        public JsonPatchRequests()
        {
            using (var store = new DocumentStore())
            {
                string documentId = null;
                dynamic originalCompany = new ExpandoObject();
                originalCompany.Name = "The Wall";

                using (var session = store.OpenSession())
                {
                    session.Store(originalCompany);
                    documentId = originalCompany.Id;
                    session.SaveChanges();
                }

                #region json_patch_Add_operation
                var patchesDocument = new JsonPatchDocument();
                patchesDocument.Add("/PropertyName", "Contents");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion

                #region json_patch_Remove_operation
                patchesDocument = new JsonPatchDocument();
                patchesDocument.Remove("/PropertyName");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion

                #region json_patch_Replace_operation
                patchesDocument = new JsonPatchDocument();
                // Replace document property contents with a new value (100)
                patchesDocument.Replace("/PropertyName", "NewContents");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion

                #region json_patch_Copy_operation
                patchesDocument = new JsonPatchDocument();
                // Copy document property contents to another document property
                patchesDocument.Copy("/PropertyName1", "/PropertyName2");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion

                #region json_patch_Move_operation
                patchesDocument = new JsonPatchDocument();
                // Move document property contents to another document property
                patchesDocument.Move("/PropertyName1", "/PropertyName2");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion

                #region json_patch_Test_operation
                patchesDocument = new JsonPatchDocument();
                patchesDocument.Test("/PropertyName", "Value"); // Compare property contents with the value 
                                                                // Revoke all patch operations if the test fails 
                try
                {
                    store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                }
                catch (RavenException e)
                {
                    // handle the exception
                }
                #endregion

                #region json_patch_Add_array_element
                patchesDocument = new JsonPatchDocument();
                // Use the path parameter to add an array element
                patchesDocument.Add("/ArrayName/12", "Contents");
                store.Operations.Send(new JsonPatchOperation(documentId, patchesDocument));
                #endregion
            }
        }
    }
}
