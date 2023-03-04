using HCM.Api.Data.Contracts;

namespace HCM.Api.Data.Models;

public class Job : BaseModel<int>
{
    public string JobTitle { get; set; }
    public string JobDescription { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }

}