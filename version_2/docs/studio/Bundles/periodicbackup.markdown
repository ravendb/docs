# Periodic Backup Bundle
In order to enable periodic backup for a database you need to go to its setting by pressing the cog wheel on the top right next to its name:  
![Periodic Backup Fig 1](Images/studio_periodic_1.PNG)  

Once there select the "Periodic Backup" option:  
![Periodic Backup Fig 2](Images/studio_periodic_2.PNG)  

On the first run you need to activate the periodic backup, after you do so you will need to enter the setting for the backup:  
![Periodic Backup Fig 3](Images/studio_periodic_3.PNG)  

In here you have several fields to fill:  

- Glacier Vault Name/S3 Bucket Name: first select in which way you wish to backup and then enter the name  
![Periodic Backup Fig 4](Images/studio_periodic_4.PNG)  
- AWS Settings: You need to set the AWS access key, secret and region endpoint.
- Interval: Set the interval between backups (in minutes)