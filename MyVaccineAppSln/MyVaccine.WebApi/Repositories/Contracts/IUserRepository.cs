using Microsoft.AspNetCore.Identity;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Repositories.Contracts;

public interface IUserRepository : IBaseRepository<User>
{
    Task<IdentityResult> AddUser(RegisterRequestDTO request);
}