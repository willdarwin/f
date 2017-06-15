using System;
using System.ComponentModel;
using System.Reflection;

namespace FPXAppDesign.DesignerClass.Component
{
    public class LocalizedDisplayNameDesigner : DisplayNameAttribute
    {
        private string _defaultName = "";

        public Type ResourceType { get; set; }

        public string ResourceName { get; set; }

        public LocalizedDisplayNameDesigner()
        {
        }

        public LocalizedDisplayNameDesigner (string defaultName) { _defaultName = defaultName; }

        public override string DisplayName
        {
            get
            {
                var p = ResourceType.GetProperty(ResourceName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);


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