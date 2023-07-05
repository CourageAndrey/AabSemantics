using NUnit.Framework;

using AabSemantics.Modules.Processes.Json;
using AabSemantics.Serialization.Json;
using AabSemantics.TestCore;

namespace AabSemantics.Modules.Processes.Tests.Json
{
	[TestFixture]
	public class EmptyConstructorsTest
	{
		[Test]
		public void GivenStatements_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Statement[]
			{
				new ProcessesStatement(),
			}.TestParameterlessConstructors();
		}

		[Test]
		public void GivenQuestions_WhenCreateWithoutParameters_ThenSucceed()
		{
			new Question[]
			{
				new ProcessesQuestion(),
			}.TestParameterlessConstructors();
		}
	}
}
