using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using DockerApi.Domain.Entities;
using DockerApi.Infra.Interfaces;


namespace DockerApi.Application.Services
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public CustomerFullViewModel Create(CustomerPostViewModel customer)
        {

            var customerRecord = new Customer()
            {
                Id = Guid.NewGuid(),
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Password = customer.Password
            };

            _customerRepository.Create(customerRecord);

            var createdComment = new CustomerFullViewModel()
            {
                Id = customerRecord.Id,
                Email = customerRecord.Email,
                FirstName = customerRecord.FirstName,
                LastName = customerRecord.LastName,
                Password = customerRecord.Password
            };
            return createdComment;
        }

        public bool Delete(Guid id)
        {
            return _customerRepository.Delete(id);
        }

        public IEnumerable<CustomerGetViewModel> GetAll()
        {
            var customerList = _customerRepository.GetAll();

            return MapComments(customerList);
        }

        public CustomerGetViewModel GetById(Guid id)
        {
            var customerRecord = _customerRepository.Get(id);

            if (customerRecord is object)
            {
                var customer = new CustomerGetViewModel()
                {
                    Id = customerRecord.Id,
                    FirstName = customerRecord.FirstName,
                    LastName = customerRecord.LastName,
                    Email = customerRecord.Email

                };
                return customer;
            }
            else
            {
                return null;
            }
        }

        public CustomerPostViewModel Update(Guid id, CustomerPostViewModel customer)
        {
            var customerRecord = _customerRepository.Get(id);
            if (customerRecord is not null)
            {
                var updtCustomer = new Customer()
                {
                    Id = customerRecord.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Password = customer.Password
                };
                _customerRepository.Update(updtCustomer);
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

        private IEnumerable<CustomerGetViewModel> MapComments(IEnumerable<Customer> customertList)
        {
            var customers = new List<CustomerGetViewModel>();
            foreach (var customer in customertList)
            {
                customers.Add(new CustomerGetViewModel()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                });
            }
            return customers;
        }
    }
}
