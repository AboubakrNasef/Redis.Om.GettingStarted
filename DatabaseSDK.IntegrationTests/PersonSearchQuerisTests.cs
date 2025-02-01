using AutoBogus;
using DatabaseSDK.IQueries;
using DatabaseSDK.Queries;
using Redis.OM;
using Redis.OM.Model;
using Redis.OM.Searching;
using Shouldly;
using Testcontainers.Redis;

namespace DatabaseSDK.IntegrationTests
{
    public class PersonSearchQuerisTests : IAsyncLifetime
    {
        private IPersonSearchQueries _personSearchQueris;
        private RedisContainer _redisContainer;
        private RedisConnectionProvider _provider;
        private IRedisCollection<Person> _personsCollection;
        private readonly AutoFaker<Person> _personFaker;
        public PersonSearchQuerisTests()
        {
            _redisContainer = new RedisBuilder()
                  .WithImage("redis/redis-stack")
                  .WithExposedPort(6379)
                  .Build();
            _personFaker = new AutoFaker<Person>();
            _personFaker.RuleFor(fake => fake.Id, fake => fake.Random.Guid().ToString())
                .RuleFor(f=>f.FirstName,f=>f.Name.FirstName())
                .RuleFor(f=>f.LastName,f=>f.Name.LastName())
                .RuleFor(f=>f.EmailAddress,f=>f.Internet.Email());
        }

        public async Task InitializeAsync()
        {

            await _redisContainer.StartAsync();

            // Get the connection string for the Redis container
            var connectionString = _redisContainer.GetConnectionString();

            // Initialize Redis OM
            _provider = new RedisConnectionProvider($"redis://{connectionString}");
            await _provider.Connection.CreateIndexAsync(typeof(Person));
            _personsCollection = _provider.RedisCollection<Person>();
            _personSearchQueris = new PersonSearchQueries(_provider);
        }

        public async Task DisposeAsync()
        {
            await _redisContainer.StopAsync();
        }


        [Fact]
        public async Task Test_AddPersonAsync()
        {
            var fakePerson = GenerateFakePersons(1)[0];
            await _personSearchQueris.AddPersonAsync(fakePerson);
            var retrievedPerson = await _personsCollection.Where(p => p.Id == fakePerson.Id).FirstOrDefaultAsync();
            retrievedPerson.ShouldNotBeNull();
            retrievedPerson.FirstName.ShouldBe(fakePerson.FirstName);
        }

        [Fact]
        public async Task Test_GetAllPersonsAsync()
        {
            var fakePersons = GenerateFakePersons(5);
            foreach (var person in fakePersons)
            {
                await _personSearchQueris.AddPersonAsync(person);
            }

            var allPersons = await _personsCollection.ToListAsync();
            allPersons.ShouldNotBeNull();
            allPersons.Count().ShouldBeGreaterThanOrEqualTo(5);
        }

        [Fact]
        public async Task Test_SearchPersonsAsync()
        {
            var fakePersons = GenerateFakePersons(15);
            foreach (var person in fakePersons)
            {
                await _personSearchQueris.AddPersonAsync(person);
            }
            var searchTerm = fakePersons[0].FirstName[..2];
            var resultsExpected = fakePersons.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm) || p.EmailAddress.Contains(searchTerm)).ToList();
            var searchResults = await _personSearchQueris.SearchPersonsAsync(searchTerm);
            searchResults.ShouldBeEquivalentTo(resultsExpected);
        }

        #region Private Methods


        private List<Person> GenerateFakePersons(int count)
        {

            return _personFaker.Generate(count);

        }

        #endregion
    }
}