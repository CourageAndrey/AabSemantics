using System;

namespace Inventor.Core
{
	public class QuestionDefinition
	{
		#region Properties

		public Type QuestionType
		{ get; }

		private readonly Func<ILanguage, String> _questionNameGetter;

		#endregion

		#region Constructors

		public QuestionDefinition(Type questionType, Func<ILanguage, String> questionNameGetter)
		{
			QuestionType = questionType;
			_questionNameGetter = questionNameGetter;
		}

		public QuestionDefinition(Type questionType)
			: this(questionType, language => (String) typeof(ILanguageQuestionNames).GetProperty(questionType.Name).GetValue(language.QuestionNames))
		{ }

		#endregion

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}
	}
}