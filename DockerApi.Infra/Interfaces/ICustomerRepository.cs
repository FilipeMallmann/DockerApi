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
        Customer Create(Customer customer);
        bool Delete(Guid id);
        Customer Get(Guid id);
        IEnumerable<Customer> GetAll();
        Customer Update(Customer customer);
        IEnumerable<Customer> GetByPostId(Guid postId);
    }
}
