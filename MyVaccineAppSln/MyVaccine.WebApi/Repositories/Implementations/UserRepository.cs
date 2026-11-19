using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = request.UserName.ToLower(),
                    Email = request.UserName
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return result;  // ✅ Retornar resultado directo
                }

                var newUser = new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = request.BirthDate,
                    AspNetUserId = user.Id,
                    CreatedAt = DateTime.UtcNow  // ✅ Auditoría
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                scope.Complete();

                return result;
            }
            catch (DbUpdateException ex)
            {
                // ✅ Error de BD
                return IdentityResult.Failed(
                    new IdentityError { Code = "DatabaseError", Description = "Error en base de datos" }
                );
            }
            catch (Exception ex)
            {
                // ✅ Error inesperado
                return IdentityResult.Failed(
                    new IdentityError { Code = "UnexpectedError", Description = "Error inesperado" }
                );
            }
        }
    }
}