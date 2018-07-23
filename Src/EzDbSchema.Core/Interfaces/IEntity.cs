﻿using System;
namespace EzDbSchema.Core.Interfaces
{
    public interface IEntity : IEzObject, IXmlRenderable
    {
        IDatabase Parent { get; set; }
        bool IsTemporalView { get; set; }
        string Name { get; set; }
        string Alias { get; set; }
        string ObjectState { get; set; }
        string Schema { get; set; }
        string TemporalType { get; set; }
        string Type { get; set; }
        bool HasPrimaryKeys();
        bool IsAuditable();
        bool isAuditablePropertyName(string propertyNameToCheck);

        IPropertyDictionary Properties { get; set; }
        IRelationshipReferenceList Relationships { get; set; }
        IPrimaryKeyProperties PrimaryKeys { get; set; }
        ICustomAttributes CustomAttributes { get; set; }
    }
}
