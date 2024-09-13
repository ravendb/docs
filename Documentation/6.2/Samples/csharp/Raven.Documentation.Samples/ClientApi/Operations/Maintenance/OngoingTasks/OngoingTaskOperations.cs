using System;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.OngoingTasks;
using Raven.Client.Documents.Operations.Replication;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.OngoingTasks
{
    public class OngoingTaskOperations
    {
        public void CreateTask()
        {
            using (var store = new DocumentStore())
            {
                #region create_task
                // Define a simple External Replication task
                var taskDefintion = new ExternalReplication
                {
                    Name = "MyExtRepTask",
                    ConnectionStringName = "MyConnectionStringName"
                };

                // Deploy the task to the server
                var taskOp = new UpdateExternalReplicationOperation(taskDefintion);
                var sendResult = store.Maintenance.Send(taskOp);

                // The task ID is available in the send result
                var taskId = sendResult.TaskId;
                #endregion
            }
        }
        
        public void GetTaskInfo()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1; // Placeholder value for compilation purpose
                
                #region get
                // Define the get task operation, pass:
                // * The ongoing task ID or the task name
                // * The task type 
                var getTaskInfoOp = new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.Replication);
                
                // Execute the operation by passing it to Maintenance.Send
                var taskInfo = (OngoingTaskReplication)store.Maintenance.Send(getTaskInfoOp);
                
                // Access the task info
                var taskState = taskInfo.TaskState;
                var taskDelayTime = taskInfo.DelayReplicationFor;
                var destinationUrls= taskInfo.TopologyDiscoveryUrls;
                // ...
                #endregion
            }
        }
        
        public async Task GetTaskInfoAsync()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1;
                
                #region get_async
                var getTaskInfoOp = new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.Replication);
                var taskInfo = (OngoingTaskReplication) await store.Maintenance.SendAsync(getTaskInfoOp);

                var taskState = taskInfo.TaskState;
                var taskDelayTime = taskInfo.DelayReplicationFor;
                var destinationUrls= taskInfo.TopologyDiscoveryUrls;
                // ...
                #endregion
            }
        }
        
        public void ToggleTask()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1;
                
                #region toggle
                // Define the delete task operation, pass:
                // * The ongoing task ID
                // * The task type
                // * A boolean value to enable/disable
                var toggleTaskOp = new ToggleOngoingTaskStateOperation(taskId, OngoingTaskType.Replication, true);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(toggleTaskOp);
                #endregion
            }
        }
        
        public async Task ToggleTaskAsync()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1;
                
                #region toggle_async
                var toggleTaskOp = new ToggleOngoingTaskStateOperation(taskId, OngoingTaskType.Replication, true);
                await store.Maintenance.SendAsync(toggleTaskOp);
                #endregion
            }
        }
        
        public void DeleteTask()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1;
                
                #region delete
                // Define the delete task operation, pass:
                // * The ongoing task ID
                // * The task type 
                var deleteTaskOp = new DeleteOngoingTaskOperation(taskId, OngoingTaskType.Replication);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(deleteTaskOp);
                #endregion
            }
        }
        
        public async Task DeleteTaskAsync()
        {
            using (var store = new DocumentStore())
            {
                var taskId = 1;
                
                #region delete_async
                var deleteTaskOp = new DeleteOngoingTaskOperation(taskId, OngoingTaskType.Replication);
                await store.Maintenance.SendAsync(deleteTaskOp);
                #endregion
            }
        }

        private interface IFoo
        {
            /*
            #region syntax_1
            // Get
            public GetOngoingTaskInfoOperation(long taskId, OngoingTaskType type);
            public GetOngoingTaskInfoOperation(string taskName, OngoingTaskType type);
            #endregion

            #region syntax_2            
            // Delete
            public DeleteOngoingTaskOperation(long taskId, OngoingTaskType taskType);
            #endregion

            #region syntax_3
            // Toggle
            public ToggleOngoingTaskStateOperation(long taskId, OngoingTaskType type, bool disable);
            #endregion
            */
        }
        
        /*
        #region syntax_4
        private enum OngoingTaskType
        {
            Replication,
            RavenEtl,
            SqlEtl,
            OlapEtl,
            ElasticSearchEtl,
            QueueEtl,
            Backup,
            Subscription,
            PullReplicationAsHub,
            PullReplicationAsSink,
            QueueSink,
        }
        #endregion
        */
        
        #region syntax_5
        public sealed class OngoingTaskReplication : OngoingTask
        {
            public OngoingTaskReplication() => this.TaskType = OngoingTaskType.Replication;
            public string DestinationUrl { get; set; }
            public string[] TopologyDiscoveryUrls { get; set; }
            public string DestinationDatabase { get; set; }
            public string ConnectionStringName { get; set; }
            public TimeSpan DelayReplicationFor { get; set; }
        }
        #endregion
    }
}
