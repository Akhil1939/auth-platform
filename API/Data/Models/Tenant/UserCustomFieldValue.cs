﻿namespace API.Data.Models.Tenant;

public class UserCustomFieldValue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string FieldName { get; set; }
    public string FieldValue { get; set; }
}