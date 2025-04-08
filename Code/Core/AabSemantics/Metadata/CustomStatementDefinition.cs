using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;
using AabSemantics.Statements;

namespace AabSemantics.Metadata
{
	public class CustomStatementDefinition : StatementDefinition, INamed
	{
		#region Properties

		public String Kind
		{ get; }

		public ICollection<String> Concepts
		{ get; }

		internal static readonly Func<ILanguage, String> GetStatementName = language => language.Statements.CustomStatementName;

		#endregion

		public CustomStatementDefinition(
			String kind,
			ICollection<String> concepts,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion)
		: base(
			typeof(CustomStatement),
			GetStatementName,
			formatTrue,
			formatFalse,
			formatQuestion,
			statement => ((CustomStatement) statement).Concepts.ToDictionary(
				p => p.Key,
				p => p.Value as IKnowledge),
			NoConsistencyCheck)
		{
			if (!String.IsNullOrEmpty(kind))
			{
				Kind = kind;
			}
			else
			{
				throw new ArgumentNullException(nameof(kind));
			}

			Concepts = concepts ?? throw new ArgumentNullException(nameof(concepts));
		}

		public override string ToString()
		{
			return Kind;
		}

		public ILocalizedString Name
		{ get { return _name ?? (_name = new LocalizedStringConstant(l => Kind)); } }

		private ILocalizedString _name;
	}
}
