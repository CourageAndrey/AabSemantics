using System;

using Inventor.Core.Localization;

namespace Inventor.Core
{
	public class QuestionDefinition
	{
		#region Properties

		public Type QuestionType
		{ get; }

		private readonly Func<ILanguage, string> _questionNameGetter;
		private readonly Func<Question> _questionFactory;
		private readonly Func<QuestionProcessor> _processorFactory;

		#endregion

		#region Constructors

		public QuestionDefinition(Type questionType, Func<ILanguage, string> questionNameGetter, Func<Question> questionFactory, Func<QuestionProcessor> processorFactory)
		{
			QuestionType = questionType;
			_questionNameGetter = questionNameGetter;
			_questionFactory = questionFactory;
			_processorFactory = processorFactory;
		}

		public QuestionDefinition(Type questionType, Type processorType)
			: this(
				questionType,
				language => (string) typeof(ILanguageQuestionNames).GetProperty(questionType.Name).GetValue(language.QuestionNames),
				() => Activator.CreateInstance(questionType) as Question,
				() => Activator.CreateInstance(processorType) as QuestionProcessor)
		{ }

		#endregion

		public string GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}

		public Question CreateQuestion()
		{
			return _questionFactory();
		}

		public QuestionProcessor CreateProcessor()
		{
			return _processorFactory();
		}
	}
}