namespace HCM.Shared.Data.DTO;

using Contracts;

public class EmployeeDto : BaseModel<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public decimal Salary { get; set; }

    public int JobId { get; set; }
   
    public int DepartmentId { get; set; }

    public JobDto? Job { get; set; }

    public DepartmentDto? Department { get; set; }
}