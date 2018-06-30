﻿using System;
using System.Collections.Generic;
using EzDbSchema.Core.Enums;

namespace EzDbSchema.Core.Interfaces
{
    public interface IRelationshipDictionary : IDictionary<string, IRelationship>, IEzObject, IXmlRenderable
    {
    }

    public interface IRelationshipList : IList<IRelationship>, IEzObject, IXmlRenderable
    {
		IRelationshipList Fetch(RelationshipType TypeToFetch);
		int CountItems(string searchFor);
		int CountItems(RelationSearchField searchField, string searchFor);
    }
    public interface IRelationshipReferenceList : IRelationshipList, IEzObject, IXmlRenderable
    {
    }
}
