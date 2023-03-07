using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Common
{
    public class Env
    {
        #region Static

        public static Env CurEnv;
        public static Env NewEnv()
        {
            CurEnv=new Env();
            return CurEnv;
        }

        #endregion
        
        public Dictionary<Type, IService> type2ServiceDic;
        private Stack<IService> serviceStack;
        public void Inject(object obj)
        {
            if (obj == null)
            {
                return;
            }
            MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            for (int i = members.Length - 1; i >= 0; i--)
            {
                MemberInfo member = members[i];
                if (member.IsDefined(typeof(InjectAttribute), true))
                {
                    InjectAttribute attr = member.GetCustomAttributes(typeof(InjectAttribute), true)[0] as InjectAttribute;
                    if (member is PropertyInfo)
                    {
                        PropertyInfo propertyInfo = member as PropertyInfo;
                        propertyInfo.SetValue(obj, this.GetService(propertyInfo.PropertyType), null);
                    }
                    else if (member is FieldInfo)
                    {
                        FieldInfo fieldInfo = member as FieldInfo;
                        fieldInfo.SetValue(obj, this.GetService(fieldInfo.FieldType));
                    }
                }
            }
        }

        private Env()
        {
            type2ServiceDic=new Dictionary<Type, IService>();
            serviceStack = new Stack<IService>();
        }

        public void OnUpdate()
        {
            
        }

        public void OnDestroy()
        {
            while (serviceStack.Count>0)
            {
                IService service = serviceStack.Pop();
                service.OnDestroy();
            }
            type2ServiceDic.Clear();
        }

        public void BindService<T>(T service) where T: IService
        {
            if (service == null)
            {
                return;
            }
            Type baseType = typeof(T);
            if (!type2ServiceDic.ContainsKey(baseType))
            {
                type2ServiceDic.Add(baseType,service);
                serviceStack.Push(service);
                
                //注入
                Inject(service);
                service.OnInit();
            }
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public IService GetService(Type type)
        {
            if (type2ServiceDic.TryGetValue(type, out IService value))
            {
                return value;
            }

            return null;
        }
    }
}