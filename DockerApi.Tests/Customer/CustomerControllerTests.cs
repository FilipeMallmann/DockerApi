using DockerApi.Api.Controllers;
using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Security.Cryptography.X509Certificates;

namespace DockerApi.Tests.Customer
{
    public class CustomerControllerTests
    {
        private readonly CustomerController _sut;
        private readonly ICustomerService _customerServiceMoq = Substitute.For<ICustomerService>();
        private readonly ILogger<CustomerController> _loggingMoq = Substitute.For<ILogger<CustomerController>>();


        public CustomerControllerTests()
        {
            _sut = new CustomerController(_loggingMoq, _customerServiceMoq);
        }

        [Fact]
        public void GetAll_Returns_Existing_Customers()
        {
            //Arrange
            List<CustomerFullViewModel> expected = new();
            expected.Add(new CustomerFullViewModel() { FirstName = "First_Name1", LastName = "Last_Name", Email = "Email@domain.com", Password = "AnyPass" });
            expected.Add(new CustomerFullViewModel() { FirstName = "First_Name2", LastName = "Last_Name", Email = "Email@domain.com", Password = "AnyPass" });
            expected.Add(new CustomerFullViewModel() { FirstName = "First_Name3", LastName = "Last_Name", Email = "Email@domain.com", Password = "AnyPass" });
            expected.Add(new CustomerFullViewModel() { FirstName = "First_Name4", LastName = "Last_Name", Email = "Email@domain.com", Password = "AnyPass" });


            //Act
            _customerServiceMoq.GetAll().Returns(expected);
            var result = _sut.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            //Assert
            Assert.Equal(expected.Count, (okResult.Value as List<CustomerFullViewModel>).Count);


        }

        [Fact]
        public void GetById_Returns_Existing_Customer()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var expectedCustomer = new CustomerFullViewModel()
            {
                Id = customerId,
                Email = "Any string",
                FirstName = "Any string",
                LastName = "Any string",
                Password = "Any string",

            };
            _customerServiceMoq.GetById(customerId).Returns(expectedCustomer);

            //Act
            var actualCustomer = _sut.Get(customerId);
            var okObjectResult = Assert.IsType<OkObjectResult>(actualCustomer.Result);

            //Assert
            Assert.Equal(expectedCustomer, okObjectResult.Value);

        }

        [Fact]
        public void Get_MustReturnNull_WhenCustomer_DontExist()
        {
            //Arrange
            _customerServiceMoq.GetById(Arg.Any<Guid>()).ReturnsNull();

            //Act
            var customer = _sut.Get(Guid.NewGuid())?.Value;

            //Assert
            Assert.Null(customer);

        }


        [Fact]
        public void Delete_MustReturnFalse_WhenCustomer_DontExist()
        {
            //Arrange
            var customerId = Guid.NewGuid();

            _customerServiceMoq.Delete(customerId).Returns(false);

            //Act
            var customer = _sut.Delete(customerId);
            var notFound = Assert.IsType<NotFoundObjectResult>(customer.Result);
            //Assert
            Assert.Equal(new NotFoundResult().StatusCode, notFound.StatusCode);

        }
        [Fact]
        public void Edit_MustReturnNewCustomer_WhenCustomer_Exist()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var expectedCustomer = new CustomerFullViewModel()
            {
                Id = customerId,
                Email = "Any string",
                FirstName = "Any string",
                LastName = "Any string",
                Password = "Any string",
            };

            //Act
            _customerServiceMoq.Update(customerId, expectedCustomer).Returns(expectedCustomer);
            var actualPost = _sut.Put(customerId, expectedCustomer);
            var okObjectResult = Assert.IsType<OkObjectResult>(actualPost.Result);

            //Assert
            Assert.Equal(expectedCustomer, okObjectResult.Value);

        }

        [Fact]
        public void Edit_MustReturnNull_WhenCustomer_NotExist()
        {

            //Arrange
            var customerEdit = new CustomerFullViewModel()
            {
                FirstName = "Any value",
                LastName = "Any value",
                Email = "Any value",
                Password = "Any value"
            };


            //Act
            _customerServiceMoq.Update(Arg.Any<Guid>(), customerEdit).ReturnsNull();
            var customer = _sut.Put(Guid.NewGuid(), customerEdit)?.Value;

            //Assert
            Assert.Null(customer);
        }

    }
}