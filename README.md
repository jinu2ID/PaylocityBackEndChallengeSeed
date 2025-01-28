## Prerequisites
- Visual Studio 2022 or later
- SQL Server 2019 LocalDB (included with Visual Studio)
- .NET 6.0 SDK
- Entity Framework Core Tools (`dotnet tool install --global dotnet-ef`)


## Database Setup
1. Restore Packages: `dotnet restore`
2. Create/Update DB: `dotnet ef database update` 
3. Run `\PaylocityBenefitsCalculator\PaylocityBenefits.Database\Scripts\SeedData.sql` 
	to seed the database with initial data
	a. SqlPackage / SQLCMD / SSMS -> run SeedData.sql


