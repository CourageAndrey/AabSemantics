using System;
using System.Collections.Generic;
using System.Linq;

using AabSemantics.Localization;

namespace AabSemantics.Statements
{
	public class CustomStatement : Statement<CustomStatement>
	{
		#region Properties

		public String Type
		{ get; private set; }

		public Func<ILanguage, String> FormatTrue
		{ get; private set; }

		public Func<ILanguage, String> FormatFalse
		{ get; private set; }

		public Func<ILanguage, String> FormatQuestion
		{ get; private set; }

		public IDictionary<String, IConcept> Concepts
		{ get; private set; }

		#endregion

		public CustomStatement(
			String id,
			String type,
			Func<ILanguage, String> formatTrue,
			Func<ILanguage, String> formatFalse,
			Func<ILanguage, String> formatQuestion,
			LocalizedString name,
			LocalizedString hint = null,
			IDictionary<String, IConcept> concepts = null)
			: base(id, name, hint)
		{
			if (!String.IsNullOrEmpty(type))
			{
				Type = type;
			}
			else
			{
				throw new ArgumentNullException(nameof(type));
			}

			FormatTrue = formatTrue;
			FormatFalse = formatFalse;
			FormatQuestion = formatQuestion;

			Update(id, concepts);
		}

		public CustomStatement(
			String id,
			String type,
			String formatTrue,
			String formatFalse,
			String formatQuestion,
			LocalizedString name,
			LocalizedString hint = null,
			IDictionary<String, IConcept> concepts = null)
			: this(
				id,
				type,
				language => formatTrue,
				language => formatFalse,
				language => formatQuestion,
				name,
				hint,
				concepts)
		{ }

		public void Update(String id, IDictionary<String, IConcept> concepts)
		{
			Update(id);
			Concepts = new Dictionary<string, IConcept>(concepts ?? new Dictionary<String, IConcept>());
		}

		public override IEnumerable<IConcept> GetChildConcepts()
		{
			return Concepts.Values;
		}

		#region Description

		protected override String GetDescriptionTrueText(ILanguage language)
		{
			return FormatTrue(language);
		}

		protected override String GetDescriptionFalseText(ILanguage language)
		{
			return FormatFalse(language);
		}

		protected override String GetDescriptionQuestionText(ILanguage language)
		{
			return FormatQuestion(language);
		}

		protected override IDictionary<String, IKnowledge> GetDescriptionParameters()
		{
			return Concepts.ToDictionary(
				p => $"#{p.Key}#",
				p => p.Value as IKnowledge);
		}

		#endregion

		#region Consistency checking

		public override Boolean Equals(CustomStatement other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (other != null)
			{
				return	other.Type == Type &&
						other.Concepts.SequenceEqual(Concepts);
			}
			else return false;
		}

		#endregion
	}
}
