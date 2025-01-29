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

4. Run `\PaylocityBenefitsCalculator\PaylocityBenefits.Database\Scripts\SeedData.sql` 
	by using the following command in a terminal:

   `sqlcmd -S (localdb)\MSSQLLocalDB -d PaylocityBenefits -i "SeedData.sql"`

   This will create a local database, (localdb)\\MSSQLLocalDB.

   The default connection string for the DB is set in `\PaylocityBenefitsCalculator\Api\AppSettings.json`    
6. You can check to see if the database has tables correctly populated using SQL Server Management Studio



## Running Tests
The integration tests require the server to be running in order for the tests to pass.
1. In Visual Studio right click on the Api project, then select Debug > Start Without Debugging
2. Once the API is running you can run the tests in Api.Test by right clicking on the project and select Run/Debug Tests


## Implementation
### Controllers
Functionality was added to the provided Employee and Dependent controller class to include: 
- Retrieving employees and dependents by ID
- Retrieving all employees or dependents
- Adding a employee or dependent
- Retrieving an employees paycheck (detailed with cost breakdown) or just the net paycheck amount

With more time, other useful functionality that could be added includes:
- Delete employees and dependents
- Update employees and dependents

### Extensions
The MappingExtensions class handles mapping Data Transfer Objects to Model objects and vice versa. Since the model objects are small and did not have many properties it was easy to add this manually. If the number of objects and properties are high, a library like Autofac can be used for mapping.

### Services
A service layer was added to handle the business logic from incoming requests. This layer receives requests from controllers, calls the repository layer if needed, handles any business logic, and returns the result to the controller.
The service layer consists of Employee and Calculation services. 

The Employee service handles adding and retrieving Employee and Dependent information from the data access layer. 

The Calculation service handles calculating a paycheck given an employee ID. In order to be flexible, this service uses a strategy pattern to determine how the paycheck should be calculated. In the future if another calculation method is required, a new strategy can be added and generated from the strategy factory.

### Data Access Layer
The EmployeeRepository class handles database requests. I added functions that perform database requests using pagination but they are not exposed at the service or controller layers yet. Pagination is important when retrieving all Employee and Dependent records. For large databases with millions of records attempting to get all the records at once can overwhelm the database, server, and client.

## Test
### Unit Tests
Unit tests were added for the Employee and Calculation services and the DefaultBenefitCostStrategy to test their functionality. DefaultBenefitCostStrategyTests has tests to make sure the costs and paycheck calculations are performed correctly.
With more time, I would add more test cases for thoroughness and to cover more edge cases.

### Integration Tests
The provided integration tests cover end-to-end testing from the controller layer to the data layer. I did not add more tests here but the existing ones should pass. With more time, I would add tests to cover the paycheck calculation and adding/updating/deleting employees and dependents.

## Other Improvements
### Larger Dataset for Testing
I only added the few employee and dependent records show in the example to the SQL database creation script. Having more records would help test real-world scenarios as well as performance and load at scale. It would also be useful for testing pagination when retrieving a large number of records.

### Caching
If an employee paycheck or data is requested multiple times, caching the result in an in-memory cache could save the time of recalculating or retrieving the data. A distributed cache like Redis can be used if there are multiple application servers.
If employee data rarely changes and calculations become more expensive this would be beneficial. An invalidation strategy should be implemented as well for when the data changes. It could time-based, manual, or a combination of both. 
