using System;

namespace Light.EventBus.Events
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class BindingNameAttribute(string bindingName) : Attribute
    {
        public virtual string BindingName => BindingNameValue;

        protected string BindingNameValue = bindingName;
    }
}
