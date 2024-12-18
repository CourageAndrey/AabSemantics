using System;

using NUnit.Framework;

using AabSemantics.Modules.Boolean.Concepts;
using AabSemantics.Modules.Processes.Concepts;

namespace AabSemantics.Modules.Processes.Tests.Concepts
{
	[TestFixture]
	public class SequenceSignsTest
	{
		[Test]
		public void GivenNonSequenceSigns_WhenTryToCallSequenceSignExtensions_ThenFail()
		{
			foreach (var concept in LogicalValues.All)
			{
				Assert.Throws<InvalidOperationException>(() => { new[] { concept, SequenceSigns.SimultaneousWith }.Contradicts(); });

				Assert.Throws<InvalidOperationException>(() => { concept.Revert(); });

				Assert.Throws<InvalidOperationException>(() => { SequenceSigns.TryToCombineMutualSequences(concept, SequenceSigns.SimultaneousWith); });
				Assert.Throws<InvalidOperationException>(() => { SequenceSigns.TryToCombineMutualSequences(SequenceSigns.SimultaneousWith, concept); });
			}
		}

		[Test]
		public void GivenSequenceSigns_WhenRevertTwice_ThenGetTheSame()
		{
			foreach (var sign in SequenceSigns.All)
			{
				Assert.That(sign.Revert().Revert(), Is.SameAs(sign));
			}
		}
	}
}
