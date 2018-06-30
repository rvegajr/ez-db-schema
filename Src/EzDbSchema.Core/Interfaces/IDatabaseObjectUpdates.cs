﻿using System;

namespace EzDbSchema.Core.Interfaces
{
    public interface IDatabaseObjectUpdates : IEzObject, IXmlRenderable
    {
        DateTime? LastCreated { get; set; }
        DateTime? LastModified { get; set; }
        string LastItemCreated { get; set; }
        string LastItemModified { get; set; }
    }
}