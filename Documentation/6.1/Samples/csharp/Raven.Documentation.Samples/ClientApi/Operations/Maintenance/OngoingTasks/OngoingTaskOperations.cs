using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Operations.Indexes;

namespace Raven.Documentation.Samples.ClientApi.Operations.Maintenance.OngoingTasks
{
    public class OngoingTaskOperations
    {

        public CreateTask()
        {
            #region create_task
            // Define a simple External Replication task
            var taskDefintion = new ExternalReplication("sourceDatabaseName", "ConnectionStringName")
            {
                Name = "MyExtRepTask"
            };
        
            // Deploy the task to the server
            var taskOp = new UpdateExternalReplicationOperation(taskDefintion);
            var sendResult = store.Maintenance.Send(taskOp);
        
            // The task ID is available in the send result
            var taskId = sendResult.TaskId;
            #endregion
        }
        
        public GetTask()
        {
            using (var store = new DocumentStore())
            {
                #region get
                // Define the get task operation, pass:
                // * The ongoing task ID or the task name
                // * The task type 
                var getTaskOp = new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.Replication);
                
                // Execute the operation by passing it to Maintenance.Send
                store.Maintenance.Send(getTaskOp);
                #endregion
            }
        }
        
        public async Task GetTaskAsync()
        {
            using (var store = new DocumentStore())
            {
                #region get_async
                var getTaskOp = new GetOngoingTaskInfoOperation(taskId, OngoingTaskType.Replication);
                await store.Maintenance.SendAsync(getTaskOp);
                #endregion
            }
        }
        
        public ToggleTask()
        {
            using (var store = new DocumentStore())
            {
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
                #region toggle_async
                var toggleTaskOp = new ToggleOngoingTaskStateOperation(taskId, OngoingTaskType.Replication, true);
                await store.Maintenance.SendAsync(toggleTaskOp);
                #endregion
            }
        }
        
        public DeleteTask()
        {
            using (var store = new DocumentStore())
            {
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
        
        #region syntax_4
        public enum OngoingTaskType
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
    }
}
