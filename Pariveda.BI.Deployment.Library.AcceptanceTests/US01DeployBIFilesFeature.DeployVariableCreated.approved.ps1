#*==========================================================================================
#* FileName: DeploymentVariables.ps1
#*==========================================================================================
#* Script Name: Deployment Variables
#* Created: 9/30/2011
#* Author: Will Smith (Pariveda Solutions)
#*==========================================================================================
#* Purpose:
#*   This script is where developers will customize the deployment 
#*     by changing appropriate variables.
#*==========================================================================================
#* Dependencies: referenced by Deploy.ps1, Rollback.ps1, DeploymentFunctions.ps1
#*==========================================================================================
#* Execution: Do not execute directly.
#*==========================================================================================

#*==========================================================================================
#* REVISION HISTORY
#*==========================================================================================
#* 10/03/2011   Will Smith                Added comment blocks
#*==========================================================================================


# Used for logging only
$Global:PROJECT_NAME="Project"
$Global:TFS_LABEL="MY TFS LABEL"


# Server names by environment. 
# Every unique service should have its own entry in these hash lists.
#
# If new servers need to be added, the hash name must match exactly 
#    the reference in $DB_DEPLOY_SCRIPTS and $DB_ROLLBACK_SCRIPTS
#
# Hash keys for databases (e.g. DW_DB_SERVER) are not directly referenced by the runtime, so any name will do.
# However, SSIS_SERVER and FILE_SERVER are referenced directly by the runtime
#
# SERVER_DEFINITION fields (Type and Description) are referenced directly
# Entries must be of this pattern:
#    <SERVER_KEY> = (new-object psobject -property @{Type=<SERVER_TYPE>;Description=<SERVER_DESC>});
#    <SERVER_TYPE> = DB | SSIS | FILE
#    <SERVER_DESC> is used to enhance logging: 'The <SERVER_DESC> installation will occur on <SERVER_NAME>.'
$Global:SERVER_DEFINITIONS = @{
    DW_DB_SERVER         = (new-object psobject -property @{Type="DB";Description="data warehouse database"});
    DW_STAGING_DB_SERVER = (new-object psobject -property @{Type="DB";Description="data warehouse staging database"});
    CONFIG_DB_SERVER     = (new-object psobject -property @{Type="DB";Description="configuration database"});
    LOGGING_DB_SERVER    = (new-object psobject -property @{Type="DB";Description="logging database"});
    JOB_DB_SERVER        = (new-object psobject -property @{Type="DB";Description="SQL Agent job"});
    SSIS_SERVER          = (new-object psobject -property @{Type="SSIS";Description="SSIS"});
    FILE_SERVER          = (new-object psobject -property @{Type="File";Description="data file"})
}

$Global:DEV_SERVERS = @{
    DW_DB_SERVER         = "HBSWDB12\DWDEVDB";
    DW_STAGING_DB_SERVER = "HBSWDB12\DWDEVDB";
    CONFIG_DB_SERVER     = "HBDWDB09";
    LOGGING_DB_SERVER    = "HBDWDB09";
    JOB_DB_SERVER        = "HBDWDB09";
    SSIS_SERVER          = "HBDWDB09";
    FILE_SERVER          = "\\HBDWDB09"
}

$Global:TEST_SERVERS = @{
    DW_DB_SERVER         = "HBSWDB12\DWSTGDB";
    DW_STAGING_DB_SERVER = "HBSWDB12\DWSTGDB";
    CONFIG_DB_SERVER     = "HBSWDB11";
    LOGGING_DB_SERVER    = "HBSWDB11";
    JOB_DB_SERVER        = "HBSWDB11";
    SSIS_SERVER          = "HBSWDB11";
    FILE_SERVER          = "\\HBSWDB11"
}

$Global:PROD_SERVERS = @{
    DW_DB_SERVER         = "HBPWDB08";
    DW_STAGING_DB_SERVER = "HBPWDB08";
    CONFIG_DB_SERVER     = "HBPWDB06";
    LOGGING_DB_SERVER    = "HBPWDB06";
    JOB_DB_SERVER        = "HBPWDB06";
    SSIS_SERVER          = "HBPWDB06";
    FILE_SERVER          = "\\HBPWDB06"
}

# Full path from MSDB
$Global:SSIS_MSDB_FOLDER = "Gnosis Pricing"

