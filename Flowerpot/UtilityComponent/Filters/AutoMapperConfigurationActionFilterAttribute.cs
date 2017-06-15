using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using UtilityComponent.AutoMapper;

namespace UtilityComponent.Filters
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AutoMapperConfigurationActionFilterAttribute : FilterAttribute
    {
        public AutoMapperConfigurationActionFilterAttribute(Type profileType)
        {
            Mapper.Configuration.AddProfileThreadSafe(Activator.CreateInstance(profileType) as Profile);
        }
    }
}
