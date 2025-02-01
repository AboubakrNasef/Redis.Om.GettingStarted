using Redis.OM.Modeling;

namespace Redis.OM.Model;

[Document(StorageType = StorageType.Json, Prefixes = new []{"Person"})]
public class Person
{
    [RedisIdField] [Indexed]public string Id { get; set; }
    [Indexed] public string FirstName { get; set; }
    [Indexed] public string LastName { get; set; }

    [Indexed] public int Age { get; set; }

    [Searchable] public string EmailAddress { get; set; }

    
    [Indexed(CascadeDepth = 1)]
    public Address? Address { get; set; }
    
}