using System;
using System.Linq;
using System.Linq.Expressions;
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

        public static LambdaExpression StripConvert<T>(Expression<Func<T, object>> source) {
            var result = source.Body;
            while (((result.NodeType == ExpressionType.Convert)
                   || (result.NodeType == ExpressionType.ConvertChecked))
                   && (result.Type == typeof(object))) {
                result = ((UnaryExpression)result).Operand;
            }
            return Expression.Lambda(result, source.Parameters);
        }
    }
}