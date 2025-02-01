
## Tests Overview

### PersonSearchQuerisTests

This test class contains integration tests for the `IPersonSearchQueries` interface. It uses a Redis container to perform the tests.

#### Tests

- **Test_AddPersonAsync**: Tests the addition of a person to the Redis database.
- **Test_GetAllPersonsAsync**: Tests retrieving all persons from the Redis database.
- **Test_SearchPersonsAsync**: Tests searching for persons in the Redis database.

### Private Methods

- **GenerateFakePersons**: Generates a list of fake `Person` objects using `AutoFaker`.

## Dependencies

- [AutoBogus](https://github.com/nickdodd79/AutoBogus)
- [Redis.OM](https://github.com/redis/redis-om-dotnet)
- [Shouldly](https://github.com/shouldly/shouldly)
- [Testcontainers](https://github.com/testcontainers/testcontainers-dotnet)

