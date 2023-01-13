using Microsoft.AspNetCore.Mvc;

namespace DockerApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        [HttpGet]
        public IActionResult GetAsync()
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public IActionResult PostAsync()
        {
            throw new NotImplementedException();
        }
        [HttpPut]
        public IActionResult UpdateAsync()
        {
            throw new NotImplementedException();
        }
        [HttpDelete]
        public IActionResult DeleteAsync()
        {
            throw new NotImplementedException();
        }


    }
}
