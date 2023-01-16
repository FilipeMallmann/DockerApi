using AutoFixture;
using DockerApi.Api.Controllers;
using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DockerApi.Tests.Customer
{
    public class CustomerControllerTests
    {
        private readonly CustomerController _sut;
        private readonly ICustomerService _customerServiceMoq = Substitute.For<ICustomerService>();
        private readonly ILogger<CustomerController> _loggingMoq = Substitute.For<ILogger<CustomerController>>();

        private readonly Fixture _fixture;
        public CustomerControllerTests()
        {
            _sut = new CustomerController(_loggingMoq, _customerServiceMoq);
            _fixture = new Fixture();
        }

        [Fact]
        public void Post_MustReturnError_WhenEmail_IsNull()
        {
            //Arrange
            var customer = _fixture.Create<CustomerPostViewModel>();
            _customerServiceMoq.Create(customer).ReturnsNull();

            //Act
            var postValidation = _sut.Post(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public void Post_MustReturnError_WhenLastName_IsNull()
        {
            //Arrange
            var customer = _fixture.Build<CustomerPostViewModel>()
                .Without(c => c.LastName)
                .Create();

            _customerServiceMoq.Create(customer).ReturnsNull();

            //Act
            var postValidation = _sut.Post(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public void Post_MustReturnError_WhenFirstName_IsNull()
        {
            //Arrange
            var customer = _fixture.Build<CustomerPostViewModel>()
                .Without(c => c.FirstName)
                .Create();
            _customerServiceMoq.Create(customer).ReturnsNull();

            //Act
            var postValidation = _sut.Post(customer);
            var badRequestResult = Assert.IsType<BadRequestResult>(postValidation.Result);

            //Assert
            Assert.Equal(badRequestResult.StatusCode, new BadRequestResult().StatusCode);
        }

        [Fact]
        public void GetAll_Returns_Existing_Customers()
        {
            //Arrange
            var expected = _fixture.CreateMany<CustomerGetViewModel>().ToList();


            //Act
            _customerServiceMoq.GetAll().Returns(expected);
            var result = _sut.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            //Assert
            Assert.Equal(expected.Count, (okResult.Value as List<CustomerGetViewModel>).Count);


        }

        [Fact]
        public void GetById_Returns_Existing_Customer()
        {
            //Arrange
            var customerId = Guid.NewGuid();

            var expectedCustomer = _fixture.Create<CustomerGetViewModel>();
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


            var expectedCustomer = _fixture.Create<CustomerPostViewModel>();

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
            var customerEdit = _fixture.Create<CustomerPostViewModel>();


            //Act
            _customerServiceMoq.Update(Arg.Any<Guid>(), customerEdit).ReturnsNull();
            var customer = _sut.Put(Guid.NewGuid(), customerEdit)?.Value;

            //Assert
            Assert.Null(customer);
        }

    }
}