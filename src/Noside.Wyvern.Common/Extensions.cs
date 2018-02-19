using System;
using System.ComponentModel;
using System.Reflection;

namespace Noside.Wyvern.Common
{
    public static class Extensions
    {
        public static T[] Init<T>(this T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }

	    public static T GetEnum<T>(this string description)
	    {
		    Type type = typeof(T);
		    if (!type.IsEnum) throw new InvalidOperationException();
		    foreach (FieldInfo field in type.GetFields())
		    {
			    if (Attribute.GetCustomAttribute(field,
				    typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
			    {
				    if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
					    return (T)field.GetValue(null);
			    }
			    else
			    {
				    if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
					    return (T)field.GetValue(null);
			    }
		    }
		    return default(T);
	    }

//	    public static Color GetColor(this RGBColor rgb) {
//			return Color.FromRgb((byte)(rgb.R * 0xFF), (byte)(rgb.G * 0xFF), (byte)(rgb.B * 0xFF));
//		}
	}
}
