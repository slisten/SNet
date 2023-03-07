using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common
{
    public interface IAttribute: IService
    {
        void Add(Assembly assembly);
        List<Type> GetTypes<T>() where T : AttributeBase;
        List<Type> GetTypes(Type attributeType);
    }
}