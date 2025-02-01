using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Redis.OM.Model;

namespace DatabaseSDK.IQueries
{
    public interface IPersonSearchQueries
    {
        Task<Person> GetPersonByIdAsync(string id);
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task AddPersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(string id);
        Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm);
    }

}
