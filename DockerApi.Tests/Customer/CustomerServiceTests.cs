using AutoFixture;
using DockerApi.Application.Services;
using DockerApi.Application.ViewModels;
using DockerApi.Infra.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace DockerApi.Tests.Customer
{
    public class CustomerServiceTests
    {
        private readonly ICustomerRepository _repository;
        private readonly CustomerService _customerService;
        private readonly Fixture _fixture;
        public CustomerServiceTests()
        {
            _repository = Substitute.For<ICustomerRepository>();
            _customerService = new CustomerService(_repository);
            _fixture = new Fixture();
        }
        [Fact]
        public void Create_ShouldCreateNewCustomer()
        {
            // Arrange
            var customer = _fixture.Create<CustomerPostViewModel>();


            var customerRecord = _fixture.Create<Domain.Entities.Customer>();

            _repository.Create(Arg.Any<Domain.Entities.Customer>()).Returns(customerRecord);

            // Act
            var result = _customerService.Create(customer);

            // Assert
            _repository.Received(1).Create(Arg.Any<Domain.Entities.Customer>());

            customerRecord.Should().BeEquivalentTo(result);

        }

        [Fact]
        public void Update_ShouldUpdateExistingCustomer()
        {
            // Arrange
            var customerId = _fixture.Create<Guid>();
            var updatedCustomer = _fixture.Create<CustomerPostViewModel>();
            var customerRecord = _fixture.Create<Domain.Entities.Customer>();
            _repository.Get(customerId).Returns(customerRecord);

            // Act
            var result = _customerService.Update(customerId, updatedCustomer);

            // Assert
            updatedCustomer.Should().BeEquivalentTo(result);

        }

    }
}
