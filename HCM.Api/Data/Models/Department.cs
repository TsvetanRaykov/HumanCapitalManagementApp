using HCM.Shared.Data.Contracts;

namespace HCM.Api.Data.Models;
public class Department : BaseModel<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public virtual ICollection<Employee>? Employees { get; set; }
}