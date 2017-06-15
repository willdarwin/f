using System;
using System.Collections.Generic;
using AutoMapper;

namespace UtilityComponent.AutoMapper
{
    public static class AutoMapperExtension
    {
        private static List<Type> _addedProfiles = new List<Type>();

        public static void AddProfileThreadSafe(this IConfiguration instance, Profile profile)
        {
            lock (_addedProfiles)
            {
                if (_addedProfiles.Contains(profile.GetType()))
                    return;
            }

            lock (instance)
            {
                instance.AddProfile(profile);
                instance.Seal();
            }

            lock (_addedProfiles)
            {
                _addedProfiles.Add(profile.GetType());
            }
        }
    }
}
