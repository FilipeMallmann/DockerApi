using DockerApi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerFullViewModel> CreateAsync(CustomerPostViewModel comment);
        Task<CustomerGetViewModel> GetByIdAsync(Guid id);
        Task<IEnumerable<CustomerGetViewModel>> GetAllAsync();
        Task<CustomerFullViewModel> UpdateAsync(Guid id, CustomerFullViewModel comment);
        Task<bool> DeleteAsync(Guid id);
    }
}
