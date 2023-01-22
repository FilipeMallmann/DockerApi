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
        public async Task Post_MustReturnError_WhenEmail_IsNull()
        {
            //Arrange
            var customer = new CustomerPostViewModel()
            {
                FirstName = "Any string",
                LastName = "Any string",
                Password = "Any string",

            };
            _customerServiceMoq.CreateAsync(customer).ReturnsNull();

            //Act
            var postValidation = await _sut.PostAsync(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public async Task Post_MustReturnError_WhenLastName_IsNull()
        {
            //Arrange
            var customer = new CustomerPostViewModel()
            {
                FirstName = "Any string",
                Email = "Any string",
                Password = "Any string",

            };
            _customerServiceMoq.CreateAsync(customer).ReturnsNull();

            //Act
            var postValidation = await _sut.PostAsync(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public async Task Post_MustReturnError_WhenFirstName_IsNull()
        {
            //Arrange
            var customer = new CustomerPostViewModel()
            {
                LastName = "Any string",
                Email= "Any string",
                Password = "Any string",

            };
            _customerServiceMoq.CreateAsync(customer).ReturnsNull();

            //Act
            var postValidation = await _sut.PostAsync(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public async Task GetAll_Returns_Existing_Customers()
        {
            //Arrange
            List<CustomerGetViewModel> expected = new();
            expected.Add(new CustomerGetViewModel() { FirstName = "First_Name1", LastName = "Last_Name", Email = "Email@domain.com"});
            expected.Add(new CustomerGetViewModel() { FirstName = "First_Name2", LastName = "Last_Name", Email = "Email@domain.com"});
            expected.Add(new CustomerGetViewModel() { FirstName = "First_Name3", LastName = "Last_Name", Email = "Email@domain.com"});
            expected.Add(new CustomerGetViewModel() { FirstName = "First_Name4", LastName = "Last_Name", Email = "Email@domain.com"});


            //Act
             _customerServiceMoq.GetAllAsync().Returns(expected);
            var result = await _sut.GetAllAsync();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            //Assert
            Assert.Equal(expected.Count, (okResult.Value as List<CustomerGetViewModel>).Count);


        }

        [Fact]
        public async Task GetById_Returns_Existing_Customer()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            var expectedCustomer = new CustomerGetViewModel()
            {
                Id = customerId,
                Email = "Any string",
                FirstName = "Any string",
                LastName = "Any string"

            };
            _customerServiceMoq.GetByIdAsync(customerId).Returns(expectedCustomer);

            //Act
            var actualCustomer = await _sut.GetAsync(customerId);
            var okObjectResult = Assert.IsType<OkObjectResult>(actualCustomer.Result);

            //Assert
            Assert.Equal(expectedCustomer, okObjectResult.Value);

        }

        [Fact]
        public async Task Get_MustReturnNull_WhenCustomer_DontExist()
        {
            //Arrange
            _customerServiceMoq.GetByIdAsync(Arg.Any<Guid>()).ReturnsNull();

            //Act
            var customer = await _sut.GetAsync(Guid.NewGuid());

            //Assert
            Assert.Null(customer.Value);

        }


        [Fact]
        public async Task Delete_MustReturnFalse_WhenCustomer_DontExist()
        {
            //Arrange
            var customerId = Guid.NewGuid();

            _customerServiceMoq.DeleteAsync(customerId).Returns(false);

            //Act
            var customer = await _sut.DeleteAsync(customerId);
            var notFound = Assert.IsType<NotFoundObjectResult>(customer.Result);
            //Assert
            Assert.Equal(new NotFoundResult().StatusCode, notFound.StatusCode);

        }
        [Fact]
        public async Task Edit_MustReturnNewCustomer_WhenCustomer_Exist()
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
            _customerServiceMoq.UpdateAsync(customerId, expectedCustomer).Returns(expectedCustomer);
            var actualPost = await _sut.PutAsync(customerId, expectedCustomer);
            var okObjectResult = Assert.IsType<OkObjectResult>(actualPost.Result);

            //Assert
            Assert.Equal(expectedCustomer, okObjectResult.Value);

        }

        [Fact]
        public async Task Edit_MustReturnNull_WhenCustomer_NotExist()
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
            _customerServiceMoq.UpdateAsync(Arg.Any<Guid>(), customerEdit).ReturnsNull();
            var customer = await _sut.PutAsync(Guid.NewGuid(), customerEdit);

            //Assert
            Assert.Null(customer.Value);
        }

    }
}