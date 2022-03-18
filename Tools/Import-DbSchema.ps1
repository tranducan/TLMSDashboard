param (
    # Specifies the connection string to an instance of the ModuleAssembly database
    # to re-generate the data model from. 
    [Parameter(Mandatory = $false)]
    [string]
    $ConnectionString = "Data Source=DESKTOP-M6N0IBR\SQLEXPRESS;Initial Catalog=ERPSOFT;Integrated Security=SSPI;Application Name=DBGenerationCommand",

    # Specifies the location to write the output files to.
    [Parameter(Mandatory = $false)]
    [string]
    $OutputPath = "EFCore",

    # Specifies the name of the EntityFrameworkCore provider to use.
    [Parameter(Mandatory = $false)]
    [string]
    $ProviderName = "Microsoft.EntityFrameworkCore.SqlServer",

    # Specifies the project to modify.
    [Parameter(Mandatory = $false)]
    [string]
    $Project = "..\EFTechlink\",

    # Specifies the name of the context being generated.
    [Parameter(Mandatory = $false)]
    [string]
    $ContextName = "TLMSDataContext2"
)

$tables =
    "ProcessHistory.DailyPerformanceGoal"
    

$commandArgs =
    "dbcontext", "scaffold", $ConnectionString, $ProviderName,
    "--context", $ContextName, $DataContextName, "--force", "--output-dir", $OutputPath,
    "--json", "--project", $Project, "--startup-project", $Project, "--no-build"

$commandArgs += $tables | ForEach-Object { "--table", $_ }

Write-Output $commandArgs

dotnet ef $commandArgs
