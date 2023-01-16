
using AutoFixture;
using DockerApi.Api;
using DockerApi.Application.ViewModels;
using DockerApi.IntegrationTests;
using FluentAssertions;
using System.Net.Mail;
using Xunit;


namespace IntegrationTests
{
    public class BasicTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Fixture _fixture;
        private readonly HttpClient _client;

        public BasicTests(CustomWebApplicationFactory<Program> factory)
        {
            _fixture = new Fixture();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
        {
            //arrange
            var customer = _fixture.Build<CustomerFullViewModel>()
               .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
               .Create();

            // Act
            await _client.PostAsJsonAsync("/Customer", customer);

            var response = await _client.GetFromJsonAsync<IEnumerable<CustomerFullViewModel>>("/Customer");

            // Assert
            response.Should().NotBeNull();

        }

        [Fact]
        public async Task ShouldCreateCustomer()
        {
            // Arrange          
            var customer = _fixture.Build<CustomerFullViewModel>()
                  .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                  .Create();


            // Act
            var response = await _client.PostAsJsonAsync("/Customer", customer);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();

            created.Should().NotBeNull();
            created.Id.Should().NotBeEmpty();
        }
        [Fact]
        public async Task ShouldCreateUpdateAndDeleteCustomer()
        {
            // Arrange
            var customer = _fixture.Build<CustomerPostViewModel>()
                 .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                 .Create();

            // Act
            var response = await _client.PostAsJsonAsync("/Customer", customer);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<CustomerGetViewModel>();

            // Act
            created.FirstName = _fixture.Create<string>();
            response = await _client.PutAsJsonAsync("/Customer", created);

            // Assert
            response.EnsureSuccessStatusCode();



            response = await _client.GetAsync($"/Customer/{created.Id}");
            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();
            Assert.Equal(updated.FirstName, created.FirstName);

            response = await _client.DeleteAsync($"/Customer{created.Id}");
            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync($"/Customer/{created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task ShouldCreateAndDeleteCustomer()
        {
            //create a customer
            var customer = _fixture.Build<CustomerFullViewModel>()
                 .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                 .Create();
            var response = await _client.PostAsJsonAsync("/Customer", customer);
            response.EnsureSuccessStatusCode();
            //get the customer
            var created = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();

            //delete the customer
            response = await _client.DeleteAsync($"/Customer/{created.Id}");
            response.EnsureSuccessStatusCode();


        }

    }
}