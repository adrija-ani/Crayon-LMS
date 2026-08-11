using Crayon.Entity.Common;
using Crayon.Entity.Data;
using Crayon.Entity.Dto;
using Crayon.Entity.Models;
using Crayon.Services.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Crayon.Services.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(UserManager<IdentityUser> userManager, ApplicationDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credentials =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(
                            _configuration["Jwt:ExpiryMinutes"])),
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
        public async Task<Result<UserResponse>>Authorize(UserRequest request)
        {
            Result<UserResponse> result = new();
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    result.Error.Add( new Errors
                        { ErrorCode = 404,ErrorMessage = "User Not Found"
                        });
                    return result;
                }
                bool isValid = await _userManager.CheckPasswordAsync(user,request.Password);
                if (!isValid)
                {
                    result.Error.Add(
                        new Errors
                        {
                            ErrorCode = 401,
                            ErrorMessage = "Invalid Password"
                        });
                    return result;
                }
                string token = await GenerateToken(user);
                result.Response =
                    new UserResponse
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        Token = token
                    };
                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(
                    new Errors
                    {
                        ErrorCode = 500,
                        ErrorMessage = ex.Message
                    });
                return result;
            }
        }

        public async Task<bool> CheckPassword(string email,string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
            return await _userManager.CheckPasswordAsync(user,password);
        }

        public async Task<Result<UserResponse>> Register(EmployeeRequest request, string Role)
        {
            Result<UserResponse> userResult = new Result<UserResponse>();
            try
            {

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user != null)
                {
                    userResult.Error.Add(new Errors { ErrorCode = 200, ErrorMessage = "User Already Registered" });
                    return userResult;
                }
                IdentityUser newUser = new()
                {
                    Email = request.Email,
                    UserName = request.Email

                };
                var result = await _userManager.CreateAsync(newUser, request.Password);
                if (result.Succeeded)
                {
                    Employee employee = new()
                    {
                        UserId             = newUser.Id,
                        EmployeeCode       = request.EmployeeCode,
                        FirstName          = request.FirstName,
                        LastName           = request.LastName,
                        PhoneNumber        = request.PhoneNumber,
                        DepartmentId       = request.DepartmentId,
                        DesignationId      = request.DesignationId,
                        WorkplaceId        = request.WorkplaceId,
                        ReportingEmployeeId = request.ReportingManagerId,
                        ReportingToEmployeeId = request.ReportingToEmployeeId,
                        IsActive           = true,
                        CreatedDate        = DateTime.UtcNow
                    };
                    UserResponse userResponse = new UserResponse()
                    {
                        UserName = request.Email,
                        Email = request.Email,
                        UserId = newUser.Id,
                        PhoneNumber = request.PhoneNumber,
                        Token = "1",
                    };

                    await _userManager.AddToRoleAsync(newUser, Role);
                    _context.EmployeeSet.Add(employee);
                    await _context.SaveChangesAsync();

                    // Auto-assign leave balances from each leave type's default days
                    var leaveTypes = await _context.LeaveTypeSet
                        .Where(lt => lt.IsActive)
                        .ToListAsync();

                    foreach (var lt in leaveTypes)
                    {
                        _context.LeaveBalanceSet.Add(new LeaveBalance
                        {
                            EmployeeId    = employee.EmployeeId,
                            LeaveTypeId   = lt.LeaveTypeId,
                            AvailableDays = lt.DefaultDays
                        });
                    }

                    if (leaveTypes.Count > 0)
                        await _context.SaveChangesAsync();

                    userResult.Response = userResponse;
                    return userResult;
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        userResult.Error.Add(new Errors { ErrorCode = 200, ErrorMessage = err.Description });
                    }
                    return userResult;
                }
                                
            }
            catch(Exception e) 
            {
                userResult.Error.Add(new Errors { ErrorCode = 500, ErrorMessage = e.Message });
                return userResult;
            }

        }

        private async Task<Employee?> GetCurrentEmployee()
        {
            var userId = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return await _context.EmployeeSet.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        
        public async Task<Result<List<EmployeeResponse>>> GetVisibleEmployees()
        {
            Result<List<EmployeeResponse>> result = new();

            try
            {
                var userId = _httpContextAccessor.HttpContext?.User
                    .FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 401,
                        ErrorMessage = "User not authenticated"
                    });

                    return result;
                }

                var currentEmployee = await _context.EmployeeSet
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (currentEmployee == null)
                {
                    result.Error.Add(new Errors
                    {
                        ErrorCode = 404,
                        ErrorMessage = "Employee not found."
                    });

                    return result;
                }

                var user = _httpContextAccessor.HttpContext.User;

                IQueryable<Employee> employeeQuery =
                    _context.EmployeeSet.Where(x => x.IsActive);

                if (user.IsInRole("SystemAdministrator"))
                {
                    // sees everyone
                }
                else if (user.IsInRole("HRAdministrator"))
                {
                    employeeQuery = employeeQuery.Where(x =>
                        x.ReportingEmployeeId == currentEmployee.EmployeeId
                        ||
                        _context.EmployeeSet.Any(m =>
                            m.EmployeeId == x.ReportingEmployeeId &&
                            m.ReportingEmployeeId == currentEmployee.EmployeeId));
                }
                else if (user.IsInRole("Manager"))
                {
                    employeeQuery = employeeQuery.Where(x =>
                        x.ReportingEmployeeId == currentEmployee.EmployeeId
                        ||
                        _context.EmployeeSet.Any(s =>
                            s.EmployeeId == x.ReportingEmployeeId &&
                            s.ReportingEmployeeId == currentEmployee.EmployeeId));
                }
                else if (user.IsInRole("Supervisor"))
                {
                    employeeQuery = employeeQuery.Where(x =>
                        x.ReportingEmployeeId == currentEmployee.EmployeeId);
                }
                else
                {
                    employeeQuery = employeeQuery.Where(x =>
                        x.EmployeeId == currentEmployee.EmployeeId);
                }

                var employees = await (
                    from e in employeeQuery

                    join d in _context.DepartmentSet
                        on e.DepartmentId equals d.DepartmentId

                    join des in _context.DesignationSet
                        on e.DesignationId equals des.DesignationId

                    join w in _context.WorkplaceSet
                        on e.WorkplaceId equals w.WorkplaceId

                    join u in _context.Users
                        on e.UserId equals u.Id

                    join rm in _context.EmployeeSet
                        on e.ReportingEmployeeId equals rm.EmployeeId
                        into managerGroup

                    from manager in managerGroup.DefaultIfEmpty()

                    select new EmployeeResponse
                    {
                        EmployeeId = e.EmployeeId,
                        UserId = e.UserId,
                        EmployeeCode = e.EmployeeCode,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        FullName = e.FirstName + " " + e.LastName,
                        Email = u.Email,
                        PhoneNumber = e.PhoneNumber,

                        DepartmentId = d.DepartmentId,
                        DepartmentName = d.DepartmentName,

                        DesignationId = des.DesignationId,
                        DesignationName = des.DesignationName,

                        WorkplaceId = w.WorkplaceId,
                        WorkplaceName = w.Name,

                        RoleName = (
                            from ur in _context.UserRoles
                            join r in _context.Roles
                                on ur.RoleId equals r.Id
                            where ur.UserId == u.Id
                            select r.Name
                        ).FirstOrDefault(),

                        ReportingManagerId = e.ReportingEmployeeId,

                        ReportingManagerName =
                            manager != null
                            ? manager.FirstName + " " + manager.LastName
                            : "",

                        IsActive = e.IsActive,
                        CreatedDate = e.CreatedDate
                    }

                ).ToListAsync();

                result.Response = employees;

                return result;
            }
            catch (Exception ex)
            {
                result.Error.Add(new Errors
                {
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });

                return result;
            }
        }
    }
}
