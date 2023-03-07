using System;
using System.Collections.Generic;
using System.Reflection;

namespace Common
{
    public class AttributeService : IAttribute
    {
        private Dictionary<Type, List<Type>> typeDic = new Dictionary<Type, List<Type>>();
        public void Add(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                object[] objects = type.GetCustomAttributes(typeof(AttributeBase), false);
                if (objects.Length == 0)
                {
                    continue;
                }

                AttributeBase baseAttribute = (AttributeBase) objects[0];
                if (!typeDic.ContainsKey(baseAttribute.AttributeType))
                {
                    typeDic.Add(baseAttribute.AttributeType,new List<Type>());
                }
                typeDic[baseAttribute.AttributeType].Add(type);
            }
        }
        
        public List<Type> GetTypes<T>() where T: AttributeBase
        {
            Type attributeType = typeof(T);
            return GetTypes(attributeType);
        }
        
        public List<Type> GetTypes(Type attributeType)
        {
            if (!this.typeDic.ContainsKey(attributeType))
            {
                return new List<Type>();
            }
            return this.typeDic[attributeType];
        }

        public void OnInit()
        {
            
        }

        public void OnDestroy()
        {
            
        }
    }
}