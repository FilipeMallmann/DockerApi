using DockerApi.Domain.Entities;
using DockerApi.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly DockerApiDbContext _dbContext;

        public CustomerRepository(DockerApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Customer> CreateAsync(Customer customer)
        {
            try
            {
                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var customer = _dbContext.Customers.FirstOrDefault(f => f.Id == id);
                _dbContext.Customers.Remove(customer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Customer> GetAsync(Guid id)
        {
            try
            {
                return await _dbContext.Customers.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                return _dbContext.Customers;
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            try
            {
                _dbContext.Entry<Customer>(customer).State = EntityState.Modified;
                await  _dbContext.SaveChangesAsync();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
