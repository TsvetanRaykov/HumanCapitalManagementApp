namespace HCM.Shared.Data.DTO;
using Contracts;

public class JobDto : BaseModel<int>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal MinSalary { get; set; }
    public decimal MaxSalary { get; set; }
    public ICollection<EmployeeDto>? Employees { get; set; }
}
