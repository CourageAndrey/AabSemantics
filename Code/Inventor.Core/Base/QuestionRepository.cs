using System;
using System.Collections.Generic;

namespace Inventor.Core.Base
{
	public class QuestionRepository : IQuestionRepository
	{
		public IDictionary<Type, QuestionDefinition> QuestionDefinitions
		{ get; } = new Dictionary<Type, QuestionDefinition>();

		public void DefineQuestion(QuestionDefinition questionDefinition)
		{
			QuestionDefinitions[questionDefinition.QuestionType] = questionDefinition;
		}
	}
}
