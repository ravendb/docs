#IIS - HTTP Error 503

When deploying to IIS you get HTTP Error 503. The service is unavailable. Nothing is written to the event log.

The problem usually occurs when you are trying to run the RavenDB's IIS site on port 8080 after you have run RavenDB in executable mode. The problem stems from RavenDB reserving the *http://+:8080/* namespace for your user, and when IIS attempts to start a site on the same endpoint, it errors.

You can resolve this problem by executing the following on the command line:

    netsh http delete urlacl http://+:8080/

You can also see all existing registrations with the following command:

    ntsh http show urlacl

