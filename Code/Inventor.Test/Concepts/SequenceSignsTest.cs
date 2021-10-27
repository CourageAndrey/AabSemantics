using System;

using NUnit.Framework;

using Inventor.Semantics.Concepts;
using Inventor.Mathematics.Concepts;
using Inventor.Processes.Concepts;

namespace Inventor.Test.Concepts
{
	[TestFixture]
	public class SequenceSignsTest
	{
		[Test]
		public void OnlySequenceSignsSuit()
		{
			foreach (var concept in SystemConcepts.GetAll())
			{
				if (!SequenceSigns.All.Contains(concept))
				{
					Assert.Throws<InvalidOperationException>(() => { concept.Contradicts(SequenceSigns.SimultaneousWith); });
					Assert.Throws<InvalidOperationException>(() => { SequenceSigns.SimultaneousWith.Contradicts(concept); });

					Assert.Throws<InvalidOperationException>(() => { SequenceSigns.Revert(concept); });

					Assert.Throws<InvalidOperationException>(() => { SequenceSigns.TryToCombineMutualSequences(concept, SequenceSigns.SimultaneousWith); });
					Assert.Throws<InvalidOperationException>(() => { SequenceSigns.TryToCombineMutualSequences(SequenceSigns.SimultaneousWith, concept); });
				}
			}
		}

		[Test]
		public void DoubleReversionDoNothing()
		{
			foreach (var sign in SequenceSigns.All)
			{
				Assert.AreSame(sign, SequenceSigns.Revert(SequenceSigns.Revert(sign)));
			}
		}
	}
}
