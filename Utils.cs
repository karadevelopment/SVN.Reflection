using System.Collections;
using System.Collections.Generic;

namespace SVN.Reflection
{
    public static class Utils
    {
        public class PropertyDiff
        {
            public string key { get; set; }
            public string value1 { get; set; }
            public string value2 { get; set; }
        }

        public static IEnumerable<PropertyDiff> GetDifferences<T>(T obj1, T obj2)
        {
            foreach (var property in obj1.GetType().GetProperties())
            {
                var propertyName = property.Name;
                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                if (property.PropertyType.IsGenericType)
                {
                    var list1 = (IList)value1;
                    var list2 = (IList)value2;

                    if (list1.Count < list2.Count)
                    {
                        yield return new PropertyDiff
                        {
                            key = propertyName,
                            value1 = string.Empty,
                            value2 = $"{list2.Count - list1.Count} Einträge hinzugefügt",
                        };
                    }
                    else if (list2.Count < list1.Count)
                    {
                        yield return new PropertyDiff
                        {
                            key = propertyName,
                            value1 = string.Empty,
                            value2 = $"{list1.Count - list2.Count} Einträge entfernt",
                        };
                    }
                    else
                    {
                        for (var i = 1; i <= list1.Count; i++)
                        {
                            value1 = list1[i - 1];
                            value2 = list2[i - 1];

                            foreach (var result in Utils.GetDifferences(value1, value2))
                            {
                                yield return result;
                            }
                        }
                    }
                }
                else if (value1.ToString() != value2.ToString())
                {
                    yield return new PropertyDiff
                    {
                        key = propertyName,
                        value1 = value1.ToString(),
                        value2 = value2.ToString(),
                    };
                }
            }
        }
    }
}