# These scripts will run on the appropriate server in the order provided here
#
# entry pattern:
#    ,("<target server variable name>","<script file name>")
# 
# <target server variable name> = DW_DB_SERVER | DW_STAGING_DB_SERVER | CONFIG_DB_SERVER | LOGGING_DB_SERVER | JOB_DB_SERVER | SSIS_SERVER
#     (if new servers are added to DEV_SERVERS, TEST_SERVERS, etc. then they will be valid references here.)
#
# <script file name> = name of script file relative to the "<install path>\Source\DB\" path
#     The script file name may include known variables in the name (e.g. MyScript.DEV.sql or MyScript.TEST.sql)
#       In this case reference the variable with a back-tic (e.g. `$TARGET_ENV ==> MyScript.`$TARGET_ENV.sql)
#           entry example:  ,("CONFIG_DB_SERVER","DatabaseConfigurations.`$TARGET_ENV.sql")
#       Known variables will be resolved just before execution of the sql script.
#
$Global:DB_DEPLOY_SCRIPTS = @( 
    ,("DW_DB_SERVER","DW_DB_Script.sql")
    ,("DW_STAGING_DB_SERVER","DW_STAGING_DB_Script.sql")
    ,("CONFIG_DB_SERVER","CONFIG_DB_Script.`$TARGET_ENV.sql")
    ,("LOGGING_DB_SERVER","LOGGING_DB_Script.sql")
	,("JOB_DB_SERVER","JOB_DB_Script.`$TARGET_ENV.sql")
)

# These scripts will run on the appropriate server in the order provided here
#
# entry pattern:
#    ("<target server variable name>","<script file name>")
# 
# <target server variable name> = DB_SERVER | SSIS_SERVER
#
# <script file name> = name of script file relative to the "<install path>\Rollback\DB\" path
#
$Global:DB_ROLLBACK_SCRIPTS = @(
    #,("DW_DB_SERVER","FullUninstall.sql")
)

# These files/folders will be backed up from the $FILE_SERVER in the order provided here
#
# entry pattern:
#    ("<backup from>","<backup to>")
# 
# <backup from> = The rollback target relative to $FILE_SERVER\. 
#                 The target will be checked for existence before copying.
#
# <backup to> = The backup location relative to "<install path>\Rollback\File\"
#               An empty string will rollback directly to "<install path>\Rollback\File\"
#
# NOTE: for directory trees do not include the tree root in the <rollback to>
# e.g. CORRECT:     ("Gnosis_Automation\Pricing","")          # Yields "<install path>\Rollback\File\Pricing"
#      INCORRECT:   ("Gnosis_Automation\Pricing","Pricing")   # Yields "<install path>\Rollback\File\Pricing\Pricing"
#
$Global:FILE_BACKUP_COMMANDS = @(
    #,("Gnosis_Automation\Pricing","")
)

# These files/folders will be migrated to the $FILE_SERVER in the order provided here
#
# entry pattern:
#    ("<migrate from>","<migrate to>")
# 
# <migrate from> = The file source relative to "<install path>\Source\File\" 
#
# <migrate to> = The target location relative to "$FILE_SERVER\"
#
# NOTE: for directory trees do not include the tree root in the <migrate to>
# e.g. CORRECT:     ("Pricing","Gnosis_Automation")           # Yields "$FILE_SERVER\Gnosis_Automation\Pricing\"
#      INCORRECT:   ("Pricing","Gnosis_Automation\Pricing")   # Yields "$FILE_SERVER\Gnosis_Automation\Pricing\Pricing\"
#
$Global:FILE_MIGRATION_COMMANDS = @(
    #,("Pricing", "Gnosis_Automation")
)

# These files/folders will be restored to the $FILE_SERVER in the order provided here
#
# entry pattern:
#    ("<restore from>","<restore to>")
# 
# <restore from> = The restore source relative to "<install path>\Rollback\File\" 
#
# <restore to> = The target location relative to "$FILE_SERVER\"
#
# NOTE: This may always be the same settings as the $MIGRATION_FILES
#
# NOTE: for directory trees do not include the tree root in the <migrate to>
# e.g. CORRECT:     ("Pricing","Gnosis_Automation")           # Yields "$FILE_SERVER\Gnosis_Automation\Pricing\"
#      INCORRECT:   ("Pricing","Gnosis_Automation\Pricing")   # Yields "$FILE_SERVER\Gnosis_Automation\Pricing\Pricing\"
#
$Global:FILE_RESTORE_COMMANDS = @(
    #,("Pricing", "Gnosis_Automation")
)
