using DockerApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Infra.Interfaces
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
