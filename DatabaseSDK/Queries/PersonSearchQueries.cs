﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseSDK.IQueries;
using Redis.OM;
using Redis.OM.Model;
using Redis.OM.Searching;

namespace DatabaseSDK.Queries
{
    public class PersonSearchQueries : IPersonSearchQueries
    {
        private readonly RedisConnectionProvider _provider;
        private readonly IRedisCollection<Person> _persons;

        public PersonSearchQueries(RedisConnectionProvider provider)
        {
            _provider = provider;
            _persons = _provider.RedisCollection<Person>();
        }

        public async Task AddPersonAsync(Person person)
        {
            await _persons.InsertAsync(person);
        }

        public async Task DeletePersonAsync(string id)
        {
            await  _provider.Connection.UnlinkAsync($"Person:{id}");
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            return await _persons.ToListAsync();
        }

        public async Task<Person> GetPersonByIdAsync(string id)
        {
            return await _persons.Where(p=>p.Id==id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm)
        {
           
            return await _persons.Where(p => p.FirstName.Contains(searchTerm) || p.LastName.Contains(searchTerm) || p.EmailAddress.Contains(searchTerm)).ToListAsync();
        }

        public async Task UpdatePersonAsync(Person person)
        {
            await _persons.UpdateAsync(person);
        }
    }
}
