﻿namespace HCM.Api.Data.Contracts;

public interface IAuditInfo
{
    DateTime CreatedOn { get; set; }
    DateTime? ModifiedOn { get; set; }
}