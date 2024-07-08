from typing import List

from ravendb import AbstractIndexCreationTask
from ravendb.documents.indexes.abstract_index_creation_tasks import AbstractJavaScriptIndexCreationTask

from examples_base import ExampleBase


class IndexingAttachments:
    """
    # region syntax
    IEnumerable<AttachmentName> AttachmentsFor(object doc);
    # endregion

    # region syntax_2
    public IAttachmentObject LoadAttachment(object doc, string name);
    public IEnumerable<IAttachmentObject> LoadAttachments(object doc);
    # endregion

    # region result
    public string Name;
    public string Hash;
    public string ContentType;
    public long Size;
    # endregion
    """


# region AttFor_index_LINQ
class Employees_ByAttachmentNames(AbstractIndexCreationTask):
    class Result:
        def __init__(self, attachment_names: List[str] = None):
            self.attachment_names = attachment_names

    def __init__(self):
        super().__init__()
        self.map = (
            "from e in employees "
            "let attachments = AttachmentsFor(e) "
            "select new "
            "{"
            "    attachment_names = attachments.Select(x => x.Name).ToArray()"
            "}"
        )


# endregion


# region AttFor_index_JS
class Employees_ByAttachmentNames_JS(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, attachment_names: List[str] = None):
            self.attachment_names = attachment_names

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (e) {
                var attachments = attachmentsFor(e);
                return {
                    attachment_names: attachments.map(
                        function(attachment) {
                            return attachment.Name;
                        }
                };
            })
            """
        }


# endregion


# region LoadAtt_index_LINQ
class Companies_With_Attachments(AbstractJavaScriptIndexCreationTask):
    class Result:
        def __init__(self, attachment_names: List[str] = None):
            self.attachment_names = attachment_names

    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Employees', function (e) {
                var attachments = attachmentsFor(e);
                return {
                    attachment_names: attachments.map(
                        function(attachment) {
                            return attachment.Name;
                        }
                };
            })
            """
        }


# endregion


# region LoadAtt_index_LINQ
class Companies_With_Attachments(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from company in companies"
            "let attachments = LoadAttachment(company, company.ExternalId)"
            "select new"
            "{"
            "    company_name = company.Name,"
            "    attachment_name = attachment.Name,"
            "    attachment_content_type = attachment.ContentType,"
            "    attachment_hash = attachment.Hash,"
            "    attachment_content = attachment.GetContentAsString(Encoding.UTF8),"
            "}"
        )


# endregion


# region LoadAtt_index_JS
class Companies_With_Attachments_JavaScript(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Companies', function (company) {
                var attachment = loadAttachment(company, company.ExternalId);
                return {
                    company_name: company.Name,
                    attachment_name: attachment.Name,
                    attachment_content_type: attachment.ContentType,
                    attachment_hash: attachment.Hash,
                    attachment_size: attachment.Size,
                    attachment_content: attachment.getContentAsString('utf8')
                };
            })
            """
        }


# endregion


# region LoadAtts_index_LINQ
class Companies_With_All_Attachments(AbstractIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.map = (
            "from company in companies "
            "let attachments = LoadAttachments(company)"
            "from attachment in attachments"
            "select new"
            "{"
            "    attachment_name = attachment.Name,"
            "    attachment_content = attachment.GetContentAsString(Encoding.UTF8)"
            "}"
        )


# endregion


# region LoadAtts_index_JS
class Companies_With_All_Attachments_JS(AbstractJavaScriptIndexCreationTask):
    def __init__(self):
        super().__init__()
        self.maps = {
            """
            map('Companies', function (company) {
                var attachments = loadAttachments(company);
                return attachments.map(attachment => ({
                    attachment_name: attachment.Name,
                    attachment_content: attachment.getContentAsString('utf8')
                }));
            })
            """
        }


# endregion


class IndexingAttachments(ExampleBase):
    def setUp(self):
        super().__init__()

    def test_indexing_attachments(self):
        with self.embedded_server.get_document_store("IndexingAttachments") as store:
            with store.open_session() as session:
                # region query1
                # return all employees that have an attachment called "cv.pdf"
                employees = list(
                    session.query_index_type(
                        Employees_ByAttachmentNames, Employees_ByAttachmentNames.Result
                    ).contains_any("attachment_names", ["cv.pdf"])
                )
                # endregion
