using System;

namespace Inventor.Core.Metadata
{
	public class QuestionDefinition : IMetadataDefinition
	{
		#region Properties

		public Type Type
		{ get; }

		private readonly Func<ILanguage, String> _questionNameGetter;

		#endregion

		public QuestionDefinition(Type type, Func<ILanguage, String> questionNameGetter)
		{
			Type = type;
			_questionNameGetter = questionNameGetter;
		}

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}
	}
}