## Prerequisites
- Visual Studio 2022 or later
- SQL Server 2019 LocalDB (included with Visual Studio)
- .NET 6.0 SDK
- Entity Framework Core Tools (`dotnet tool install --global dotnet-ef`)


## Database Setup
1. Restore Packages: `dotnet restore`
2. Create/Update DB: from `\PaylocityBenefitsCalculator\Api\` run `dotnet ef database update` 
	If you get this error, "Could not execute because the specified command or file was not found.", make sure dotnet-ef CLI tool is installed:
	`dotnet tool install --global dotnet-ef`
3. Run `\PaylocityBenefitsCalculator\PaylocityBenefits.Database\Scripts\SeedData.sql` 
	by using the following command in a terminal:
   	`sqlcmd -S (localdb)\MSSQLLocalDB -d PaylocityBenefits -i "SeedData.sql"`
4. You can check to see if the database has tables correctly populated using SQL Server Management Studio
	



