using AutoMapper;
using HCM.Api.Data.Models;
using HCM.Shared.Data.Contracts;
using HCM.Shared.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HCM.Api.Controllers;

public class EmployeesController : BaseController
{
    private readonly IRepository<Employee> _employeeRepository;
    private readonly IMapper _mapper;
    private const string EmployeeNotFountMessage = "Employee with id: {0} not found";

    public EmployeesController(IRepository<Employee> employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _mapper.ProjectTo<EmployeeDto>(_employeeRepository.AllAsNoTracking())
            .ToArrayAsync();

        return Ok(employees);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employee)
    {
        var newEmployee = _mapper.Map<Employee>(employee);

        await _employeeRepository.AddAsync(newEmployee);
        await _employeeRepository.SaveChangesAsync();

        return Ok(newEmployee.Id);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee)
    {
        var employeeToUpdate = await _employeeRepository.All().FirstOrDefaultAsync(j => j.Id == employee.Id);

        if (employeeToUpdate == null)
            return NotFound(string.Format(EmployeeNotFountMessage, employee.Id));

        _mapper.Map(employee, employeeToUpdate);

        _employeeRepository.Update(employeeToUpdate);

        await _employeeRepository.SaveChangesAsync();

        return Ok(_mapper.Map<EmployeeDto>(employeeToUpdate));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employeeToDelete = await _employeeRepository.All().FirstOrDefaultAsync(j => j.Id == id);

        if (employeeToDelete == null)
            return NotFound(string.Format(EmployeeNotFountMessage, id));

        _employeeRepository.Delete(employeeToDelete);
        await _employeeRepository.SaveChangesAsync();

        return Ok(_mapper.Map<EmployeeDto>(employeeToDelete));
    }

}