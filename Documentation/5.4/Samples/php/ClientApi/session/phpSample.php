# region php_region
$session = DocumentStoreHolder::getStore()->openSession();
try {
    $company = $session->load(Company::class, "companies/5-A");

    $company->setName($companyName);

    $session->saveChanges();
            
} finally {
    $session->close();
}
# endregion
