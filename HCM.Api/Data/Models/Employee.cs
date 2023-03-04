using HCM.Shared.Data.Contracts;

namespace HCM.Api.Data.Models;

public class Employee : BaseModel<int>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime HireDate { get; set; }
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public decimal Salary { get; set; }

    public int JobId { get; set; }
    public virtual Job Job { get; set; }

    public int DepartmentId { get; set; }
    public virtual Department Department { get; set; }

}