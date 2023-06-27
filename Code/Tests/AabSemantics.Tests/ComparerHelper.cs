using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

namespace AabSemantics.Tests
{
	public static class ComparerHelper
	{
		public static void AssertPropertiesAreEqual(this object a, object b)
		{
			Assert.NotNull(a);
			Assert.NotNull(b);

			var type = a.GetType();
			Assert.AreSame(type, b.GetType());

			foreach (var property in type.GetElementaryProperties())
			{
				Assert.AreEqual(property.GetValue(a), property.GetValue(b));
			}
		}

		private static IEnumerable<PropertyInfo> GetElementaryProperties(this Type type)
		{
			return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty).Where(p => p.PropertyType.IsElementary());
		}

		private static bool IsElementary(this Type type)
		{
			return type == typeof(string) || typeof(ValueType).IsAssignableFrom(type);
		}
	}
}
