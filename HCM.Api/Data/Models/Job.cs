using HCM.Shared.Data.Contracts;

namespace HCM.Api.Data.Models;

public class Job : BaseModel<int>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }

}