using System;

using NUnit.Framework;

using AabSemantics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.TestCore
{
	public static class CustomStatementHelper
	{
		private static string ToStringEssential(this IText text)
		{
			var s = text.ToString();
			return s.Remove(s.IndexOf("(", StringComparison.InvariantCulture));
		}

		public static void CheckAllDescriptionsAgainst(this IStatement statement, CustomStatement custom, string statementName)
		{
			// act
			string name = statement.Name.GetValue(Language.Default);
			var trueText = statement.DescribeTrue();
			var falseText = statement.DescribeFalse();
			var questionText = statement.DescribeQuestion();
			string customName = custom.Name.GetValue(Language.Default);
			var trueCustomText = custom.DescribeTrue();
			var falseCustomText = custom.DescribeFalse();
			var questionCustomText = custom.DescribeQuestion();

			// assert
			Assert.That(name, Is.EqualTo(statementName));
			Assert.That(customName, Is.EqualTo("Custom Statement"));
			Assert.That(trueText.ToStringEssential(), Is.EqualTo(trueCustomText.ToStringEssential()));
			Assert.That(falseText.ToString(), Is.EqualTo(falseCustomText.ToString()));
			Assert.That(questionText.ToString(), Is.EqualTo(questionCustomText.ToString()));
		}
	}
}
