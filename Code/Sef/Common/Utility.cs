using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using Sef.Localization;

namespace Sef.Common
{
	public static class Utility
	{
		#region Ranges

		public static T EnsureRange<T>(this T value, T from, T to)
		  where T : IComparable<T>
		{
			if (value.CompareTo(from) < 0)
			{
				return from;
			}
			else if (value.CompareTo(to) > 0)
			{
				return to;
			}
			else
			{
				return value;
			}
		}

		public static Boolean IsInRange<T>(this T value, T from, T to)
		  where T : IComparable<T>
		{
			return (value.CompareTo(from) >= 0) && (value.CompareTo(to) <= 0);
		}

		#endregion

		#region Enumerations

		public static List<EnumT> GetValues<EnumT>()
		  where EnumT : struct
		{
            if (!typeof(EnumT).IsEnum) throw new ArgumentException(String.Format(Language.Current.Errors.TypeIsntEnumerable, typeof(EnumT).FullName));
            return Enum.GetValues(typeof(EnumT)).OfType<EnumT>().ToList();
		}

		public static Boolean IsFlagOn<EnumT>(this EnumT value, EnumT flag)
		  where EnumT : struct
		{
            if (!typeof(EnumT).IsEnum) throw new ArgumentException(String.Format(Language.Current.Errors.TypeIsntEnumerable, typeof(EnumT).FullName));
            UInt64 value64 = Convert.ToUInt64(value);
			UInt64 flag64 = Convert.ToUInt64(flag);
			return (value64 & flag64) == flag64;
		}

		public static EnumT SetFlagState<EnumT>(EnumT value, EnumT flag, Boolean on)
		  where EnumT : struct
		{
            if (!typeof(EnumT).IsEnum) throw new ArgumentException(String.Format(Language.Current.Errors.TypeIsntEnumerable, typeof(EnumT).FullName));
            UInt64 value64 = Convert.ToUInt64(value);
			UInt64 flag64 = Convert.ToUInt64(flag);
			value64 = on ? (value64 | flag64) : (value64 & ~flag64);
			return (EnumT) Enum.ToObject(typeof(EnumT), value64);
		}

		#endregion

		#region Color names

		public static String GetName(this Color color)
		{
			return TypeDescriptor.GetConverter(color).ConvertToString(color);
		}

		public static Color GetColorByName(this String colorName)
		{
			var color = TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(colorName);
			return (color != null) ? (Color) color : Color.Transparent;
		}

		#endregion

		#region Random

		public static Random Randomize()
		{
			return new Random(DateTime.Now.Millisecond);
		}

		public static T GetRandom<T>(this IList<T> list, Random random = null)
		{
			return list[(random ?? Randomize()).Next(list.Count)];
		}

		#endregion

		#region 'Enumerations' lists

		public static List<T> ReadFields<T>(Type enumerationType = null)
		  where T : class
		{
			if (enumerationType == null)
			{
				enumerationType = typeof(T);
			}
			return enumerationType.GetFields().Where(f => (f.FieldType == typeof(T))).Select(f => f.GetValue(null) as T).ToList();
		}

		public static List<T> ReadProperties<T>(Type enumerationType = null)
		  where T : class
		{
			if (enumerationType == null)
			{
				enumerationType = typeof(T);
			}
			return enumerationType.GetProperties().Where(p => (p.PropertyType == typeof(T))).Select(p => p.GetValue(null) as T).ToList();
		}

		#endregion

		#region Garbage collection

		public static void CollectGarbage()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}

		#endregion

		#region x64 Check

		public static Boolean Is64BitProcess
		{
			get { return IntPtr.Size == 8; }
		}
		
		#endregion

        #region Safe image loading

	    public static Bitmap LoadBitmap(this String fileName)
	    {
            using (var tempImage = new Bitmap(fileName))
	        {
                return new Bitmap(tempImage);
	        }
	    }

        #endregion

        #region Reflection

	    public static Object GetPropertyValue(this Object instance, String fullPath)
        {
            foreach (var propertyName in fullPath.Split('.'))
            {
                if (instance == null)
                {
                    break;
                }
                var type = instance.GetType();
                var property = type.GetProperty(propertyName);
                if (property == null)
                {
                    instance = null;
                    break;
                }
                instance = property.GetValue(instance);
            }
	        return instance;
        }

        #endregion
    }
}
