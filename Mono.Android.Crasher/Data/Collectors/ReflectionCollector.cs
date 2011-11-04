using System;
using System.Reflection;
using System.Text;

namespace Mono.Android.Crasher.Data.Collectors
{
    static class ReflectionCollector
    {
        public static string CollectFields(Type someClass)
        {
            var result = new StringBuilder();
            foreach (var field in someClass.GetFields(BindingFlags.Static))
            {
                result.Append(field.Name).Append("=");
                try
                {
                    result.Append(field.GetValue(null));
                }
                catch (Exception)
                {
                    result.Append("N/A");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public static String CollectStaticProperties(Type someClass)
        {
            var result = new StringBuilder();
            foreach (var property in someClass.GetProperties(BindingFlags.Static))
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