using AutoMapper;
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
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        var department = await _departmentRepository.All().FirstOrDefaultAsync(d => d.Id == id);
        if (department == null)
            return NotFound(string.Format(DepartmentNotFountMessage, id));

        return Ok(_mapper.Map<DepartmentDto>(department));
    }

    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] DepartmentDto department)
    {
        var newDepartment = _mapper.Map<Department>(department);

        await _departmentRepository.AddAsync(newDepartment);
        await _departmentRepository.SaveChangesAsync();

        return Ok(newDepartment.Id);
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
        var departmentToDelete = await _departmentRepository.All().FirstOrDefaultAsync(d => d.Id == id);

        if (departmentToDelete == null)
            return NotFound(string.Format(DepartmentNotFountMessage, id));

        _departmentRepository.Delete(departmentToDelete);
        await _departmentRepository.SaveChangesAsync();

        return Ok(_mapper.Map<DepartmentDto>(departmentToDelete));
    }

}