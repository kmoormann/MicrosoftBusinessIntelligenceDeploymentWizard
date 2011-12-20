Feature: US01 Deploy BI Files
	
	As a business intelligence developer
	I want to build a deployment manifest that has all my BI artifacts
	so that I deploy them to different servers

Background: 
	Given I have a ticket number 02
	And the project name is "BI Project"
	And I have selected the deployment path of C:\DeploymentsTemp\
	Given I have the following Business Intelligence Deployables
		| Server               | Name                 | Type | Deploy | Path                                                    |
		|                      | Package1             | SSIS | true   | C:\DeploymentsTemp\SSISToDeploy\Package1.dtsx           |
		|                      | Package2             | SSIS | true   | C:\DeploymentsTemp\SSISToDeploy\Package2.dtsx           |
		|                      | Package3             | SSIS | false  | C:\DeploymentsTemp\SSISToDeploy\Package3.dtsx           |
		|                      | Package4             | SSIS | true   | C:\DeploymentsTemp\SSISToDeploy\Package4.dtsx           |
		|                      | Package5             | SSIS | true   | C:\DeploymentsTemp\SSISToDeploy\Package5.dtsx           |
		| DW_DB_SERVER         | DW_DB_Script         | SQL  | true   | C:\DeploymentsTemp\SQLToDeploy\DW_DB_Script.sql         |
		| DW_STAGING_DB_SERVER | DW_STAGING_DB_Script | SQL  | false  | C:\DeploymentsTemp\SQLToDeploy\DW_STAGING_DB_Script.sql |
		| CONFIG_DB_SERVER     | CONFIG_DB_Script     | SQL  | true   | C:\DeploymentsTemp\SQLToDeploy\CONFIG_DB_Script.sql     |
		| LOGGING_DB_SERVER    | LOGGING_DB_Script    | SQL  | true   | C:\DeploymentsTemp\SQLToDeploy\LOGGING_DB_Script.sql    |
		| JOB_DB_SERVER        | JOB_DB_Script        | SQL  | true   | C:\DeploymentsTemp\SQLToDeploy\JOB_DB_Script.sql        |
	When I press on the deploy button

Scenario: Folder Structure Setup
	Then a folder should be created
	And it should be named with the ticket number
	And there should be a Source folder
	And there should be a Rollback folder
	And there should be a DeployScripts folder

Scenario: Source Folder Setup
	Then there should be a Source folder with a DB subfolder
	And there should be a Source folder with a MSDB subfolder
	And there should be a Source folder with a SSRS subfolder
	And there should be a Source folder with a SSAS subfolder
	And there should be a Source folder with a File subfolder

Scenario: Source DB Folder Setup
	Then there should be a Source\DB folder with a DW_DB_SERVER subfolder
	And there should be a Source\DB folder with a DW_STAGING_DB_SERVER subfolder
	And there should be a Source\DB folder with a CONFIG_DB_SERVER subfolder
	And there should be a Source\DB folder with a LOGGING_DB_SERVER subfolder
	And there should be a Source\DB folder with a JOB_DB_SERVER subfolder

Scenario: Rollback Folder Setup
	Then there should be a Rollback folder with a DB subfolder
	And there should be a Rollback folder with a MSDB subfolder
	And there should be a Rollback folder with a SSRS subfolder
	And there should be a Rollback folder with a SSAS subfolder
	And there should be a Rollback folder with a File subfolder

Scenario: Rollback DB Folder Setup
	Then there should be a Rollback\DB folder with a DW_DB_SERVER subfolder
	And there should be a Rollback\DB folder with a DW_STAGING_DB_SERVER subfolder
	And there should be a Rollback\DB folder with a CONFIG_DB_SERVER subfolder
	And there should be a Rollback\DB folder with a LOGGING_DB_SERVER subfolder
	And there should be a Rollback\DB folder with a JOB_DB_SERVER subfolder
	

Scenario: Move SSIS Packages	
	Then only packages marked for deployment should be placed in the manifest
	And the packages should be deployed to the Source\MSDB and in the correct subpath folder


Scenario: Move SQL Scripts
	Then only the SQL scripts marked for deployment should be placed in the manifest
	And the SQL scripts for the DW_DB_SERVER should be deployed to the Source\DB\DW_DB_SERVER folder

Scenario: DeployScripts Moved
	Then the deploy.ps1 file should be in the DeployScripts folder
	And the deploymentFunctions.ps1 file should be in the DeployScripts folder
	And the Deploy.bat file should be in the Deployment folder

Scenario: DeployVariable Created
	Then the deploymentVariables.ps1 file should be created in the DeploymentScripts folder
#	And it should have the correct PROJECT_NAME variable
