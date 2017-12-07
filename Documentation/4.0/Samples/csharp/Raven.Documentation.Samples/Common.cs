using System;
using System.Collections.Generic;
using System.Data;
using Raven.Client.Documents.Operations;

namespace Raven.Documentation.Samples
{
	public class CodeOmitted : Exception
	{
        public class PatchCommandData
        {
            #region patch_command_data
            public PatchCommandData(string id, string changeVector, PatchRequest patch, PatchRequest patchIfMissing)
            #endregion
            {                
            }            
        }

        #region patch_request
        public class PatchRequest 
        {
            /// <summary>
            /// JavaScript function to use to patch a document
            /// </summary>
            /// <value>The type.</value>
            public string Script { get; set; }

            /// <summary>
            /// Additional arguments passed to JavaScript function from Script.
            /// </summary>
            public Dictionary<string, object> Values { get; set; }
        }
        #endregion

        
        public class PatchOperation
        {
            #region patch_operation
            public PatchOperation(string id, string changeVector, PatchRequest patch, PatchRequest patchIfMissing = null, bool skipPatchIfChangeVectorMismatch = false)
            #endregion
            {

            }
        }
        
    }
}
