using System;

namespace Inventor.Semantics.Metadata
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
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (type.IsAbstract || !typeof(IQuestion).IsAssignableFrom(type)) throw new ArgumentException($"Type must be non-abstract and implement {typeof(IQuestion)}.", nameof(type));
			if (questionNameGetter == null) throw new ArgumentNullException(nameof(questionNameGetter));

			Type = type;
			_questionNameGetter = questionNameGetter;
		}

		public String GetName(ILanguage language)
		{
			return _questionNameGetter(language);
		}
	}
}