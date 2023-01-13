using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Application.Services
{
    public class CustomerService : ICustomerService
    {
        public CustomerFullViewModel Create(CustomerFullViewModel comment)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CustomerFullViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public CustomerFullViewModel GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public CustomerFullViewModel Update(Guid id, CustomerFullViewModel comment)
        {
            throw new NotImplementedException();
        }
    }
}
