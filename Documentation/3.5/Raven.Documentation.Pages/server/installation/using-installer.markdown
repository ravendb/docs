# Installation: Using installer

The installation of RavenDB can be accomplished with a GUI installer. The setup wizard will guide you through an installation process where you just need to
select the type of an installation and provide configuration options.

![Figure 1: Welcome screen](images/installer_welcome_screen.png)

##Target environment

You need to choose what kind of the environment you are targeting. The licensing requirements are different depending on the options selected:

* Production / Test - you have to provide a valid license in next step,
* Development - no license is required.

##Installation type

You can install RavenDB either as a Windows service or as an IIS application. Next steps will guide you through a configuration of the chosen deployment strategy. 

##Windows Service configuration

The configuration of the RavenDB service is straight-forward. You only have to provide the name and the port number of the service.

![Figure 2: Windows Service](images/installer_windows_service.png)

##IIS Application configuration

The configuration of RavenDB run on IIS requires you to go through a few screens. 

###Web site
In the first dialog you need to enter a website configuration. You have two possibilities here:

* Create a new web site
* Use an existing one

If you choose first option you will need to fill up the following fields:

![Figure 3: New IIS site configuration](images/installer_iis_new_site.png)

If you decide to use the already existing site you just need to choose which one:

![Figure 4: Existing IIS site](images/installer_iis_existing_site.png)

The _Virtual directory_ field can be empty. This means that RavenDB will be installed at the root of the web site.

Optionally, you can select a checkbox to configure a custom application pool for RavenDB application instead of using the one configured by default for the web site.

###Application Pool (optional)

This optional dialog (shown when the checkbox on the previous screen was selected) allows you to set up a custom application pool. As previously, you can either create a new or use an existing one.

![Figure 5: Application Pool](images/installer_iis_application_pool.png)

##Installation destination and RavenDB paths

In the last wizard's step you can change the destination folder of the installation as well as customize the RavenDB's path. You can set:

* Data directory
* Indexes directory
* Logs directory (for both storages - Esent and Voron)

The modifications made to these paths will affect web.config / Raven.Server.exe.config file and set the relevant settings there during the installation process.

![Figure 4: Installation dest and custom paths](images/installer_destination_and_paths.png)


##Upgrade

The RavenDB installer supports upgrades. The previously used settings (like a service name or an installation path) will be recovered by the wizard. The installation process first will 
automatically remove the old version and then will install the new one.

##Uninstall

Uninstallation can be accomplished by _Programs and Features_ in _Control Panel_ . Only the files created during an installation process will be removed, so all of database data will remain untouched on a disk.


##Quiet mode installation from command line

The RavenDB installer can also be run from a command line with administrative privileges. In order to do that you will have to specify all required installation settings. The following command shows the dialog with available options:

{CODE-BLOCK:json}
	ravendb-[version].exe -help
{CODE-BLOCK/}

Below there is a command which installs RavenDB as a windows service:

{CODE-BLOCK:json}
	ravendb-[version].exe /quiet /log C:\Temp\raven_log.txt /msicl "RAVEN_TARGET_ENVIRONMENT=DEVELOPMENT TARGETDIR=C:\ INSTALLFOLDER=C:\RavenDB RAVEN_INSTALLATION_TYPE=SERVICE REMOVE=IIS ADDLOCAL=Service"
{CODE-BLOCK/}

The list of RavenDB specific properties:

* <em>RAVEN_INSTALLATION_TYPE</em> - available options: SERVICE or IIS (quiet mode installation on IIS is not recommended)
* <em>RAVEN_TARGET_ENVIRONMENT</em> - available options: PRODUCTION (default), DEVELOPMENT
* <em>RAVEN_LICENSE_FILE_PATH</em> - a full path to the license file
* <em>RAVEN_DATA_DIR</em> - data directory (default: ~\Data)
* <em>RAVEN_INDEX_DIR</em> - indexes location (default: empty - together with tenant db's data)
* <em>RAVEN_STORAGE_LOGS_DIR</em> - logs location (default: empty - together with tenant db's data)
* <em>SERVICE_NAME</em> - default: RavenDB
* <em>SERVICE_PORT</em> - default: 8080
