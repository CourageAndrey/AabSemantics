using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventor.Core.Base
{
	public class QuestionRepository : IQuestionRepository
	{
		public IDictionary<Type, QuestionDefinition> QuestionDefinitions
		{ get; }

		public QuestionRepository()
		{
			QuestionDefinitions = new Dictionary<Type, QuestionDefinition>();

			var questionClass = typeof(IQuestion);
			foreach (var questionType in questionClass.Assembly.GetTypes().Where(t => questionClass.IsAssignableFrom(t) && !t.IsAbstract))
			{
				DefineQuestion(new QuestionDefinition(questionType));
			}
		}

		public void DefineQuestion(QuestionDefinition questionDefinition)
		{
			QuestionDefinitions[questionDefinition.QuestionType] = questionDefinition;
		}
	}
}
