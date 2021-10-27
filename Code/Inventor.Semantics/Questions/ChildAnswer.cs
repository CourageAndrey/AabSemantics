using System.Collections.Generic;

namespace Inventor.Semantics.Questions
{
	public class ChildAnswer
	{
		public IQuestion Question
		{ get; }

		public IAnswer Answer
		{ get; }

		public ICollection<IStatement> TransitiveStatements
		{ get; }

		public ChildAnswer(IQuestion question, IAnswer answer, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			Answer = answer;
			TransitiveStatements = transitiveStatements;
		}
	}
}