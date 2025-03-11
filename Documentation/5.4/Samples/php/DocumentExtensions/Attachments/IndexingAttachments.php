<?php

namespace RavenDB\Samples\DocumentExtensions\Attachments;

use RavenDB\Documents\Indexes\AbstractIndexCreationTask;
use RavenDB\Documents\Indexes\AbstractJavaScriptIndexCreationTask;
use RavenDB\Samples\Infrastructure\Orders\Employee;
use RavenDB\Type\StringArray;

interface IFoo
{
    /*
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
    */
}

class IndexingAttachments
{
    public function sample(): void
    {
        $store = new DocumentStore();
        try {
            $session = $store->openSession();
            try {
                # region query1
                //return all employees that have an attachment called "cv.pdf"
                /** @var array<Employee> $employees */
                $employees = $session
                    ->query(Employees_ByAttachmentNames_Result::class, Employees_ByAttachmentNames::class)
                    ->whereContainsAny("AttachmentNames", ["cv.pdf"])
                    ->ofType(Employee::class)
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }

            $session = $store->openSession();
            try {
                # region query3
                //return all employees that have an attachment called "cv.pdf"
                /** @var array<Employee> $employees */
                $employees = $session
                    ->advanced()
                    ->documentQuery(Employees_ByAttachmentNames_Result::class, Employees_ByAttachmentNames::class)
                    ->whereContainsAny("AttachmentNames", ["cv.pdf"])
                    ->toList();
                # endregion
            } finally {
                $session->close();
            }
        } finally {
            $store->close();
        }
    }
}

# region AttFor_index_LINQ
class Employees_ByAttachmentNames_Result
{
    public ?StringArray $attachmentNames = null;

    public function getAttachmentNames(): ?StringArray
    {
        return $this->attachmentNames;
    }

    public function setAttachmentNames(?StringArray $attachmentNames): void
    {
        $this->attachmentNames = $attachmentNames;
    }
}
class Employees_ByAttachmentNames extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map =
            "map('Employees', function (e) {" .
            "    var attachments = attachmentsFor(e);" .
            "    return {" .
            "        AttachmentNames: attachments.map(" .
            "            function(attachment) {" .
            "                return attachment.Name;" .
            "            }" .
            "    };" .
            "})";
    }
}
# endregion

# region AttFor_index_JS
class Employees_ByAttachmentNames_JS_Result
{
    public ?StringArray $attachmentNames = null;

    public function getAttachmentNames(): ?StringArray
    {
        return $this->attachmentNames;
    }

    public function setAttachmentNames(?StringArray $attachmentNames): void
    {
        $this->attachmentNames = $attachmentNames;
    }
}
class Employees_ByAttachmentNames_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Employees', function (e) {
                var attachments = attachmentsFor(e);
                return {
                    AttachmentNames: attachments.map(
                        function(attachment) {
                            return attachment.Name;
                        }
                };
            })"
        ]);
    }
}
# endregion

# region LoadAtt_index_LINQ
class Companies_With_Attachments extends AbstractIndexCreationTask
{

    public function __construct()
    {
        parent::__construct();

        $this->map =
            "from company in companies" .
            "let attachments = LoadAttachment(company, company.ExternalId)" .
            "select new" .
            "{" .
            "    CompanyName = company.Name," .
            "    AttachmentName = attachment.Name," .
            "    AttachmentContentType = attachment.ContentType," .
            "    AttachmentHash = attachment.Hash," .
            "    AttachmentContent = attachment.GetContentAsString(Encoding.UTF8)," .
            "}";
    }
}
# endregion

# region LoadAtt_index_JS
class Companies_With_Attachments_JavaScript extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Companies', function (company) {\n" .
            "   var attachment = LoadAttachment(company, company.ExternalId);\n" .
            "   return {\n" .
            "       CompanyName: company.Name,\n" .
            "       AttachmentName: attachment.Name,\n" .
            "       AttachmentContentType: attachment.ContentType,\n" .
            "       AttachmentHash: attachment.Hash,\n" .
            "       AttachmentSize: attachment.Size,\n" .
            "       AttachmentContent: attachment.getContentAsString('utf8')\n" .
            "   }\n".
            "});"
        ]);
    }
}
# endregion

# region LoadAtts_index_LINQ
class Companies_With_All_Attachments extends AbstractIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->map = "from company in companies " .
            "let attachments = LoadAttachments(company)" .
            "from attachment in attachments" .
            "select new" .
            "{" .
            "    attachment_name = attachment.Name," .
            "    attachment_content = attachment.GetContentAsString(Encoding.UTF8)" .
            "}";
    }
}
# endregion

# region LoadAtts_index_JS
class Companies_With_All_Attachments_JS extends AbstractJavaScriptIndexCreationTask
{
    public function __construct()
    {
        parent::__construct();

        $this->setMaps([
            "map('Companies', function (company) {\n" .
            "    var attachments = LoadAttachments(company);\n" .
            "    return attachments.map(attachment => ({\n" .
            "        AttachmentName: attachment.Name,\n" .
            "        AttachmentContent: attachment.getContentAsString('utf8')\n" .
            "     }));\n" .
            "})"
        ];
    }
}
# endregion
