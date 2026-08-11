
using Crayon.Entity.Common;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crayon.Services.Repository.Interface
{
    public interface IUserRepository
    {
        Task <Result<UserResponse>> Authorize(UserRequest request);
        Task <Result<UserResponse>> Register(EmployeeRequest request,string Role);
        Task <bool> CheckPassword(string email,string Password);
        //Task<Result<List<EmployeeResponse>>> GetVisibleEmployees(string userId);
        Task<Result<List<EmployeeResponse>>> GetVisibleEmployees();

    }
}
