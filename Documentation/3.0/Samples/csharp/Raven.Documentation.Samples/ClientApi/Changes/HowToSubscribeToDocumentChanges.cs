using System;

using Raven.Abstractions.Data;
using Raven.Client.Changes;
using Raven.Client.Document;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
	public class HowToSubscribeToDocumentChanges
	{
		private interface IFoo
		{
			#region document_changes_1
			IObservableWithTask<DocumentChangeNotification> ForDocument(string docId);
			#endregion

			#region document_changes_3
			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsInCollection(string collectionName);

			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsInCollection<TEntity>();
			#endregion

			#region document_changes_6
			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsOfType(string typeName);

			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsOfType(Type type);

			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsOfType<TEntity>();
			#endregion

			#region document_changes_9
			IObservableWithTask<DocumentChangeNotification>
				ForDocumentsStartingWith(string docIdPrefix);
			#endregion

			#region document_changes_1_1
			IObservableWithTask<DocumentChangeNotification>
				ForAllDocuments();
			#endregion
		}

		public HowToSubscribeToDocumentChanges()
		{
			using (var store = new DocumentStore())
			{
				#region document_changes_2
				var subscribtion = store
					.Changes()
					.ForDocument("employees/1")
					.Subscribe(
						change =>
						{
							switch (change.Type)
							{
								case DocumentChangeTypes.Put:
									// do something
									break;
								case DocumentChangeTypes.Delete:
									// do something
									break;
							}
						});
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_4
				var subscribtion = store
					.Changes()
					.ForDocumentsInCollection<Employee>()
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_5
				var collectionName = store.Conventions.GetTypeTagName(typeof(Employee));
				var subscribtion = store
					.Changes()
					.ForDocumentsInCollection(collectionName)
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_7
				var subscribtion = store
					.Changes()
					.ForDocumentsOfType<Employee>()
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_8
				var typeName = store.Conventions.FindClrTypeName(typeof(Employee));
				var subscribtion = store
					.Changes()
					.ForDocumentsOfType(typeName)
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_1_0
				var subscribtion = store
					.Changes()
					.ForDocumentsStartingWith("employees/1") // employees/1, employees/10, employees/11, etc.
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}

			using (var store = new DocumentStore())
			{
				#region document_changes_1_2
				var subscribtion = store
					.Changes()
					.ForAllDocuments() // employees/1, orders/1, customers/1, etc.
					.Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
				#endregion
			}
		}
	}
}