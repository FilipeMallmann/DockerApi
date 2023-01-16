using DockerApi.Domain.Entities;
using DockerApi.Infra.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DockerApi.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly DockerApiDbContext _dbContext;

        public CustomerRepository(DockerApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public Customer Create(Customer customer)
        {
            try
            {
                _dbContext.Customers.Add(customer);
                _dbContext.SaveChanges();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                var customer = _dbContext.Customers.FirstOrDefault(f => f.Id == id);
                _dbContext.Customers.Remove(customer);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Customer Get(Guid id)
        {
            try
            {
                return _dbContext.Customers.AsNoTracking().FirstOrDefault(f => f.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            try
            {
                return _dbContext.Customers;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Customer Update(Customer customer)
        {
            try
            {

                _dbContext.Entry(customer).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return customer;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
