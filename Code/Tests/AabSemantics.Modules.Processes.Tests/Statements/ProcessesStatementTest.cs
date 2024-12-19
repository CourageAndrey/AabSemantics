﻿using NUnit.Framework;

using AabSemantics.Concepts;
using AabSemantics.Modules.Processes.Attributes;
using AabSemantics.Modules.Processes.Concepts;
using AabSemantics.Modules.Processes.Statements;

namespace AabSemantics.Modules.Processes.Tests.Statements
{
	[TestFixture]
	public class ProcessesStatementTest
	{
		[Test]
		public void GivenNullOther_WhenEquals_ThenReturnFalse()
		{
			// arrange
			IConcept	process1 = 1.CreateConceptByObject().WithAttribute(IsProcessAttribute.Value),
						process2 = 2.CreateConceptByObject().WithAttribute(IsProcessAttribute.Value);
			var statement = new ProcessesStatement(null, process1, process2, SequenceSigns.Causes);

			// act && assert
			Assert.That(statement.Equals(null), Is.False);
		}
	}
}
