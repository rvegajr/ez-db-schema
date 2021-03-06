﻿using EzDbSchema.Core.Enums;
using System;
namespace EzDbSchema.Core.Interfaces
{
    public interface IRelationship : IEzObject, IXmlRenderable
    {
        string FromColumnName { get; set; }
        string FromFieldName { get; set; }
        string FromTableName { get; set; }
        string Name { get; set; }
        IEntity Parent { get; set; }
        string PrimaryTableName { get; set; }
        string ToColumnName { get; set; }
        string ToFieldName { get; set; }
        string ToTableName { get; set; }
        string Type { get; set; }
        RelationshipMultiplicityType MultiplicityType { get; set; }
    }

}
