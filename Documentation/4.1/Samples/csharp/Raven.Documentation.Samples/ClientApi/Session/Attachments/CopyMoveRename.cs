using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Documentation.Samples.Orders;

namespace Raven.Documentation.Samples.ClientApi.Session.Attachments
{
    public class CopyMoveRename
    {
        private interface IFoo
        {
            #region copy_0
            void Copy(
                object sourceEntity,
                string sourceName,
                object destinationEntity,
                string destinationName);

            void Copy(
                string sourceDocumentId,
                string sourceName,
                string destinationDocumentId,
                string destinationName);
            #endregion

            #region rename_0
            void Rename(object entity, string name, string newName);

            void Rename(string documentId, string name, string newName);
            #endregion

            #region move_0
            void Move(object sourceEntity, string sourceName, object destinationEntity, string destinationName);

            void Move(string sourceDocumentId, string sourceName, string destinationDocumentId, string destinationName);
            #endregion
        }

        public async Task Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    #region copy_1
                    var employee1 = session.Load<Employee>("employees/1-A");
                    var employee2 = session.Load<Employee>("employees/2-A");

                    session.Advanced.Attachments.Copy(employee1, "photo.jpg", employee2, "photo-copy.jpg");

                    session.SaveChanges();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region copy_2
                    var employee1 = await asyncSession.LoadAsync<Employee>("employees/1-A");
                    var employee2 = await asyncSession.LoadAsync<Employee>("employees/2-A");

                    asyncSession.Advanced.Attachments.Copy(employee1, "photo.jpg", employee2, "photo-copy.jpg");

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region rename_1
                    var employee = session.Load<Employee>("employees/1-A");

                    session.Advanced.Attachments.Rename(employee, "photo.jpg", "photo-new.jpg");

                    session.SaveChanges();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region rename_2
                    var employee = await asyncSession.LoadAsync<Employee>("employees/1-A");

                    asyncSession.Advanced.Attachments.Rename(employee, "photo.jpg", "photo-new.jpg");

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region move_1
                    var employee1 = session.Load<Employee>("employees/1-A");
                    var employee2 = session.Load<Employee>("employees/2-A");

                    session.Advanced.Attachments.Move(employee1, "photo.jpg", employee2, "photo.jpg");

                    session.SaveChanges();
                    #endregion
                }

                using (var asyncSession = store.OpenAsyncSession())
                {
                    #region move_2
                    var employee1 = await asyncSession.LoadAsync<Employee>("employees/1-A");
                    var employee2 = await asyncSession.LoadAsync<Employee>("employees/2-A");

                    asyncSession.Advanced.Attachments.Move(employee1, "photo.jpg", employee2, "photo.jpg");

                    await asyncSession.SaveChangesAsync();
                    #endregion
                }
            }
        }
    }
}
