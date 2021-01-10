using System;

namespace Inventor.Core
{
	public class QuestionDefinition
	{
		#region Properties

		public Type QuestionType
		{ get; }

		private readonly Func<ILanguage, String> _questionNameGetter;
		private readonly Func<IQuestion> _questionFactory;
		private readonly Func<IQuestionProcessor> _processorFactory;

		#endregion

		#region Constructors

		public QuestionDefinition(Type questionType, Func<ILanguage, String> questionNameGetter, Func<IQuestion> questionFactory, Func<IQuestionProcessor> processorFactory)
		{
			QuestionType = questionType;
			_questionNameGetter = questionNameGetter;
			_questionFactory = questionFactory;
			_processorFactory = processorFactory;
		}

		public QuestionDefinition(Type questionType, Type processorType)
			: this(
				questionType,
				language => (String) typeof(ILanguageQuestionNames).GetProperty(questionType.Name).GetValue(language.QuestionNames),
				() => Activator.CreateInstance(questionType) as IQuestion,
				() => Activator.CreateInstance(processorType) as IQuestionProcessor)
		{ }

		#endregion

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}

		public IQuestion CreateQuestion()
		{
			return _questionFactory();
		}

		public IQuestionProcessor CreateProcessor()
		{
			return _processorFactory();
		}
	}
}