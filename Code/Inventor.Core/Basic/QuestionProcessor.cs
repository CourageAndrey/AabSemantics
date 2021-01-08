using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public abstract class QuestionProcessor
	{
		public static FormattedText Process(KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			QuestionProcessor processor;
			if (_allProcessors.TryGetValue(question.GetType(), out processor))
			{
				return processor.ProcessInternal(knowledgeBase, question, language);
			}
			else
			{
				throw new KeyNotFoundException(language.ErrorsInventor.UnknownQuestion);
			}
		}

		private static readonly IDictionary<Type, QuestionProcessor> _allProcessors = new Dictionary<Type, QuestionProcessor>();

		static QuestionProcessor()
		{
			foreach (var processorType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof (QuestionProcessor).IsAssignableFrom(t) && !t.IsAbstract))
			{
				var type = processorType;
				while (!type.IsGenericType || type.GetGenericTypeDefinition() != typeof (QuestionProcessor<>))
				{
					type = type.BaseType;
				}
				_allProcessors[type.GetGenericArguments()[0]] = Activator.CreateInstance(processorType) as QuestionProcessor;
			}
		}

		protected abstract FormattedText ProcessInternal(KnowledgeBase knowledgeBase, Question question, ILanguage language);
	}

	public abstract class QuestionProcessor<QuestionT> : QuestionProcessor
		where QuestionT : Question
	{
		protected abstract FormattedText ProcessImplementation(KnowledgeBase knowledgeBase, QuestionT question, ILanguage language);

		protected override FormattedText ProcessInternal(KnowledgeBase knowledgeBase, Question question, ILanguage language)
		{
			if (question.GetType() == typeof (QuestionT))
			{
				return ProcessImplementation(knowledgeBase, question as QuestionT, language);
			}
			else
			{
				throw new ArrayTypeMismatchException();
			}
		}
	}
}