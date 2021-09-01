using System;

namespace Inventor.Core
{
	public class QuestionDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		private readonly Func<ILanguage, String> _questionNameGetter;

		#endregion

		#region Constructors

		public QuestionDefinition(Type type, Func<ILanguage, String> questionNameGetter)
		{
			Type = type;
			_questionNameGetter = questionNameGetter;
		}

		public QuestionDefinition(Type type)
			: this(type, language => (String) typeof(ILanguageQuestionNames).GetProperty(type.Name).GetValue(language.QuestionNames))
		{ }

		#endregion

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}
	}
}