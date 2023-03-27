using System.Collections.Generic;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Backups;
using Raven.Client.Documents.Operations.ConnectionStrings;
using Raven.Client.Documents.Operations.ETL;
using Raven.Client.Documents.Operations.ETL.ElasticSearch;
using Raven.Client.Documents.Operations.ETL.OLAP;
using Raven.Client.Documents.Operations.ETL.SQL;
using Raven.Client.Documents.Operations.OngoingTasks;
using Raven.Client.ServerWide.Operations;
using Sparrow.Json.Parsing;

namespace Raven.Documentation.Samples.Sharding
{
    public class ShardingETL
    {
        public void GetShardInfo()
        {
            
            using (var store = new DocumentStore())
            {
                string[] shardName = new string[3];
                string[] NodeTag = new string[3];
                OngoingTaskConnectionStatus[] ongoingTaskConnectionStatus = new OngoingTaskConnectionStatus[3];
                string[] mentorNode = new string[3];

                var name = "taskName";

                #region get-shard-specific-info
                // Get basic info regarding the user-defined task
                var ongoingTask = store.Maintenance.Send(
                                new GetOngoingTaskInfoOperation(name, OngoingTaskType.RavenEtl));

                if (ongoingTask != null)
                {
                    // go through the shards and retrieve their info
                    for (int i = 0; i < 3; i++)
                    {
                        var singleShardInfo = store.Maintenance.ForShard(i).Send(
                                new GetOngoingTaskInfoOperation(name, OngoingTaskType.RavenEtl));

                        shardName[i] = singleShardInfo.TaskName;
                        NodeTag[i] = singleShardInfo.ResponsibleNode.NodeTag;
                        ongoingTaskConnectionStatus[i] = singleShardInfo.TaskConnectionStatus;
                        mentorNode[i] = singleShardInfo.MentorNode;
                        ongoingTaskConnectionStatus[i] = singleShardInfo.TaskConnectionStatus;

                    }
                }
                #endregion
            }
        }
    }
}

