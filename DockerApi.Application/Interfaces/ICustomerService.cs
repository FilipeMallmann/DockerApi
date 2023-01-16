﻿using DockerApi.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Application.Interfaces
{
    public interface ICustomerService
    {
       CustomerFullViewModel Create(CustomerPostViewModel comment);
        CustomerGetViewModel GetById(Guid id);
        IEnumerable<CustomerGetViewModel> GetAll();
        CustomerFullViewModel Update(Guid id, CustomerFullViewModel comment);
        bool Delete(Guid id);
    }
}
