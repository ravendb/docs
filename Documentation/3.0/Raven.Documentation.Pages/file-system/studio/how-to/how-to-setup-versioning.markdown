#How to enable and setup versioning?

To take advantage of [Versioning bundle](../../server/bundles/versioning) you have to activate it when a file system is created.
Take the following steps to setup the versioning:

Step 1. Create a file system with `Versioning` enabled:

![Figure 1. Studio. Versioning. Create file system.](images/versioning-1.png)  

Step 2. Next you will get the dialog that specifies options for a default configuration:

![Figure 2. Studio. Versioning. Configuration dialog.](images/versioning-2.png)  

You can see that this configuration is stored as `Raven/Versioning/DefaultConfiguration` config:

![Figure 3. Studio. Versioning. Default configuration.](images/versioning-3.png)  

Step 3. If you want to overwrite the default configuration for a selected directory, create the appropriate configuration and
save it as `Raven/Versioning/[directory/path]` config item. For example to create the versioning config for `/temp` directory
and disable the versioning there, create the config as follow:

![Figure 4. Studio. Versioning. Custom configuration. Create.](images/versioning-4.png)  

![Figure 5. Studio. Versioning. Custom configuration. Save.](images/versioning-5.png)  


##Revisions folder

All file revisions are visible under special `$revisions` directory:

![Figure 6. Studio. Versioning. Revisions.](images/versioning-6.png)