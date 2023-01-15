using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using DockerApi.Domain.Entities;
using DockerApi.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DockerApi.Application.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public CustomerFullViewModel Create(CustomerFullViewModel customer)
        {

            var customerRecord = new Customer() {
                Id = Guid.NewGuid(),
                Email = customer.Email,
                FirstName= customer.FirstName,
                LastName = customer.LastName,
                Password = customer.Password};

            _customerRepository.Create(customerRecord);

            // Consider using the TinyMapper...
            var createdComment = new CustomerFullViewModel() {
                Id = customerRecord.Id,
                Email = customerRecord.Email,
                FirstName = customerRecord.FirstName,
                LastName = customerRecord.LastName,
                Password = customerRecord.Password };
            return createdComment;
        }

        public bool Delete(Guid id)
        {
            return _customerRepository.Delete(id);
        }

        public IEnumerable<CustomerFullViewModel> GetAll()
        {
            var customerList = _customerRepository.GetAll();
            //tinyMapper
           
            return MapComments(customerList);
        }

        public CustomerFullViewModel GetById(Guid id)
        {
            var customerRecord = _customerRepository.Get(id);

            if (customerRecord is object)
            {
                var customer = new CustomerFullViewModel()
                {
                    Id = customerRecord.Id,
                    FirstName = customerRecord.FirstName,
                    LastName= customerRecord.LastName,
                    Email = customerRecord.Email,
                    Password = customerRecord.Password

                };
                return customer;
            }
            else
            {
                return null;
            }
        }

        public CustomerFullViewModel Update(Guid id, CustomerFullViewModel customer)
        {
            var customerRecord = _customerRepository.Get(id);
            if (customerRecord is not null)
            {
                var uptComment = new Customer() {
                    Id = customerRecord.Id,
                    FirstName = customerRecord.FirstName,
                    LastName = customerRecord.LastName,
                    Email = customerRecord.Email,
                    Password = customerRecord.Password
                };
                _customerRepository.Update(uptComment);
                customer.Id = customerRecord.Id;
                customer.Email = customerRecord.Email;
                customer.FirstName = customerRecord.FirstName;
                customer.LastName = customerRecord.LastName;

                return customer;
            }
            else
            {
                return null;
            }
        }

        private IEnumerable<CustomerFullViewModel> MapComments(IEnumerable<Customer> customertList)
        {
            var customers = new List<CustomerFullViewModel>();
            foreach (var customer in customertList)
            {
                customers.Add(new CustomerFullViewModel() { 
                  Id = customer.Id,
                  FirstName = customer.FirstName,
                  LastName= customer.LastName,
                  Email= customer.Email,
                  Password= customer.Password
                });
            }
            return customers;
        }
    }
}
