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
    private readonly IRepository<Department> _departmentRepository;
    private readonly IRepository<Job> _jobRepository;
    private readonly IMapper _mapper;

    private const string EmployeeNotFountMessage = "Employee with id: {0} not found";
    private const string JobNotFountMessage = "Job with id: {0} not found";
    private const string InvalidSalaryMessage = "Salary {0} is inappropriate with the specified job. It should be between {1} and {2}.";
    private const string EmployeeExistsMessage = "Employee whit email {0} already registered";
    private const string DepartmentNotExistsMessage = "Department with id: {0} not found";

    public EmployeesController(
        IRepository<Employee> employeeRepository,
        IRepository<Department> departmentRepository,
        IRepository<Job> jobRepository,
        IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _jobRepository = jobRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployees()
    {
        var employees = await _mapper.ProjectTo<EmployeeDto>(_employeeRepository.AllAsNoTracking()).ToArrayAsync();

        // filter nested collections
        var result = employees.Select(e =>
        {
            if (e.Job != null) e.Job.Employees = null;
            if (e.Department != null) e.Department.Employees = null;
            return e;
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto employee)
    {
        var employeeExists = await _employeeRepository.AllAsNoTracking().FirstOrDefaultAsync(e => e.Email == employee.Email);
        if (employeeExists != null)
            return Conflict(string.Format(EmployeeExistsMessage, employee.Email));

        var department = await _departmentRepository.AllAsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == employee.DepartmentId);
        
        if (department == null)
            return UnprocessableEntity(string.Format(DepartmentNotExistsMessage, employee.DepartmentId));

        var job = await _jobRepository.AllAsNoTracking().FirstOrDefaultAsync(j => j.Id == employee.JobId);
        if (job == null)
            return NotFound(string.Format(JobNotFountMessage, employee.JobId));
        
        var validateSalary = await ValidateSalary(employee);
        if (validateSalary.StatusCode != StatusCodes.Status200OK) return validateSalary;

        var newEmployee = _mapper.Map<Employee>(employee);

        await _employeeRepository.AddAsync(newEmployee);

        await _employeeRepository.SaveChangesAsync();

        return Ok(_mapper.Map<EmployeeDto>(newEmployee));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee)
    {
        var exist = await _employeeRepository.AllAsNoTracking().FirstOrDefaultAsync(e => e.Email == employee.Email);
        if (exist != null && exist.Id != employee.Id)
            return Conflict(string.Format(EmployeeExistsMessage, employee.Email));

        var validateResult = await ValidateSalary(employee);
        if (validateResult.StatusCode != StatusCodes.Status200OK) return validateResult;

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



    private async Task<ObjectResult> ValidateSalary(EmployeeDto employee)
    {
        var job = await _jobRepository.AllAsNoTracking().FirstOrDefaultAsync(j => j.Id == employee.JobId);

        if (job == null)
            return NotFound(string.Format(JobNotFountMessage, employee.JobId));

        if (employee.Salary < job.MinSalary || employee.Salary > job.MaxSalary)
            return UnprocessableEntity(string.Format(InvalidSalaryMessage, employee.Salary, job.MinSalary, job.MaxSalary));

        return Ok(null);
    }

}