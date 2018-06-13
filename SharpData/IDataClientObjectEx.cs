using SharpData.Filters;
using SharpData.Util;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SharpData {
    public static class IDataClientObjectEx {
        public static T LoadById<T>(this IDataClient client, 
                                    string table, 
                                    object id, 
                                    Expression<Func<T, object>> idProperty) where T : new() {
            var unboxed = ReflectionHelper.StripConvert(idProperty);
            var propertyInfo = ((MemberExpression)unboxed.Body).Member as PropertyInfo;
            var columns = ReflectionHelper.GetPropertiesNames(typeof(T));
            return client.Select
                         .Columns(columns)
                         .From(table)
                         .Where(Filter.Eq(propertyInfo.Name, id))
                         .AllRows()
                         .Map<T>()
                         .FirstOrDefault();
        }
    }
}