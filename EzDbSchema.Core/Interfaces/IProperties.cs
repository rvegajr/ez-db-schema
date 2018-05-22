﻿using System;
using System.Collections.Generic;

namespace EzDbSchema.Core.Interfaces
{
    public interface IPropertyDictionary : IDictionary<string, IProperty>, IJson<IPropertyDictionary>
    {
    }

    public interface IPropertyList : IList<IProperty>
    {
    }

    public interface IPrimaryKeyProperties : IPropertyList, IJson<IPrimaryKeyProperties>
    {
    }
}
