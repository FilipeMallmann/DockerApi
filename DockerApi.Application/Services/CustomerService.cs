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
        public async Task<CustomerFullViewModel> CreateAsync(CustomerPostViewModel customer)
        {
            var customerRecord = new Customer()
            {
                Id = Guid.NewGuid(),
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Password = customer.Password
            };

            await _customerRepository.CreateAsync(customerRecord);

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

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CustomerGetViewModel>> GetAllAsync()
        {
            var customerList = await  _customerRepository.GetAllAsync();

            return MapComments(customerList);
        }

        public async Task<CustomerGetViewModel> GetByIdAsync(Guid id)
        {
            var customerRecord = await _customerRepository.GetAsync(id);

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

        public async Task<CustomerFullViewModel> UpdateAsync(Guid id, CustomerFullViewModel customer)
        {
            var customerRecord = await _customerRepository.GetAsync(id);
            if (customerRecord is not null)
            {
                var updtCustomer = new Customer()
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Password = customer.Password
                };
                await _customerRepository.UpdateAsync(updtCustomer);

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
