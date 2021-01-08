using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class QuestionProcessingMechanism
	{
		public ICollection<Type> SupportedQuestionTypes
		{ get { return _allProcessors.Keys; } }

		private readonly IDictionary<Type, Func<QuestionProcessor>> _allProcessors = new Dictionary<Type, Func<QuestionProcessor>>();

		public QuestionProcessingMechanism()
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

		public FormattedText Process(KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			Func<QuestionProcessor> processorFactory;
			if (_allProcessors.TryGetValue(question.GetType(), out processorFactory))
			{
				var processor = processorFactory();
				return processor.Process(knowledgeBase, question, language);
			}
			else
			{
				throw new KeyNotFoundException(language.ErrorsInventor.UnknownQuestion);
			}
		}
	}
}
