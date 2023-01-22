using DockerApi.Infra;
using Microsoft.EntityFrameworkCore;

namespace DockerApi.IntegrationTest
{


    internal class MyHttpBackEndFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            // var baseAdd = name;
            var http = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost.com:7239/"),
                Timeout = new TimeSpan(0, 5, 0),


            };
            return http;
        }
    }

    public class IntegrationTest

    {
        HttpClient http = new MyHttpBackEndFactory().CreateClient();


        [Fact]
        public void Integration_CustomerPOST()
        {


            //arrange 


            var options = new DbContextOptionsBuilder<DockerApiDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            string baseAdress = "https://localhost:7584/Customer/";



            using (var context = new DockerApiDbContext(options))
            {


            };
        }
    }
}