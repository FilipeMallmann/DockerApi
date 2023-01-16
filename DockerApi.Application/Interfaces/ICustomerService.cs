using DockerApi.Application.ViewModels;

namespace DockerApi.Application.Interfaces
{
    public interface ICustomerService
    {
        CustomerFullViewModel Create(CustomerPostViewModel comment);
        CustomerGetViewModel GetById(Guid id);
        IEnumerable<CustomerGetViewModel> GetAll();
        CustomerPostViewModel Update(Guid id, CustomerPostViewModel comment);
        bool Delete(Guid id);
    }
}
