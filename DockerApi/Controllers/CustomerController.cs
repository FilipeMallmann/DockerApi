using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DockerApi.Application.Interfaces;
using DockerApi.Application.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace DockerApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _service;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerGetViewModel>> GetAll()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while getting customer: {e.Message}");
                return BadRequest(e.Message);

            }
        }

        [HttpGet("{id:guid}")]
        public ActionResult<CustomerFullViewModel> Get([FromRoute] Guid id)
        {
            try
            {
                var customer = _service.GetById(id);
                if (customer is not null) return Ok(customer);
                return NotFound();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while searching for customer {id}: {e.Message}");
                throw;
            }
        }

        [HttpPost]
        public ActionResult<CustomerFullViewModel> Post([FromBody] CustomerPostViewModel customer)
        {
            try
            {
                var newComment = _service.Create(customer);
                if (newComment is null) return BadRequest();
                return Ok(newComment);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while creating customer: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:guid}")]
        public ActionResult<CustomerFullViewModel> Put([FromRoute][Required] Guid id, [FromBody] CustomerFullViewModel customer)
        {
            try
            {
                return Ok(_service.Update(id, customer));
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while editing customer: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        public ActionResult<bool> Delete([FromRoute][Required] Guid id)
        {
            try
            {
                var result = _service.Delete(id);
                return result ? Ok(true) : NotFound(false);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while deleting customer: {e.Message}");
                return BadRequest(e.Message);
            }
        }
    }
}