using System;
using Raven.Client.Documents;
using Raven.Client.Documents.Changes;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Changes
{
    public class HowToSubscribeToDocumentChanges
    {
        private interface IFoo
        {
            #region document_changes_1
            IChangesObservable<DocumentChange> ForDocument(string docId);
            #endregion

            #region document_changes_3
            IChangesObservable<DocumentChange> ForDocumentsInCollection(string collectionName);

            IChangesObservable<DocumentChange> ForDocumentsInCollection<TEntity>();
            #endregion

            #region document_changes_9
            IChangesObservable<DocumentChange> ForDocumentsStartingWith(string docIdPrefix);
            #endregion

            #region document_changes_1_1
            IChangesObservable<DocumentChange> ForAllDocuments();
            #endregion
        }

        public HowToSubscribeToDocumentChanges()
        {
            using (var store = new DocumentStore())
            {
                #region document_changes_2
                IDisposable subscribtion = store
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
                IDisposable subscribtion = store
                    .Changes()
                    .ForDocumentsInCollection<Employee>()
                    .Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region document_changes_5
                string collectionName = store.Conventions.FindCollectionName(typeof(Employee));
                IDisposable subscribtion = store
                    .Changes()
                    .ForDocumentsInCollection(collectionName)
                    .Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region document_changes_1_0
                IDisposable subscribtion = store
                    .Changes()
                    .ForDocumentsStartingWith("employees/1") // employees/1, employees/10, employees/11, etc.
                    .Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
                #endregion
            }

            using (var store = new DocumentStore())
            {
                #region document_changes_1_2
                IDisposable subscribtion = store
                    .Changes()
                    .ForAllDocuments() // employees/1, orders/1, customers/1, etc.
                    .Subscribe(change => Console.WriteLine("{0} on document {1}", change.Type, change.Id));
                #endregion
            }
        }
    }
}
