using Integration.Cache;
using Integration.Interfaces;
using Integration.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration
{
    class Initializer
    {
        public static ConcurrentDictionary<Type, ClassMetaData> Initialize()
        {
            RunStarters();
            return InitializeMetaData();
        }

        private static void RunStarters()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var starterTypes = assemblies.Select((Assembly assembly) =>
            {
                var types = assembly.GetTypes()
                        .Where((Type type) => typeof(IStarter).IsAssignableFrom(type) && !type.IsInterface)
                        .Select((Type type) => type);
                return types;
            }).SelectMany(type => type);

            foreach (Type starter in starterTypes)
            {
                object instance = Activator.CreateInstance(starter);
                MethodInfo startMethod = typeof(IStarter).GetMethod("Start");
                startMethod.Invoke(instance, null);
            }
        }

        private static ConcurrentDictionary<Type, ClassMetaData> InitializeMetaData()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      
            var metaData = assemblies.Select((Assembly assembly) =>
            {
                var keyValuePairs = assembly.GetTypes()
                .Where((Type type) =>
                {
                    bool typePropertiesHaveAttributes = type.GetProperties(ReflectionUtils.GetPublicBindingFlags())
                               .Any((PropertyInfo property) => property.GetCustomAttributes(typeof(IAttributeMarker), true).Length > 0);

                    bool classHasAttributes = ReflectionUtils.HasCustomAttributeType<IAttributeMarker>(type);
                    return typePropertiesHaveAttributes || classHasAttributes;
                })
                .Select((Type type) => new KeyValuePair<Type, ClassMetaData>(type, new ClassMetaData(type)));
                return keyValuePairs;
            });

            return new ConcurrentDictionary<Type, ClassMetaData>(metaData.SelectMany(i => i));
        }
    }
}
