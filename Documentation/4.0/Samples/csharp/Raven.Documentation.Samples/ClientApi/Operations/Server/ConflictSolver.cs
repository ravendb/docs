using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace Raven.Documentation.Samples.ClientApi.Operations.Server
{

    public class ConflictSolver
    {
        private interface IFoo
        {
            /*
            #region solver_1
            public ModifyConflictSolverOperation(
                string database, 
                Dictionary<string, ScriptResolver> collectionByScript = null, 
                bool resolveToLatest = false)
            #endregion
            */
        }
        private class Foo
        {
            #region solver_2
            public class ScriptResolver
            {
                public string Script { get; set; }
            }
            #endregion
        }

        public ConflictSolver()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region solver_3
                    // resolve conflict to latest version
                    ModifyConflictSolverOperation operation = 
                        new ModifyConflictSolverOperation("Northwind", null, resolveToLatest: true);
                    store.Maintenance.Server.Send(operation);
                    #endregion  
                }

                {
                    #region solver_4
                    // resolve conflict by finding max value 
                    string script = @"
                    var maxRecord = 0;
                    for (var i = 0; i < docs.length; i++) {
                        maxRecord = Math.max(docs[i].maxRecord, maxRecord);   
                    }
                    docs[0].MaxRecord = maxRecord;

                    return docs[0];";

                    ModifyConflictSolverOperation operation = 
                        new ModifyConflictSolverOperation("Northwind", new Dictionary<string, ScriptResolver>
                        {
                            { "Orders", new ScriptResolver { Script = script} }
                        }, resolveToLatest: false);
                    store.Maintenance.Server.Send(operation);
                    #endregion 
                }
            }
        }
    }
}
