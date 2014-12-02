using System;
using Raven.Abstractions.Exceptions;
using Raven.Client;

namespace ASP.NET_MVC4.Tasks
{
	public abstract class BackgroundTask
	{
		protected IDocumentSession DocumentSession;

		protected virtual void Initialize(IDocumentSession session)
		{
			DocumentSession = session;
			DocumentSession.Advanced.UseOptimisticConcurrency = true;
		}

		protected virtual void OnError(Exception e)
		{
		}

		public bool? Run(IDocumentSession openSession)
		{
			Initialize(openSession);
			try
			{
				Execute();
				DocumentSession.SaveChanges();
				TaskExecutor.StartExecuting();
				return true;
			}
			catch (ConcurrencyException e)
			{
				OnError(e);
				return null;
			}
			catch (Exception e)
			{
				OnError(e);
				return false;
			}
			finally
			{
				TaskExecutor.Discard();
			}
		}

		public abstract void Execute();
	}
}