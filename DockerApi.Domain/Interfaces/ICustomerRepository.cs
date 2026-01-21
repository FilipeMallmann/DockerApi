using DockerApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DockerApi.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<bool> DeleteAsync(Guid id);
        Task<Customer> GetAsync(Guid id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> UpdateAsync(Customer customer);
    }
}
