using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ASP.NET_MVC4.Helpers;

namespace ASP.NET_MVC4.Tasks
{
	public static class TaskExecutor
	{
		private static readonly ThreadLocal<List<BackgroundTask>> tasksToExecute =
			new ThreadLocal<List<BackgroundTask>>(() => new List<BackgroundTask>());

		public static Action<Exception> ExceptionHandler { get; set; }

		public static void ExcuteLater(BackgroundTask task)
		{
			tasksToExecute.Value.Add(task);
		}

		public static void Discard()
		{
			tasksToExecute.Value.Clear();
		}

		public static void StartExecuting()
		{
			var value = tasksToExecute.Value;
			var copy = value.ToArray();
			value.Clear();

			if (copy.Length > 0)
			{
				Task.Factory.StartNew(() =>
				{
					foreach (var backgroundTask in copy)
					{
						ExecuteTask(backgroundTask);
					}
				}, TaskCreationOptions.LongRunning)
					.ContinueWith(task =>
					{
						if (ExceptionHandler != null) ExceptionHandler(task.Exception);
					}, TaskContinuationOptions.OnlyOnFaulted);
			}
		}

		public static void ExecuteTask(BackgroundTask task)
		{
			for (var i = 0; i < 10; i++)
			{
				using (var session = DocumentStoreHolder.Store.OpenSession())
				{
					switch (task.Run(session))
					{
						case true:
						case false:
							return;
						case null:
							break;
					}
				}
			}
		}
	}
}