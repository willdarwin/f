using System;
using System.ComponentModel;
using System.Reflection;

namespace MVCWebUIComponent.Models
{
    public class LocalizedDisplayName : DisplayNameAttribute
    {
        private string _defaultName = "";

        public Type ResourceType { get; set; }

        public string ResourceName { get; set; }

        public LocalizedDisplayName (string defaultName) { _defaultName = defaultName; }

        public override string DisplayName
        {
            get
            {
                System.Reflection.PropertyInfo p = ResourceType.GetProperty(ResourceName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);


                if (p != null)
                {
                    return p.GetValue(null, null).ToString();
                }
                else
                {
                    return _defaultName;
                }

            }
        }
    }
}