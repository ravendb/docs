using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Raven.Client.Documents;
using Raven.Client.Documents.Attachments;
using Raven.Client.Documents.Indexes;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Operations.Attachments;
using Raven.Client.Documents.Operations.Configuration;
using Raven.Client.Documents.Queries;
using Raven.Client.Http;
using Raven.Client.ServerWide;

namespace Raven.Documentation.Samples.ClientApi.Operations
{
    public class Attachments
    {
        private interface IFoo
        {
            /*
            #region delete_attachment_syntax
            public DeleteAttachmentOperation(string documentId, string name, string changeVector = null)
            #endregion

            #region put_attachment_syntax
            public PutAttachmentOperation(string documentId, 
                    string name, 
                    Stream stream, 
                    string contentType = null, 
                    string changeVector = null)
            #endregion

            #region get_attachment_syntax
            public GetAttachmentOperation(string documentId, string name, AttachmentType type, string changeVector)
            #endregion
            */
        }

        private class Foo
        {
            #region put_attachment_return_value
            public class AttachmentDetails : AttachmentName
            {
                public string ChangeVector;
                public string DocumentId;
            }

            public class AttachmentName
            {
                public string Name;
                public string Hash;
                public string ContentType;
                public long Size;
            }
            #endregion
        }

        private class Foo2
        {
            #region get_attachment_return_value
            public class AttachmentResult
            {
                public Stream Stream;
                public AttachmentDetails Details;
            }

            public class AttachmentDetails : AttachmentName
            {
                public string ChangeVector;
                public string DocumentId;
            }

            public class AttachmentName
            {
                public string Name;
                public string Hash;
                public string ContentType;
                public long Size;
            }
            #endregion
        }


        public Attachments()
        {
            using (var store = new DocumentStore())
            {
                {
                    #region delete_1
                    store.Operations.Send(new DeleteAttachmentOperation("orders/1-A", "invoice.pdf"));
                    #endregion
                }

                {
                    #region get_1
                    store.Operations.Send(new GetAttachmentOperation("orders/1-A",
                        "invoice.pdf",
                        AttachmentType.Document,
                        changeVector: null));
                    #endregion
                }

                {
                    Stream stream = null;
                    #region put_1
                    AttachmentDetails attachmentDetails =
                        store.Operations.Send(
                            new PutAttachmentOperation("orders/1-A",
                                "invoice.pdf",
                                stream,
                                "application/pdf"));
                    #endregion
                }
            }
        }
    }
}
