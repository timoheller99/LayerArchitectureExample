# LayerArchitectureExample
# About this project
This is a repository containing a small example for a layered architecture approach.

It's in an early stage right now and will be developed further in the future.

# Technologies

* .NET 6
* C# 10

### DataAccess
* MySQL Database
* Dapper

### Logging / Metrics
* Serilog
* ElasticSearch

### Validation
* FluentValidation

### Tests
* xUnit
* FluentAssertions
* DotNet.Testcontainers

# Roadmap
The current stage is a very early stage of the development.
I have a lot of ideas to implement in the future.

# Try it out yourself

1. Clone the repository and made sure it compiles
2. Make sure that docker is running to set up the local test environment
3. To set up the local test environment, execute the bash script `scripts/init.sh` or `scripts/reset.sh` to setup the local test environment.

* Currently there are only a database and ElasticSearch/Kibana server available. So you can connect to the database with localhost:3306 and have a look at the database or execute the integration tests and look at the logs in [Kibana](http://localhost:5601/app/discover)
