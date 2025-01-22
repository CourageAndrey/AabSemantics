using System;

using AabSemantics.Statements;
using AabSemantics.Utils;

namespace AabSemantics.Metadata
{
	public delegate String CustomStatementStringDelegate(CustomStatement statement, ILanguage language);
	public delegate ILocalizedString CustomStatementLocalizedDelegate(CustomStatement statement);

	public class CustomStatementDefinition
	{
		#region Properties

		public String Type
		{ get; }

		private readonly CustomStatementStringDelegate _formatTrue;
		private readonly CustomStatementStringDelegate _formatFalse;
		private readonly CustomStatementStringDelegate _formatQuestion;
		private readonly CustomStatementLocalizedDelegate _nameCreator;
		private readonly CustomStatementLocalizedDelegate _hintCreator;

		#endregion

		public CustomStatementDefinition(
			String type,
			CustomStatementStringDelegate formatTrue,
			CustomStatementStringDelegate formatFalse,
			CustomStatementStringDelegate formatQuestion,
			CustomStatementLocalizedDelegate nameCreator,
			CustomStatementLocalizedDelegate hintCreator)
		{
			if (!String.IsNullOrEmpty(type))
			{
				Type = type;
			}
			else
			{
				throw new ArgumentNullException(nameof(type));
			}

			_formatTrue = formatTrue.EnsureNotNull(nameof(formatTrue));
			_formatFalse = formatFalse.EnsureNotNull(nameof(formatFalse));
			_formatQuestion = formatQuestion.EnsureNotNull(nameof(formatQuestion));
			_nameCreator = nameCreator.EnsureNotNull(nameof(nameCreator));
			_hintCreator = hintCreator.EnsureNotNull(nameof(hintCreator));
		}

		#region Description

		public String GetDescriptionTrueText(CustomStatement statement, ILanguage language)
		{
			return _formatTrue(statement, language);
		}

		public String GetDescriptionFalseText(CustomStatement statement, ILanguage language)
		{
			return _formatFalse(statement, language);
		}

		public String GetDescriptionQuestionText(CustomStatement statement, ILanguage language)
		{
			return _formatQuestion(statement, language);
		}

		public ILocalizedString CreateName(CustomStatement statement)
		{
			return _nameCreator(statement);
		}

		public ILocalizedString CreateHint(CustomStatement statement)
		{
			return _hintCreator(statement);
		}

		#endregion
	}
}
