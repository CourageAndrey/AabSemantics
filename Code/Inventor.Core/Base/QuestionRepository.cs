using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class QuestionRepository : IQuestionRepository
	{
		public IDictionary<Type, QuestionDefinition> Definitions
		{ get; } = new Dictionary<Type, QuestionDefinition>();

		public void Define(QuestionDefinition questionDefinition)
		{
			Definitions[questionDefinition.Type] = questionDefinition;
		}
	}
}
