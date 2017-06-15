using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace UtilityComponent.AutoMapper
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AutoMapperConfigurationAttribute : Attribute
    {

        public AutoMapperConfigurationAttribute(Type profileType)
            : base()
        {
            Mapper.Configuration.AddProfileThreadSafe(Activator.CreateInstance(profileType) as Profile);
        }

        public AutoMapperConfigurationAttribute(Type profileType, Type profileType2)
            : this(profileType)
        {
            Mapper.Configuration.AddProfileThreadSafe(Activator.CreateInstance(profileType2) as Profile);
        }

        public AutoMapperConfigurationAttribute(Type profileType, Type profileType2, Type profileType3)
            : this(profileType, profileType2)
        {
            Mapper.Configuration.AddProfileThreadSafe(Activator.CreateInstance(profileType3) as Profile);
        }

        public AutoMapperConfigurationAttribute(Type profileType, Type profileType2, Type profileType3, Type profileType4)
            : this(profileType, profileType2, profileType3)
        {
            Mapper.Configuration.AddProfileThreadSafe(Activator.CreateInstance(profileType4) as Profile);
        }
    }
}
