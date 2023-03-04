using System.ComponentModel.DataAnnotations;

namespace HCM.Api.Data.Contracts;

public abstract class BaseModel<TKey> : IAuditInfo
{
    [Key]
    TKey Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}