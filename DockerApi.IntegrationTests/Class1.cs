
using AutoFixture;
using DockerApi.Api;
using DockerApi.Application.ViewModels;
using DockerApi.IntegrationTests;
using FluentAssertions;
using System.Net.Http.Json;
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
            await _client.PostAsJsonAsync("/Customer/Create", customer);

            var response = await _client.GetFromJsonAsync<IEnumerable<CustomerFullViewModel>>("/api/Customer/List");

            // Assert
            Assert.Single(response);

        }

        [Fact]
        public async Task ShouldCreateCustomer()
        {
            // Arrange          
            var customer = _fixture.Build<CustomerFullViewModel>()
                  .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                  .Create();


            // Act
            var response = await _client.PostAsJsonAsync("/Customer/Create", customer);

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
            var customer = _fixture.Build<CustomerFullViewModel>()
                 .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                 .Create();

            // Act
            var response = await _client.PostAsJsonAsync("/Customer/Create", customer);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();

            // Act

            created.FirstName = _fixture.Create<string>();
            response = await _client.PutAsJsonAsync("/Customer/Update", created);

            // Assert
            response.EnsureSuccessStatusCode();



            response = await _client.GetAsync($"/Customer/Get?id={created.Id}");
            response.EnsureSuccessStatusCode();

            var updated = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();
            Assert.Equal(updated.FirstName, created.FirstName);

            response = await _client.DeleteAsync($"/Customer?id={created.Id}");
            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync($"/Customer/Get?id={created.Id}");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task ShouldCreateAndDeleteCustomer()
        {
            //create a customer
            var customer = _fixture.Build<CustomerFullViewModel>()
                 .With(c => c.Email, _fixture.Create<MailAddress>().ToString())
                 .Create();
            var response = await _client.PostAsJsonAsync("/api/Customer/Create", customer);
            response.EnsureSuccessStatusCode();
            //get the customer
            var created = await response.Content.ReadFromJsonAsync<CustomerFullViewModel>();

            //delete the customer
            response = await _client.DeleteAsync($"/api/Customer?id={created.Id}");
            response.EnsureSuccessStatusCode();


        }

    }
}