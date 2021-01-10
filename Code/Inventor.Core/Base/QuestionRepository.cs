using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Inventor.Core.Base
{
	public class QuestionRepository : IQuestionRepository
	{
		public IDictionary<Type, QuestionDefinition> QuestionDefinitions
		{ get; }

		public QuestionRepository()
		{
			QuestionDefinitions = new Dictionary<Type, QuestionDefinition>();

			foreach (var processorType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(QuestionProcessor).IsAssignableFrom(t) && !t.IsAbstract))
			{
				var type = processorType;
				while (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(QuestionProcessor<>))
				{
					type = type.BaseType;
				}
				var questionType = type.GetGenericArguments()[0];
				DefineQuestion(new QuestionDefinition(questionType, processorType));
			}
		}

		public void DefineQuestion(QuestionDefinition questionDefinition)
		{
			QuestionDefinitions[questionDefinition.QuestionType] = questionDefinition;
		}
	}
}
