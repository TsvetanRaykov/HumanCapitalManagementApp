using HCM.Shared.Data.Contracts;

namespace HCM.Shared.Data.DTO;

public class DepartmentDto : BaseModel<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string? Description { get; set; }
    public ICollection<EmployeeDto>? Employees { get; set; }
}