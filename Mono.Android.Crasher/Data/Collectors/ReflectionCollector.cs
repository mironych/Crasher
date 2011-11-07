using System;
using System.Reflection;
using System.Text;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class ReflectionCollector
    {
        public static String CollectStaticProperties(Type someClass)
        {
            var result = new StringBuilder();
            foreach (var property in someClass.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                result.Append(property.Name).Append('=');
                try
                {
                    result.Append(property.GetValue(null, null)).AppendLine();
                }
                catch (Exception)
                {
                    result.Append("N/A");
                }
            }
            return result.ToString();
        }
    }
}