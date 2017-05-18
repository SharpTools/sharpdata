using System.Reflection;

namespace SharpData.Util {
    public class ReflectionHelper {
        public static BindingFlags NoRestrictions = BindingFlags.Public |
                                                    BindingFlags.NonPublic |
                                                    BindingFlags.Static |
                                                    BindingFlags.Instance;

    }
}