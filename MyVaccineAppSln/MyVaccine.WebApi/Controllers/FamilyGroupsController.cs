using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyVaccine.WebApi.DTOs.Request;
using MyVaccine.WebApi.Services.Contracts;

namespace MyVaccine.WebApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FamilyGroupsController : ControllerBase
{
    private readonly IFamilyGroupService _familyGroupService;
    private readonly IValidator<FamilyGroupRequestDTO> _validator;

    public FamilyGroupsController(IFamilyGroupService familyGroupService, IValidator<FamilyGroupRequestDTO> validator)
    {
        _familyGroupService = familyGroupService;
        _validator = validator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var familyGroups = await _familyGroupService.GetAll();
        return Ok(familyGroups);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var familyGroup = await _familyGroupService.GetById(id);
            return Ok(familyGroup);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });        
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(FamilyGroupRequestDTO familyGroupDto)
    {
        var validationResult = await _validator.ValidateAsync(familyGroupDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var familyGroup = await _familyGroupService.Add(familyGroupDto);

        return Ok(familyGroup);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, FamilyGroupRequestDTO familyGroupDto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(familyGroupDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var familyGroup = await _familyGroupService.Update(id, familyGroupDto);
            return Ok(familyGroup);
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
            var familyGroup = await _familyGroupService.Delete(id);
            return Ok(familyGroup);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
