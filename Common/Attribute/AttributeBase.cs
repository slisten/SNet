using System;

namespace Common
{
    public class AttributeBase: Attribute
    {
        public Type AttributeType { get; }

        public AttributeBase()
        {
            this.AttributeType = this.GetType();
        }
    }
}