using System;
using System.Linq;
using System.Reflection;

namespace SharpData.Util {
    public class ReflectionHelper {
        public static BindingFlags NoRestrictions = BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Static |
                                                    BindingFlags.Instance;

        public static string[] GetPropertiesNames(Type type) => 
            type.GetProperties(NoRestrictions | BindingFlags.IgnoreCase)
                .Select(p => p.Name)
                .ToArray();

        public static object[] GetPropertiesValues(object obj) =>
            obj.GetType()
               .GetProperties(NoRestrictions | BindingFlags.IgnoreCase)
               .Select(p => p.GetValue(obj))
               .ToArray();
    }
}