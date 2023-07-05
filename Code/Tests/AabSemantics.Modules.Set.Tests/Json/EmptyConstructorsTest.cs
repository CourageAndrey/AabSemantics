using NUnit.Framework;

using AabSemantics.Modules.Set.Json;
using AabSemantics.Serialization.Json;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Set.Tests.Json
{
	[TestFixture]
	public class EmptyConstructorsTest
	{
		[Test]
		public void GivenStatements_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Statement[]
			{
				new GroupStatement(),
				new HasPartStatement(),
				new HasSignStatement(),
				new SignValueStatement(),
			}.TestParameterlessConstructors();
		}

		[Test]
		public void GivenQuestions_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Question[]
			{
				new DescribeSubjectAreaQuestion(),
				new EnumerateContainersQuestion(),
				new EnumeratePartsQuestion(),
				new EnumerateSignsQuestion(),
				new FindSubjectAreaQuestion(),
				new GetCommonQuestion(),
				new GetDifferencesQuestion(),
				new HasSignQuestion(),
				new HasSignsQuestion(),
				new IsPartOfQuestion(),
				new IsSignQuestion(),
				new IsSubjectAreaQuestion(),
				new IsValueQuestion(),
				new SignValueQuestion(),
				new WhatQuestion(),
			}.TestParameterlessConstructors();
		}
	}
}
