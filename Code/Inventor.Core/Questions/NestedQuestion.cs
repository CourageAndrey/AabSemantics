using System.Collections.Generic;

namespace Inventor.Core.Questions
{
	public class NestedQuestion
	{
		public IQuestion Question
		{ get; }

		public ICollection<IStatement> TransitiveStatements
		{ get; }

		public NestedQuestion(IQuestion question, ICollection<IStatement> transitiveStatements)
		{
			Question = question;
			TransitiveStatements = transitiveStatements;
		}
	}
}