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
			Assert.That(a, Is.Not.Null);
			Assert.That(b, Is.Not.Null);

			var type = a.GetType();
			Assert.That(b.GetType(), Is.SameAs(type));

			foreach (var property in type.GetElementaryProperties())
			{
				Assert.That(property.GetValue(b), Is.EqualTo(property.GetValue(a)));
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
