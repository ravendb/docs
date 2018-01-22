using System;
using System.Collections.Generic;
using System.Data;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;

namespace Raven.Documentation.Samples
{
    public class CodeOmitted : Exception
    {
        public class PatchCommandData
        {
            #region patch_command_data
            public PatchCommandData(
                    string id,
                    string changeVector,
                    PatchRequest patch,
                    PatchRequest patchIfMissing)
            #endregion
            {
            }
        }

        #region patch_request
        public class PatchRequest
        {
            /// JavaScript function to use to patch a document
            public string Script { get; set; }

            /// Additional arguments passed to JavaScript function from Script.
            public Dictionary<string, object> Values { get; set; }
        }
        #endregion


        public class PatchOperation
        {
            #region patch_operation
            public PatchOperation(
                    string id,
                    string changeVector,
                    PatchRequest patch,
                    PatchRequest patchIfMissing = null,
                    bool skipPatchIfChangeVectorMismatch = false)
            #endregion
            {

            }
        }

        public interface OperationSend
        {
            #region sendingSetBasedPatchRequest
            Operation Send(PatchByQueryOperation operation);
            #endregion
        }

        public class PatchByQueryOperation
        {
            #region patchBeQueryOperationCtor1
            public PatchByQueryOperation(string queryToUpdate)
            #endregion
            {

            }


            #region patchBeQueryOperationCtor2
            public PatchByQueryOperation(IndexQuery queryToUpdate, QueryOperationOptions options = null)
            #endregion
            {

            }

        }

    }
}
