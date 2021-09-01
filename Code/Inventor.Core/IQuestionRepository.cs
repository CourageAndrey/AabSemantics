using System;
using System.Collections.Generic;

namespace Inventor.Core
{
	public interface IQuestionRepository
	{
		IDictionary<Type, QuestionDefinition> Definitions
		{ get; }

		void Define(QuestionDefinition questionDefinition);
	}
}
