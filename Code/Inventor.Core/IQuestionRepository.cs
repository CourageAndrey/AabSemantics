using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestionRepository
	{
		IDictionary<Type, QuestionDefinition> QuestionDefinitions
		{ get; }

		void DefineQuestion(QuestionDefinition questionDefinition);
	}
}
