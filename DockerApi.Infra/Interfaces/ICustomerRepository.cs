using DockerApi.Domain.Entities;

namespace DockerApi.Infra.Interfaces
{
    public interface ICustomerRepository
    {
        Customer Create(Customer customer);
        bool Delete(Guid id);
        Customer Get(Guid id);
        IEnumerable<Customer> GetAll();
        Customer Update(Customer customer);
    }
}
