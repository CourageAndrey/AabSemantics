using NUnit.Framework;

using AabSemantics.Modules.Mathematics.Json;
using AabSemantics.Serialization.Json;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Mathematics.Tests.Serialization.Json
{
	[TestFixture]
	public class EmptyConstructorsTest
	{
		[Test]
		public void GivenStatements_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Statement[]
			{
				new ComparisonStatement(),
			}.TestParameterlessConstructors();
		}

		[Test]
		public void GivenQuestions_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Question[]
			{
				new ComparisonQuestion(),
			}.TestParameterlessConstructors();
		}
	}
}
