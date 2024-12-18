using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NUnit.Framework;

using AabSemantics.Serialization.Json;

namespace AabSemantics.TestCore
{
	public static class EmptyConstructorsTest
	{
		public static void TestParameterlessConstructors(this ICollection<Statement> statementsToCheck)
		{
			// assert
			foreach (var statement in statementsToCheck)
			{
				var propertiesToCheck = statement.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).ToList();
				foreach (var property in propertiesToCheck)
				{
					var value = property.GetValue(statement, null);
					Assert.That(value, Is.Null);
				}
			}
		}

		public static void TestParameterlessConstructors(this ICollection<Question> questionsToCheck)
		{
			// assert
			foreach (var question in questionsToCheck)
			{
				Assert.That(question.Preconditions.Count, Is.EqualTo(0));
				var propertiesToCheck = question.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance).ToList();
				propertiesToCheck.Remove(propertiesToCheck.First(p => p.Name == nameof(Question.Preconditions)));
				foreach (var property in propertiesToCheck)
				{
					var value = property.GetValue(question, null);
					if (property.PropertyType == typeof(bool))
					{
						Assert.That(value, Is.False);
					}
					else
					{
						Assert.That(value, Is.Null);
					}
				}
			}
		}
	}
}
