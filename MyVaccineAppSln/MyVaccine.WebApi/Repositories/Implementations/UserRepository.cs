using Microsoft.AspNetCore.Identity;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using System.Transactions;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly MyVaccineAppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public UserRepository(MyVaccineAppDbContext context, UserManager<IdentityUser> userManager) : base(context)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddUser(RegisterRequestDTO request)
    {
        var response = new IdentityResult();
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = new ApplicationUser
            {
                UserName = request.UserName.ToLower(),
                Email = request.UserName
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            response = result;

            if (!result.Succeeded)
            {
                return response;
            }

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                AspNetUserId = user.Id
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            scope.Complete();
        }

        return response;
    }
}