#SaltoKS Demo task

##The task
Create an API that would enable users, regardless of the machine they access the system (mobile, PC) to register and be able to access 2 doors, 1 that would open to all entrants and another to open for employees only

##About the approach
The way I approached the solution was to create the ability to create Door entries dynamically (not stick to the 2 in the example) and, depending of the permissions granted for the user, let them open the ones they've been allowed to.
Also added audit logging of the timestamp when access to a door was granted.

####Authorization/Authentication
Also included User registration and the user login is done using JwtTokens. Also have included refreshing tokens functionality.
Chose this approach to allow ease of API access for any device as the JwtToken is being sent in the headers and allows for reduced need for logins.

####Database
Initially I opted for a code-first approach while utilizing EntityFramework on a SQL DB, planning to go to Azure's CosmosDB and use MongoDB. Didn't manage to find time to make the migration to Azure's NoSql DB.

####Testing
Included a few xUnit integration tests on the DoorsController covering a few different scenarios. To save time, opted to use an InMemory DB implementation instead of Setup & Teardown which is the more reliable approach as it tests the full integration with the DB.