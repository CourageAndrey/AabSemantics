using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class QuestionProcessorRepository
	{
		public ICollection<Type> SupportedQuestionTypes
		{ get { return _allProcessors.Keys; } }

		private readonly IDictionary<Type, Func<QuestionProcessor>> _allProcessors = new Dictionary<Type, Func<QuestionProcessor>>();

		public QuestionProcessorRepository()
		{
			foreach (var processorType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(QuestionProcessor).IsAssignableFrom(t) && !t.IsAbstract))
			{
				var type = processorType;
				while (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof(QuestionProcessor<>))
				{
					type = type.BaseType;
				}
				RegisterQuestionProcessor(
					type.GetGenericArguments()[0],
					() => Activator.CreateInstance(processorType) as QuestionProcessor);
			}
		}

		public void RegisterQuestionProcessor(Type questionType, Func<QuestionProcessor> processorFactory)
		{
			_allProcessors[questionType] = processorFactory;
		}

		public QuestionProcessor CreateQuestionProcessor(Question question, ILanguage language)
		{
			Func<QuestionProcessor> processorFactory;
			if (_allProcessors.TryGetValue(question.GetType(), out processorFactory))
			{
				return processorFactory();
			}
			else
			{
				throw new KeyNotFoundException(language.ErrorsInventor.UnknownQuestion);
			}
		}
	}
}
