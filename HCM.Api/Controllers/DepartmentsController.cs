﻿using AutoMapper;
using HCM.Api.Data.Models;
using HCM.Shared.Data.Contracts;
using HCM.Shared.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCM.Api.Controllers;

public class DepartmentsController : BaseController
{
    private readonly IRepository<Department> _departmentRepository;
    private readonly IMapper _mapper;
    private const string DepartmentNotFountMessage = "Department with id: {0} not found";
    private const string DepartmentNotEmptyMessage = "There are employees assigned to department '{0}' ";

    public DepartmentsController(IRepository<Department> departmentRepository, IMapper mapper)
    {
        _departmentRepository = departmentRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        var departments = await _mapper.ProjectTo<DepartmentDto>(_departmentRepository.AllAsNoTracking())
            .ToArrayAsync();

        return Ok(departments);
    }


    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetJobById(int id)
    {
        var departmentQuery = _departmentRepository.AllAsNoTracking().Where(j => j.Id == id);

        var departments = await _mapper.ProjectTo<DepartmentDto>(departmentQuery).ToArrayAsync();

        var department = departments.FirstOrDefault();

        if (department == null) return NoContent();

        return Ok(department);
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto department)
    {
        var newDepartment = _mapper.Map<Department>(department);

        await _departmentRepository.AddAsync(newDepartment);
        await _departmentRepository.SaveChangesAsync();

        return Ok(_mapper.Map<DepartmentDto>(newDepartment));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto department)
    {
        var departmentToUpdate = await _departmentRepository.All().FirstOrDefaultAsync(d => d.Id == department.Id);

        if (departmentToUpdate == null)
            return NotFound(string.Format(DepartmentNotFountMessage, department.Id));

        _mapper.Map(department, departmentToUpdate);

        _departmentRepository.Update(departmentToUpdate);

        await _departmentRepository.SaveChangesAsync();

        return Ok(_mapper.Map<DepartmentDto>(departmentToUpdate));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var departments = await _mapper.ProjectTo<Department>(_departmentRepository.All())
            .ToArrayAsync();

        var departmentToDelete = departments.FirstOrDefault(d => d.Id == id);

        if (departmentToDelete == null)
            return NotFound(string.Format(DepartmentNotFountMessage, id));

        if (departmentToDelete.Employees != null && departmentToDelete.Employees.Any())
            return UnprocessableEntity(string.Format(DepartmentNotEmptyMessage, departmentToDelete.Name));

        _departmentRepository.Delete(departmentToDelete);
        await _departmentRepository.SaveChangesAsync();

        return Ok(_mapper.Map<DepartmentDto>(departmentToDelete));
    }

}