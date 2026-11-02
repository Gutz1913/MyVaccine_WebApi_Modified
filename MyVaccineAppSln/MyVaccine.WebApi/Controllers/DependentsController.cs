using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class DependentsController : ControllerBase
{
    private readonly IDependentService _dependentService;
    private readonly IValidator<DependentRequestDTO> _validator;

    public DependentsController(
        IDependentService dependentService,
        IValidator<DependentRequestDTO> validator)
    {
        _dependentService = dependentService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var dependents = await _dependentService.GetAll();

        return Ok(dependents);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var dependent = await _dependentService.GetById(id);
            return Ok(dependent);
        }
        catch (Exception ex)
        {

            return NotFound(new { message = ex.Message });
        }
    }

    [HttpGet("get-dependents-by-userid/{userId}")]
    public async Task<IActionResult> GetDependentsByUserId(int userId)
    {
        try
        {
            var dependents = await _dependentService.GetDependentsByUserId(userId);
            return Ok(dependents);
        }
        catch (Exception ex)
        {

            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(DependentRequestDTO dependentDto)
    {
        var validationResult = await _validator.ValidateAsync(dependentDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }
        var dependent = await _dependentService.Add(dependentDto);

        return Ok(dependent);
    }

    [HttpPost("register-allergies")]
    public async Task<IActionResult> RegisterAllergy(DependentAllergyRequestDTO request)
    {
        try
        {
            await _dependentService.RegisterAllergyAsync(request);
            return Ok(new { message = "Allergia registrada exitosamente" });
        }
        catch (Exception ex)
        {

            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, DependentRequestDTO dependentDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(dependentDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var dependent = await _dependentService.Update(id, dependentDto);
            return Ok(dependent);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var dependent = await _dependentService.Delete(id);
            return Ok(dependent);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
